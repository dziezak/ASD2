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
        ///  w kolejce trzymamy: ( numer_service, dzien_w_ktorym_jest_zarazony)
        public (int numberOfInfectedServices, int[] listOfInfectedServices) Stage2(Graph G, int K, int[] s, int[] serviceTurnoffDay)
        {
            bool print = false;
            int n = G.VertexCount;
            bool[] infected = new bool[n];
            int[] visited = new int[n];
            Queue<(int, int)> q = new Queue<(int, int)>();
            List<int> result = new List<int>();

            for (int k=0; k<n; k++)
            {
                visited[k] = -1;
                infected[k] = false;
            }

            if(print)
                Console.Write("Wrzucamy na poczatku:");
            foreach (var sn in s)
            {
                /*
                if (serviceTurnoffDay[sn] >= 1)
                {
                */
                    if(print)
                        Console.WriteLine($"({sn}, {1})");
                    q.Enqueue((sn, 1));
                    infected[sn] = true;
                //}
            }

            if(print)
                Console.WriteLine("Koniec wstawiania.");
               
            while (q.Count > 0)
            {
                var (A, dayAInfected) = q.Dequeue();
                if(print)
                    Console.Write($"Dequeue: ({A}, {dayAInfected})");
                if (visited[A] == -1 || dayAInfected < visited[A])
                {
                    visited[A] = dayAInfected;
                    if(print)
                        Console.WriteLine(" przetwarzane");
                }
                else
                {
                    if(print)
                        Console.WriteLine(" NIE przetwarzane.");
                    continue;
                }
                //visited[A] = dayAInfected;
                foreach (var B in G.OutNeighbors(A))
                {
                    int dayInfectedNeighbor = dayAInfected + 1;
                    /*
                    if (visited[B] != -1 && dayInfectedNeighbor >= visited[B] )
                        continue;
                        */
                    if (serviceTurnoffDay[B] > dayInfectedNeighbor && serviceTurnoffDay[A] > dayInfectedNeighbor)
                        ;
                    else
                        continue;
                    
                    if(dayInfectedNeighbor > K)
                        continue;
                    
                    if (visited[B] == -1 || dayInfectedNeighbor < visited[B] )
                    {
                        infected[B] = true;
                        if(print)
                            Console.WriteLine($"Enqueu: ({B}, {dayInfectedNeighbor})");
                        q.Enqueue((B, dayInfectedNeighbor));
                    }
                }
            }

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
        ///  w kolejce trzymamy: ( numer_service, dzien_w_ktorym_jest_zarazony)
        ///  
        public (int numberOfInfectedServices, int[] listOfInfectedServices) Stage3(Graph G, int K, int[] s, int[] serviceTurnoffDay, int[] serviceTurnonDay)
        {
            bool print = false;
            int n = G.VertexCount;
            bool[] infected = new bool[n];
            int[] visited = new int[n];
            List<int> result = new List<int>();
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
                if(print)
                    Console.WriteLine($"Enqueu: ({sn}, {1})");
                q.Enqueue((sn, 1));
               // visited[sn] = 1;
                infected[sn] = true;
                //result.Add(sn);
            }

            if(print)
                Console.WriteLine("Koniec wstawiania.");
               
            while (q.Count > 0)
            {
                var (A, dayAInfected) = q.Dequeue();
                if(print)
                    Console.WriteLine($"Dequeue: ({A}, {dayAInfected})");
                if (visited[A] == -1 || dayAInfected < visited[A])
                {
                    visited[A] = dayAInfected;
                }
                else continue;
                visited[A] = dayAInfected;
                foreach (var B in G.OutNeighbors(A))
                {
                    if(visited[B] > 0) continue;
                    int dayInfectedNeighbor = dayAInfected + 1;
                    if ((serviceTurnonDay[A]+1<serviceTurnoffDay[B]) 
                        || (serviceTurnonDay[B]+1<serviceTurnoffDay[A]))
                    {
                        if (serviceTurnonDay[A]+1<serviceTurnoffDay[B])
                        {
                            if (dayInfectedNeighbor < serviceTurnoffDay[A]) ;
                            else if (dayInfectedNeighbor < serviceTurnonDay[B])
                                dayInfectedNeighbor = Math.Max(dayInfectedNeighbor , serviceTurnonDay[A] + 1);
                            else dayInfectedNeighbor = Math.Max(dayInfectedNeighbor, serviceTurnonDay[B] + 1);
                        }

                        if (serviceTurnonDay[B] + 1 < serviceTurnoffDay[A])
                        {
                            if (dayInfectedNeighbor < serviceTurnoffDay[B]) ;
                            else if (dayInfectedNeighbor < serviceTurnonDay[A]) 
                                dayInfectedNeighbor = Math.Max(dayInfectedNeighbor,serviceTurnonDay[B] + 1);
                            else dayInfectedNeighbor= Math.Max(dayInfectedNeighbor, serviceTurnonDay[A] + 1);
                        }
                    }
                    else
                    {
                       int DiMin=Math.Min(serviceTurnoffDay[A],serviceTurnoffDay[B]);
                       int DiMax = Math.Max(serviceTurnonDay[A], serviceTurnonDay[B]);
                       if (dayInfectedNeighbor < DiMin) ;
                       else dayInfectedNeighbor = Math.Max(dayInfectedNeighbor, DiMax + 1);
                    }
                    /*
                    if (dayInfectedNeighbor < serviceTurnoffDay[A])
                    {
                        ;
                    }
                    else
                    {
                        dayInfectedNeighbor =Math.Max(serviceTurnonDay[A] + 1, dayInfectedNeighbor);
                    }

                    if (dayInfectedNeighbor < serviceTurnoffDay[B])
                    {
                        ;
                    }
                    else if (dayInfectedNeighbor <= serviceTurnonDay[B])
                    {
                        dayInfectedNeighbor = Math.Max(serviceTurnonDay[B] + 1, dayInfectedNeighbor);
                    }
                    */

                    if (dayInfectedNeighbor > K)
                        continue;

                    if (visited[B] == -1 || dayInfectedNeighbor < visited[B])
                    {
                        infected[B] = true;
                        if(print)
                            Console.WriteLine($"Enqueu: ({B}, {dayInfectedNeighbor})");
                        q.Enqueue((B, dayInfectedNeighbor));
                    }
                }
            }

            for (int j = 0; j < n; j++)
            {
                if (infected[j])
                    result.Add(j);
            }

            if (print)
            {
                for (int i = 0; i < result.Count; i++)
                    Console.Write($"{result[i]}, ");
                Console.WriteLine();
            }

            return (result.Count, result.ToArray());
        }
        
    }
}
