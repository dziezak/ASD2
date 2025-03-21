﻿using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASD
{
    public class Lab04 : MarshalByRefObject
    {
        /// <summary>
        /// Etap 1 - Wyznaczenie liczby oraz listy zainfekowanych serwisów po upływie K dni.
        /// Algorytm analizuje propagację infekcji w grafie i zwraca wszystkie dotknięte nią serwisy.
        /// </summary>
        /// <param name="G">Graf reprezentujący infrastrukturę serwisów.</param>
        /// <param name="K">Liczba dni propagacji infekcji.</param>
        /// <param name="s">Indeks początkowo zainfekowanego serwisu.</param>
        /// <returns>
        /// (int numberOfInfectedServices, int[] listOfInfectedServices) - 
        /// numberOfInfectedServices: liczba zainfekowanych serwisów,
        /// listOfInfectedServices: tablica zawierająca numery zainfekowanych serwisów w kolejności rosnącej.
        /// </returns>
        public (int numberOfInfectedServices, int[] listOfInfectedServices) Stage1(Graph G, int K, int s)
        {
            int n = G.VertexCount;
            int m = G.EdgeCount;
            Queue<(int, int)> q = new Queue<(int, int)>();
            bool[] visited = new bool[n];

            q.Enqueue((s, 1));

            while(q.Count > 0)
            {
                var (i, daysPassed) = q.Dequeue();
                visited[i] = true;
                if(daysPassed < K)
                {
                    foreach (int v in G.OutNeighbors(i))
                    {
                        if (!visited[v])
                        {
                            int newDaysPassed = daysPassed+1;
                            q.Enqueue((v, newDaysPassed));
                        }
                    }
                }
            }

            List<int> result = new List<int>();
            for(int j=0; j<n; j++)
            {
                if (visited[j])
                    result.Add(j);
            }
            return (result.Count, result.ToArray());
        }

        /// <summary>
        /// Etap 2 - Wyznaczenie liczby oraz listy zainfekowanych serwisów przy uwzględnieniu wyłączeń.
        /// Algorytm analizuje propagację infekcji z możliwością wcześniejszego wyłączania serwisów.
        /// </summary>
        /// <param name="G">Graf reprezentujący infrastrukturę serwisów.</param>
        /// <param name="K">Liczba dni propagacji infekcji.</param>
        /// <param name="s">Tablica początkowo zainfekowanych serwisów.</param>
        /// <param name="serviceTurnoffDay">Tablica zawierająca dzień, w którym dany serwis został wyłączony (K + 1 oznacza brak wyłączenia).</param>
        /// <returns>
        /// (int numberOfInfectedServices, int[] listOfInfectedServices) - 
        /// numberOfInfectedServices: liczba zainfekowanych serwisów,
        /// listOfInfectedServices: tablica zawierająca numery zainfekowanych serwisów w kolejności rosnącej.
        /// </returns>
        public (int numberOfInfectedServices, int[] listOfInfectedServices) Stage2(Graph G, int K, int[] s, int[] serviceTurnoffDay)
        {
            int n = G.VertexCount;
            int m = G.EdgeCount;
            int p = s.Length;
            bool[] infected = new bool[n];
            int[] visited = new int[n];
            Queue<(int, int)> q = new Queue<(int, int)>();

            for (int k=0; k<n; k++)
            {
                visited[k] = -1;
            }

            foreach (int sn in s)
            {
                q.Enqueue((sn, 1));
            }
               
            while (q.Count > 0)
            {
                
                var (i, daysPassed) = q.Dequeue();
               
                visited[i] = daysPassed;
                infected[i] = true;
                if (daysPassed < K)
                {
                    foreach (int v in G.OutNeighbors(i))
                    {
                        if ((visited[v] < 0 || visited[v] > daysPassed + 1) && serviceTurnoffDay[v] < daysPassed + 1)
                        {
                            q.Enqueue((v, daysPassed + 1));
                        }
                    }
                }
            }

            List<int> result = new List<int>();
            for (int j = 0; j < n; j++)
            {
                if (infected[j])
                    result.Add(j);
            }

            return (result.Count, result.ToArray());
        }

        /// <summary>
        /// Etap 3 - Wyznaczenie liczby oraz listy zainfekowanych serwisów z możliwością ponownego włączenia wyłączonych serwisów.
        /// Algorytm analizuje propagację infekcji uwzględniając serwisy, które mogą być ponownie uruchamiane po określonym czasie.
        /// </summary>
        /// <param name="G">Graf reprezentujący infrastrukturę serwisów.</param>
        /// <param name="K">Liczba dni propagacji infekcji.</param>
        /// <param name="s">Tablica początkowo zainfekowanych serwisów.</param>
        /// <param name="serviceTurnoffDay">Tablica zawierająca dzień, w którym dany serwis został wyłączony (K + 1 oznacza brak wyłączenia).</param>
        /// <param name="serviceTurnonDay">Tablica zawierająca dzień, w którym dany serwis został ponownie włączony.</param>
        /// <returns>
        /// (int numberOfInfectedServices, int[] listOfInfectedServices) - 
        /// numberOfInfectedServices: liczba zainfekowanych serwisów,
        /// listOfInfectedServices: tablica zawierająca numery zainfekowanych serwisów w kolejności rosnącej.
        /// </returns>
        public (int numberOfInfectedServices, int[] listOfInfectedServices) Stage3(Graph G, int K, int[] s, int[] serviceTurnoffDay, int[] serviceTurnonDay)
        {
            int n = G.VertexCount;
            int m = G.EdgeCount;
            int p = s.Length;

            return (int.MaxValue, null);
        }
    }
}
