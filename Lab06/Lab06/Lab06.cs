using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;

namespace ASD
{
    public class Lab06 : MarshalByRefObject
    {
        /// <summary>Etap I</summary>
        /// <param name="G">Graf opisujący połączenia szlakami turystycznymi z podanym czasem przejścia krawędzi w wadze.</param>
        /// <param name="waitTime">Czas oczekiwania Studenta-Podróżnika w danym wierzchołku.</param>
        /// <param name="s">Wierzchołek startowy (początek trasy).</param>
        /// <returns>Pierwszy element krotki to wierzchołek końcowy szukanej trasy. Drugi element to długość trasy w minutach. Trzeci element to droga będąca rozwiązaniem: sekwencja odwiedzanych wierzchołków (zawierająca zarówno wierzchołek początkowy, jak i końcowy).</returns>
        public (int t, int l, int[] path) Stage1(DiGraph<int> G, int[] waitTime, int s)
        {
            //pomysl: bedziemy dawac wartosci ujemne do kolejki priorytetowej tak aby znalezc najkrotsza trase
            // do kazdego wierzcholka w father przetrzymujmey ojca aby moc sie do niego cofnac pozniej
            int[] odp = new int[G.VertexCount];
            int[] father = new int[G.VertexCount];
            int time = 0; // l
            int end = -1; // t
            void Dijkstra(DiGraph<int> G, int[] waitTime, int s)
            {
                for(int i=0; i<G.VertexCount; i++)
                {
                    odp[i] = int.MaxValue;
                    father[i] = -1;
                }
                var pq = new PriorityQueue<int, int> ();
                pq.Insert(s, 0);
                odp[s] = 0;

                while (pq.Count > 0)
                {
                    int vertex = pq.Extract();
                    int CurrTime = odp[vertex];
                    
                    foreach(int neighbor in G.OutNeighbors(vertex))
                    {
                        int edgeWeight = G.GetEdgeWeight(vertex, neighbor);
                        int newTime = CurrTime + edgeWeight + waitTime[neighbor];
                        /*Console.WriteLine($"Vertex: {vertex}, Neighbor: {neighbor}, " +
                            $"CurrTime: {CurrTime}, EdgeWeight: {edgeWeight}, " +
                            $"WaitTime: {waitTime[neighbor]}," +
                            $" NewTime: {newTime}, " +
                            $"Current odp[neighbor]: {odp[neighbor]}");*/

                        if (newTime < odp[neighbor])
                        {
                            odp[neighbor] = newTime;
                            father[neighbor] = vertex;
                            pq.Insert(neighbor, newTime);
                        }
                    }
                }
            }

            
            int[] GetPath()
            {
                int endOfPath = s;
                for(int i=0; i< G.VertexCount; i++)
                {
                    if (odp[i] < int.MaxValue && odp[i] - waitTime[i] > odp[endOfPath] - waitTime[endOfPath])
                    {
                        //if (i != s)
                        endOfPath = i;
                    }
                }
                end = endOfPath;
                if(endOfPath != s)
                    time = odp[endOfPath] - waitTime[endOfPath];

                List<int> path = new List<int> ();
                while(endOfPath != -1)
                {
                    path.Add(endOfPath);
                    endOfPath = father[endOfPath];
                }
                path.Reverse();

                return path.ToArray();
            }

            //Sam program
            Dijkstra(G, waitTime, s);
            int[] path = GetPath();


            return (end, time, path);
        }

        /// <summary>Etap II</summary>
        /// <param name="G">Graf opisujący połączenia szlakami turystycznymi z podanym czasem przejścia krawędzi w wadze.</param>
        /// <param name="C">Graf opisujący koszty przejścia krawędziami w grafie G.</param>
        /// <param name="waitTime">Czas oczekiwania Studenta-Podróżnika w danym wierzchołku.</param>
        /// <param name="s">Wierzchołek startowy (początek trasy).</param>
        /// <param name="t">Wierzchołek końcowy (koniec trasy).</param>
        /// <returns>Pierwszy element krotki to długość trasy w minutach. Drugi element to koszt przebycia trasy w złotych. Trzeci element to droga będąca rozwiązaniem: sekwencja odwiedzanych wierzchołków (zawierająca zarówno wierzchołek początkowy, jak i końcowy). Jeśli szukana trasa nie istnieje, funkcja zwraca `null`.</returns>
        public (int l, int c, int[] path)? Stage2(DiGraph<int> G, Graph<int> C, int[] waitTime, int s, int t)
        {
            return null;
        }
    }
}