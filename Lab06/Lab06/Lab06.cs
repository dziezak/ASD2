using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

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
            int[] odl = new int[G.VertexCount];
            int[] father = new int[G.VertexCount];
            int maxCost;
            // Ta funkcja jest do poprawy bo G jest skierowany a C jest nieskierowany dlatego przechodzenie po nich nie da zawsze dobrej odpowiedzi
            // np. cykl 0->1->2->3->4->0 z wagami 1 wszedzie s = 0 t = 4 , FindMin powie, że cena jest 1 a powinna byc 4
            int FindMin(Graph<int> C, int s, int t)
            {
                int[] cost  = new int[C.VertexCount];
                var pq = new PriorityQueue<int, int>();
                for (int i = 0; i < C.VertexCount; i++)
                {
                    cost[i] = int.MaxValue; 
                }
                pq.Insert(s, 0);
                cost[s] = 0;
                while (pq.Count > 0)
                {
                    int vertex = pq.Extract();
                    foreach (var neighbor in G.OutNeighbors(vertex)) // zamiana na G bo C jest nieskierowane
                    {
                        int newCost = cost[vertex] + C.GetEdgeWeight(vertex, neighbor); // tutaj ba byc C bo to koszty
                        if (newCost < cost[neighbor])
                        {
                            cost[neighbor] = newCost;
                            pq.Insert(neighbor, newCost);
                        }
                    }
                }
                return cost[t] == int.MaxValue ? 0 : cost[t];
            }
            
            void Dijkstra(DiGraph<int> G, Graph<int> C, int s, int t)// Dijkstra, ale nie przekraczamy cost[t];
            {
                maxCost = FindMin(C, s, t); // bedzie dzialac tylko jesli odpalimy po Findmin
                int[] cost = new int[G.VertexCount];
                for(int i=0; i<G.VertexCount; i++)
                {
                    odl[i] = int.MaxValue;
                    cost[i] = int.MaxValue;
                    father[i] = -1;
                }
                var pq = new PriorityQueue<int, int> ();
                pq.Insert(s, 0);
                odl[s] = 0;
                cost[s] = 0;
                while (pq.Count > 0)
                {
                    int vertex = pq.Extract();
                    int currOdl = odl[vertex];
                    int currCost = cost[vertex];
                    
                    foreach(int neighbor in G.OutNeighbors(vertex))
                    {
                        int edgeWeight = G.GetEdgeWeight(vertex, neighbor);
                        int costWeight = C.GetEdgeWeight(vertex, neighbor);
                        int newOdl = currOdl + edgeWeight + waitTime[neighbor];
                        int newCost = currCost + costWeight;
                        /*Console.WriteLine($"Vertex: {vertex}, Neighbor: {neighbor}, " +
                            $"CurrTime: {CurrTime}, EdgeWeight: {edgeWeight}, " +
                            $"WaitTime: {waitTime[neighbor]}," +
                            $" NewTime: {newTime}, " +
                            $"Current odp[neighbor]: {odp[neighbor]}");*/

                        if (newOdl < odl[neighbor]) // nowa sciezka jest lepsza
                        {
                            if (newCost <= maxCost) // nowa sciezka ma sens pod wzgledem ceny
                            {
                                odl[neighbor] = newOdl;
                                father[neighbor] = vertex;
                                pq.Insert(neighbor, newOdl);
                            }
                        }
                    }
                }
            }
            int[] GetPath(int t)
            {
                int endOfPath = t;
                List<int> path = new List<int> ();
                while(endOfPath != -1)
                {
                    path.Add(endOfPath);
                    endOfPath = father[endOfPath];
                }
                path.Reverse();
                return path.ToArray();
            }
            
            Dijkstra(G, C, s, t); // wyznacza nam ojcow
            int[] path = GetPath(t);
            if (path[0] == s && path[path.Length - 1] == t)
                return (odl[t], maxCost, path);
            else
                return null;
        }
        
        
    }
    
}