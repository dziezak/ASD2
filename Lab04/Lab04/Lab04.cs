using ASD.Graphs;
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
            bool print = false;
            int n = G.VertexCount;
            //int m = G.EdgeCount;
            //int p = s.Length;
            bool[] infected = new bool[n];
            int[] visited = new int[n];
            Queue<(int, int)> q = new Queue<(int, int)>();

            for (int k=0; k<n; k++)
            {
                visited[k] = -1;
                infected[k] = false;
            }

            if(print)
                Console.Write("Wrzucamy na poczatku:");
            foreach (var sn in s)
            {
                if (serviceTurnoffDay[sn] >= 1)
                {
                    if(print)
                        Console.WriteLine($"({sn}, {1})");
                    q.Enqueue((sn, 1));
                    visited[sn] = 1;
                    infected[sn] = true;
                }
            }

            if(print)
                Console.WriteLine("Koniec wstawiania.");
               
            while (q.Count > 0)
            {
                var (i, daysPassed) = q.Dequeue();
                if(print)
                    Console.WriteLine($"Dequeue: ({i}, {daysPassed})");
                if (daysPassed >= K)
                    continue;
                if(serviceTurnoffDay[i] <= daysPassed+1)
                    continue;
                //Console.WriteLine("Koniec wstawiania.");
                foreach (var v in G.OutNeighbors(i))
                {
                    if (serviceTurnoffDay[v] > 1 + daysPassed)
                    {
                        if (visited[v] == -1 || daysPassed + 1 < visited[v])
                        {
                            if(print)
                                Console.WriteLine($"({v}, {daysPassed + 1})");
                            visited[v] = daysPassed + 1;
                            infected[v] = true;
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
            
            if(print)
                foreach (var i in result)
                    Console.Write($"{i}, ");

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
        /// czyli serwer v jest bezpieczny w trakcie czasu [serviceTurnoffDay[v], serviceTurnOnDay[v]];
        /// dla kazdego v: 1<=serviceTurnoffday[v]<=serviceTurnonDay[v]<=K+1
        /// czyli jesli mialby byc zarazony w ten czas to nie zaraza
        /// jesli byl zarazony i jest zatrzymany to przestaje zarazac teraz i bedzie zarazac dopiero w serviceTurnonday[v]+1 ( bo wlaczenie jest na koniec)
        ///  w kolejce trzymamy: ( numer_service, dzien_w_ktory_zaraza)
        ///  
        public (int numberOfInfectedServices, int[] listOfInfectedServices) Stage3(Graph G, int K, int[] s, int[] serviceTurnoffDay, int[] serviceTurnonDay)
        {
            bool print = false;
            int n = G.VertexCount;
            bool[] infected = new bool[n];
            int[] visited = new int[n];
            Queue<(int, int)> q = new Queue<(int, int)>();

            for (int k=0; k<n; k++)
            {
                visited[k] = -1;
                infected[k] = false;
            }

            if(print)
                Console.Write("Wrzucamy na poczatku:");
            
            foreach (var sn in s)
            {
                if (serviceTurnoffDay[sn] > 1 ) // serwer nie jest wylaczony dnia pierwszego
                {
                    if(print)
                        Console.WriteLine($"({sn}, {1})");
                    q.Enqueue((sn, 1));
                    visited[sn] = 1;
                    infected[sn] = true;
                } 
                else if(serviceTurnonDay[sn]< K+1)// serwer ponownie wylaczony poprawka z testu 2
                {
                    q.Enqueue((sn, serviceTurnonDay[sn]+1));
                    visited[sn] = serviceTurnonDay[sn]+1;
                    infected[sn] = true;
                } 
            }

            if(print)
                Console.WriteLine("Koniec wstawiania.");
               
            while (q.Count > 0)
            {
                var (i, daysPassed) = q.Dequeue();
                if(print)
                    Console.WriteLine($"Dequeue: ({i}, {daysPassed})");
                if (daysPassed > K)
                    continue;
                if(serviceTurnoffDay[i] <= daysPassed+1 && daysPassed + 1 <= serviceTurnonDay[i]) // pomyslec czy drugi warunek ok poprawka z testu 3
                    continue;
                foreach (var v in G.OutNeighbors(i))
                {
                    if (serviceTurnoffDay[v] > daysPassed) 
                    {
                        if (visited[v] == -1 || daysPassed + 1 < visited[v])
                        {
                            if(print)
                                Console.WriteLine($"({v}, {daysPassed + 1})");
                            visited[v] = daysPassed + 1;
                            infected[v] = true;
                            q.Enqueue((v, daysPassed + 1));
                        }
                    }else if (daysPassed + 1 > serviceTurnonDay[v] || serviceTurnonDay[v] <= K ) // drugi warunek poprawka z testu 4
                    {
                        if (visited[v] == -1 || serviceTurnonDay[v]+1 < visited[v])
                        {
                            if (print)
                                Console.WriteLine($"({v}, {daysPassed + 1})");
                            visited[v] = serviceTurnonDay[v]+1;
                            infected[v] = true;
                            q.Enqueue((v, serviceTurnonDay[v]+2));// tutaj +2 bo +1 jest dnia jego zarazenia więc +2 to dzien w ktory zaraza
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
            
            if(print)
                foreach (var i in result)
                    Console.Write($"{i}, ");

            return (result.Count, result.ToArray());
        }
        
    }
}
