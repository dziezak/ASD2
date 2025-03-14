﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Xml;

namespace ASD
{
    public class Lab02 : MarshalByRefObject
    {
        public struct Pair
        {
            public int x;
            public int y;
            public Pair(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        private int[,] moveCost;
        /// <summary>
        /// Etap 1 - wyznaczenie najtańszej trasy, zgodnie z którą pionek przemieści się z pozycji poczatkowej (0,0) na pozycję docelową
        /// </summary>
        /// <param name="n">wysokość prostokąta</param>
        /// <param name="m">szerokość prostokąta</param>
        /// <param name="moves">tablica z dostępnymi ruchami i ich kosztami (di - o ile zwiększamy numer wiersza, dj - o ile zwiększamy numer kolumnj, cost - koszt ruchu)</param>
        /// <returns>(bool result, int cost, (int, int)[] path) - result ma wartość true jeżeli trasa istnieje, false wpp., cost to minimalny koszt, path to wynikowa trasa</returns>
        public (bool result, int cost, (int i, int j)[] path) Lab02Stage1(int n, int m, ((int di, int dj) step, int cost)[] moves)
        {
            bool wys = false;
            List<(int, int)> path = new List<(int, int)> ();
            bool result = false;
            int resultVal = int.MaxValue;
            int[,] moveCost = new int[n, m];
            (int di,int dj)[,] lastMove = new (int, int)[n, m];
            int movesCount = moves.Length;

            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    moveCost[i,j] = int.MaxValue;
                }
            }
            moveCost[0,0] = 0;
            PriorityQueue<Pair, int> prioQ = new PriorityQueue<Pair, int>();
            prioQ.Enqueue(new Pair(0, 0), 0);
            while ( prioQ.Count > 0 )
            {
                prioQ.TryDequeue(out Pair item, out int priority);
                //Console.WriteLine($"Element: ({item.y}, {item.x}), Priorytet: {priority}");
                if(item.y == n - 1)
                {
                    result = true;
                }
                for(int i=0; i<movesCount; i++)
                {
                    Pair p = new Pair(item.x + moves[i].step.dj, item.y + moves[i].step.di);
                    int newCost = priority + moves[i].cost; 
                    if(p.x < m && p.y < n && newCost < moveCost[p.y, p.x])
                    {
                        moveCost[p.y, p.x] = newCost;
                        prioQ.Enqueue(p, newCost);
                        lastMove[p.y, p.x] = (item.y, item.x);
                    }
                }
            }
            if (wys)
            {
                Console.WriteLine();
                for(int i=0; i<n; i++)
                {
                    for(int j=0; j<m; j++)
                    {
                        Console.Write(moveCost[i,j]+" ");
                    }
                    Console.WriteLine();
                }
            }
            if (result)
            {
                int i = n - 1;
                int j = 0;
                for (int col = 0; col < m; col++)
                {
                    if (moveCost[i, col] < moveCost[i, j])
                    {
                        j = col;
                        resultVal = moveCost[i, col];
                    }
                }

                while (i != 0 || j != 0)
                {
                    path.Add((i, j));
                    (i, j) = lastMove[i, j];
                }
                path.Add((0, 0));
                path.Reverse();
            }
            return (result, resultVal, path.ToArray());
        }


        /// <summary>
        /// Etap 2 - wyznaczenie najtańszej trasy, zgodnie z którą pionek przemieści się z pozycji poczatkowej (0,0) na pozycję docelową - dodatkowe założenie, każdy ruch może być wykonany co najwyżej raz
        /// </summary>
        /// <param name="n">wysokość prostokąta</param>
        /// <param name="m">szerokość prostokąta</param>
        /// <param name="moves">tablica z dostępnymi ruchami i ich kosztami (di - o ile zwiększamy numer wiersza, dj - o ile zwiększamy numer kolumnj, cost - koszt ruchu)</param>
        /// <returns>(bool result, int cost, (int, int)[] path) - result ma wartość true jeżeli trasa istnieje, false wpp., cost to minimalny koszt, path to wynikowa trasa</returns>
        public struct State
        {
            public int i, j, usedMoves;
            public State(int i, int j, int usedMoves)
            {
                this.i = i;
                this.j = j;
                this.usedMoves = usedMoves;
            }
        }
        public (bool result, int cost, (int i, int j)[] pat) Lab02Stage2(int n, int m, ((int di, int dj) step, int cost)[] moves)
        {
            int movesCount = moves.Length;
            int[,,] moveCost = new int[n,m,1<<movesCount];
            State[,,] lastMove = new State[n,m,1<<movesCount];
            PriorityQueue<State, int> prioQ = new();

            for(int x = 0; x< n; x++)
            {
                for(int y = 0; y< m; y++)
                {
                    for(int  z = 0; z<(1<<movesCount); z++)
                    {
                        moveCost[x,y,z] = int.MaxValue;
                    }
                }
            }

            moveCost[0,0,0] = 0;
            prioQ.Enqueue(new State(0, 0, 0), 0);

            while(prioQ.Count > 0)
            {
                prioQ.TryDequeue(out var state, out int cost);
                int i = state.i;
                int j = state.j;
                int usedMoves = state.usedMoves;

                if(i == n - 1)
                {
                    List<(int, int)> path = new();
                    while (!(state.i == 0 && state.j == 0 && state.usedMoves == 0))
                    {
                        path.Add((state.i, state.j));
                        state = lastMove[state.i, state.j, state.usedMoves];
                    }
                    path.Add((0, 0));
                    path.Reverse();
                    return (true, cost, path.ToArray());
                }

                for(int moveIdx = 0; moveIdx < movesCount; moveIdx++)
                {
                    if ((usedMoves & (1 << moveIdx)) != 0) continue;

                    int ni = i + moves[moveIdx].step.di;
                    int nj = j + moves[moveIdx].step.dj;
                    int newUsedMoves = usedMoves | (1 << moveIdx);
                    int newCost = cost + moves[moveIdx].cost;

                    if (ni < n && nj < m && nj >= 0) 
                    {
                        var newState = new State(ni, nj, newUsedMoves);
                        if(newCost < moveCost[ni, nj, newUsedMoves])
                        {
                            moveCost[ni, nj, newUsedMoves] = newCost;
                            prioQ.Enqueue(new State(ni, nj, newUsedMoves), newCost);
                            lastMove[ni, nj, newUsedMoves] = state;
                        }
                    }
                }
            }
            return (false, int.MaxValue, null);
        }
    }
}