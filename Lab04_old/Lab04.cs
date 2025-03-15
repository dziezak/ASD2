using System;
using System.Collections;
using ASD.Graphs;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace ASD
{
    public class Lab04 : MarshalByRefObject
    {
        /// <summary>
        /// Etap 1 - wyznaczanie numerów grup, które jest w stanie odwiedzić Karol, zapisując się na początku do podanej grupy
        /// </summary>
        /// <param name="graph">Ważony graf skierowany przedstawiający zasady dołączania do grup</param>
        /// <param name="start">Numer grupy, do której początkowo zapisuje się Karol</param>
        /// <returns>Tablica numerów grup, które może odwiedzić Karol, uporządkowana rosnąco</returns>
        public int[] Lab04Stage1(DiGraph<int> graph, int start)
        {
            bool[] knowlage = new bool[graph.VertexCount];
            knowlage[start] = true;

            void BFS(int v, int parent)
            {
                bool[] visited = new bool[graph.VertexCount];
                Queue<(int v, int parnet)> queue = new Queue<(int v, int parent)>();
                queue.Enqueue((v, parent));
                visited[v] = true;
                knowlage[v] = true;
                while (queue.Count > 0)
                {
                    var (vOut, parentOut) = queue.Dequeue();
                    foreach (var vertex in graph.OutNeighbors(vOut))
                    {
                        if( visited[vertex] ) continue;
                        if (parentOut == graph.GetEdgeWeight(vOut, vertex) || parentOut == -1)
                        {
                            queue.Enqueue((vertex, vOut));
                            visited[vertex] = true;
                            knowlage[vertex] = true;
                        }
                    }
                }
            }
            BFS(start, -1);
            

            List<int> result = new List<int>();
            for (int i = 0; i < graph.VertexCount; i++)
            {
                if (knowlage[i]) 
                    result.Add(i);
            }

            return result.ToArray();
        }


        /// <summary>
        /// Etap 2 - szukanie możliwości przejścia z jednej z grup z `starts` do jednej z grup z `goals`
        /// </summary>
        /// <param name="graph">Ważony graf skierowany przedstawiający zasady dołączania do grup</param>
        /// <param name="starts">Tablica z numerami grup startowych (trasę należy zacząć w jednej z nich)</param>
        /// <param name="goals">Tablica z numerami grup docelowych (trasę należy zakończyć w jednej z nich)</param>
        /// <returns>(possible, route) - `possible` ma wartość true gdy istnieje możliwość przejścia, wpp. false, 
        /// route to tablica z numerami kolejno odwiedzanych grup (pierwszy numer to numer grupy startowej, ostatni to numer grupy docelowej),
        /// jeżeli possible == false to route ustawiamy na null</returns>
        public (bool possible, int[] route) Lab04Stage2(DiGraph<int> graph, int[] starts, int[] goals)
        {
            // TODO
            return (false, null);
        }
    }
}
