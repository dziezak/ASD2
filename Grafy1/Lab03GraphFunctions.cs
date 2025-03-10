using System;
using ASD.Graphs;
using ASD;
using System.Collections.Generic;
using System.Drawing;

namespace ASD
{

    public class Lab03GraphFunctions : System.MarshalByRefObject
    {

        // Część 1
        // Wyznaczanie odwrotności grafu
        //   0.5 pkt
        // Odwrotność grafu to graf skierowany o wszystkich krawędziach przeciwnie skierowanych niż w grafie pierwotnym
        // Parametry:
        //   g - graf wejściowy
        // Wynik:
        //   odwrotność grafu
        // Uwagi:
        //   1) Graf wejściowy pozostaje niezmieniony
        //   2) Graf wynikowy musi być w takiej samej reprezentacji jak wejściowy
        public DiGraph Lab03Reverse(DiGraph g)
        {
            DiGraph reversed = new DiGraph(g.VertexCount, g.Representation);
            for(int i=0; i< g.VertexCount; i++)
            {
                foreach (var e in g.OutNeighbors(i))
                {
                    //reversed.AddEdge(i, e);
                    reversed.AddEdge(e, i);
                }
            }
            return reversed;
        }

        // Część 2
        // Badanie czy graf jest dwudzielny
        //   0.5 pkt
        // Graf dwudzielny to graf nieskierowany, którego wierzchołki można podzielić na dwa rozłączne zbiory
        // takie, że dla każdej krawędzi jej końce należą do róźnych zbiorów
        // Parametry:
        //   g - badany graf
        //   vert - tablica opisująca podział zbioru wierzchołków na podzbiory w następujący sposób
        //          vert[i] == 1 oznacza, że wierzchołek i należy do pierwszego podzbioru
        //          vert[i] == 2 oznacza, że wierzchołek i należy do drugiego podzbioru
        // Wynik:
        //   true jeśli graf jest dwudzielny, false jeśli graf nie jest dwudzielny (w tym przypadku parametr vert ma mieć wartość null)
        // Uwagi:
        //   1) Graf wejściowy pozostaje niezmieniony
        //   2) Podział wierzchołków może nie być jednoznaczny - znaleźć dowolny
        //   3) Pamiętać, że każdy z wierzchołków musi być przyporządkowany do któregoś ze zbiorów
        //   4) Metoda ma mieć taki sam rząd złożoności jak zwykłe przeszukiwanie (za większą będą kary!)
        public bool Lab03IsBipartite(Graph g, out int[]? vert)
        {
            int n = g.VertexCount;
            vert = new int[n];

            for (int i = 0; i < n; i++)
            {
                vert[i] = -1;            }

            for (int start = 0; start < n; start++)            
            {
                if (vert[start] != -1) continue; 
                Queue<int> queue = new Queue<int>();
                queue.Enqueue(start);
                vert[start] = 1;

                while (queue.Count > 0)
                {
                    int u = queue.Dequeue();

                    foreach (int v in g.OutNeighbors(u))
                    {
                        if (vert[v] == -1)
                        {
                            vert[v] = 3 - vert[u]; 
                            queue.Enqueue(v);
                        }
                        else if (vert[v] == vert[u])
                        {
                            vert = null;
                            return false;                         }
                    }

                    foreach (int v in g.OutNeighbors(u))
                    {
                        if (vert[v] == -1)
                        {
                            vert[v] = 3 - vert[u];
                            queue.Enqueue(v);
                        }
                        else if (vert[v] == vert[u])
                        {
                            vert = null;
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        // Część 3
        // Wyznaczanie minimalnego drzewa rozpinającego algorytmem Kruskala
        //   1 pkt
        // Schemat algorytmu Kruskala
        //   1) wrzucić wszystkie krawędzie do "wspólnego worka"
        //   2) wyciągać z "worka" krawędzie w kolejności wzrastających wag
        //      - jeśli krawędź można dodać do drzewa to dodawać, jeśli nie można to ignorować
        //      - punkt 2 powtarzać aż do skonstruowania drzewa (lub wyczerpania krawędzi)
        // Parametry:
        //   g - graf wejściowy
        //   mstw - waga skonstruowanego drzewa (lasu)
        // Wynik:
        //   skonstruowane minimalne drzewo rozpinające (albo las)
        // Uwagi:
        //   1) Graf wejściowy pozostaje niezmieniony
        //   2) Wykorzystać klasę UnionFind z biblioteki Graph
        //   3) Jeśli graf g jest niespójny to metoda wyznacza las rozpinający
        //   4) Graf wynikowy (drzewo) musi być w takiej samej reprezentacji jak wejściowy
        public Graph<int> Lab03Kruskal(Graph<int> g, out int mstw)
        {
            mstw = 0;
            int n = g.VertexCount;

            Graph<int> mst = new Graph<int>(n, g.Representation);
            List<Edge<int>> edges = g.DFS().SearchAll().ToList();
            edges = edges.OrderBy(e => e.Weight).ToList();
            UnionFind unionFind = new UnionFind(n);
            foreach (var e in edges)
            {
                if (unionFind.Find(e.From) != unionFind.Find(e.To))
                {
                    mst.AddEdge(e.From, e.To, e.Weight);
                    unionFind.Union(e.From, e.To);
                    mstw += e.Weight;
                }
            }

            return mst;
        }

        // Część 4
        // Badanie czy graf nieskierowany jest acykliczny
        //   0.5 pkt
        // Parametry:
        //   g - badany graf
        // Wynik:
        //   true jeśli graf jest acykliczny, false jeśli graf nie jest acykliczny
        // Uwagi:
        //   1) Graf wejściowy pozostaje niezmieniony
        //   2) Najpierw pomysleć jaki, prosty do sprawdzenia, warunek spełnia acykliczny graf nieskierowany
        //      Zakodowanie tego sprawdzenia nie powinno zająć więcej niż kilka linii!
        //      Zadanie jest bardzo łatwe (jeśli wydaje się trudne - poszukać prostszego sposobu, a nie walczyć z trudnym!)
        public bool Lab03IsUndirectedAcyclic(Graph g)
        {
            Stack<int> stack = new Stack<int>();
            int[] visited = new int[g.VertexCount];
            visited[0] = 1;
            stack.Push(0);
            List<(int, int)> lista = new List<(int, int)>();
            while (stack.Count > 0)
            {
                var v = stack.Pop();
                foreach (var neighbor in g.OutNeighbors(v))
                {
                    if (lista.Contains((neighbor, v))) continue;
                    if (visited[neighbor] == 1)
                        return false;
                    else
                    {
                        lista.Add((v, neighbor));
                        visited[neighbor] = 1;
                        stack.Push(neighbor);
                    }
                }
            }

            return true;
        }
           
    }

}
