using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ASD
{
    public class Lab10 : MarshalByRefObject
    {
        /// <summary>
        /// Szukanie najdłuższego powtórzenia w zadanym kolorowaniu grafu.
        /// </summary>
        /// <param name="G">Graf prosty</param>
        /// <param name="color">Kolorowanie wierzchołków G (color[v] to kolor wierzchołka v)</param>
        /// <returns>Ścieżka, na której występuje powtórzenie (numery kolejnych wierzchołków)</returns>
        /// <remarks>W przypadku braku powtórzeń należy zwrócić null lub tablicę o długości 0</remarks>
    
        //rozwiazanie z dobra iloscia optymalizacji:
        //********************************************************************************************************************************
       
        public int[] FindLongestRepetitionIdea(Graph G, int[] color)
        {
            int n = color.Length;
            List<int> longestPath = null;
            int maxRepetitionLength = 0;

            bool[] visited = new bool[n];
             

            var colorFreq = new Dictionary<int, int>();
            foreach(int c in color){
                if (!colorFreq.ContainsKey(c)) colorFreq[c] = 0;
                colorFreq[c]++;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    //przechodze po kazdej trasie z jednego wierzchola 
                        // jesli trasa jest nie mozliwa z tego wierzcholka to koniec tej trasy 
                        // jesli traca polaczy wierzcholek v_i do v_j  
                        // jesli trasa obecnie znaleziona jest lepsza ( dluzsza od tego co mielismy wczesniej to nadpisujemy longestPath
                        
                }
            }
            return longestPath?.ToArray() ?? Array.Empty<int>();
        }

        public int[] FindLongestRepetition(Graph G, int[] color)
        {
            void printPaths(List<int> path1, List<int> path2)
            {
                Console.Write("Path1:");
                foreach (var v in path1)
                {
                    Console.Write(v + ",");
                }

                Console.WriteLine();
                Console.Write("Path2:");
                foreach (var v in path2)
                {
                    Console.Write(v + ",");
                }
            }

            int n = color.Length;
            int[] longestPath = new int[0];

            var colorFreq = new Dictionary<int, int>();
            foreach (int c in color)
            {
                if (!colorFreq.ContainsKey(c)) colorFreq[c] = 0;
                colorFreq[c]++;
            }

            void FindPath(bool[] visited, List<int> path1, List<int> path2)
            {
                int end1 = path1.Last();
                int end2 = path2.Last();

                if (path1.Count * 2 > longestPath.Length)
                {
                    if (G.HasEdge(end1, path2[0]))
                    {
                        var sumPath = new List<int>(path1);
                        sumPath.AddRange(path2);
                        longestPath = sumPath.ToArray();
                    }

                    if (G.HasEdge(end2, path1[0]))
                    {
                        var sumPath = new List<int>(path2);
                        sumPath.AddRange(path1);
                        longestPath = sumPath.ToArray();
                    }
                }


                foreach (int v in G.OutNeighbors(end1))
                {
                    if(visited[v]) continue; // fajna optymalizacja
                    foreach (int u in G.OutNeighbors(end2))
                    {
                        if (color[u] == color[v] && visited[u]==false  && visited[v]==false && u!=v)
                        {
                            visited[u] = true;
                            visited[v] = true;
                            path1.Add(v);
                            path2.Add(u);
                            
                            FindPath(visited, path1, path2);
                            
                            visited[u] = false;
                            visited[v] = false;
                            path1.Remove(v);
                            path2.Remove(u);
                        }
                    }
                }
            }
            for (int i = 0; i < n; i++)
            {
                if (colorFreq[color[i]] < 2) continue; // nie ma sensu brak kolorów gdzie jest ich mniej niz 2

                for (int j = i+1; j < n; j++) // czy nie j=0?
                {
                    if( color[i] != color[j]) continue; //nie ma sensu brac kolorów ponizej 2
                    var visited = new bool[n];
                    var path1 = new List<int>(){i};
                    var path2 = new List<int>(){j};
                    visited[i] = true;
                    visited[j] = true;
                    FindPath(visited, path1, path2);
                }
            }
            return longestPath?.ToArray() ?? Array.Empty<int>();
        }
    }
}




