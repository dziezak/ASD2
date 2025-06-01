using System;
using ASD.Graphs;
using ASD;
using System.Collections.Generic;
using System.Linq;

namespace ASD
{

    public class Lab15 : System.MarshalByRefObject
    {
        /// <summary>
        /// Etap 1: Rozmiar najliczniejszej krainy w zadanym grafie (2.5p)
        /// 
        /// Przez krainę rozumiemy maksymalny zbiór wierzchołków, z których
        /// każde dwa należą do jakiegoś cyklu (równoważnie: najliczniejszy
        /// zbiór wierzchołków G indukujący podgraf 2-spójny wierzchołkowo).
        /// 
        /// Uwaga: Z powyższej definicji wynika, że zbiór pusty jest krainą, 
        /// a zbiór jednoelementowy nie.
        /// </summary>
        /// <param name="G">Graf prosty</param>
        /// <returns>Rozmiar największej bańki</returns>
        
        public int MaxProvinceSize(Graph G)
        {
            int n = G.VertexCount;
            int myTime = 0;
            int maxSize = 0;

            int[] dfs_num = new int[n];       // czas wejscia do wierzcholka
            int[] low = new int[n];           // najszybszy czas wejscia
            bool[] visited = new bool[n];
            Stack<(int, int)> edgeStack = new Stack<(int, int)>();

            void DFS(int u, int parent, ref int time)
            {
                visited[u] = true;
                dfs_num[u] = low[u] = ++time;
                int children = 0;

                foreach (int v in G.OutNeighbors(u))
                {
                    if (!visited[v])
                    {
                        edgeStack.Push((u, v));
                        DFS(v, u, ref time);
                        low[u] = Math.Min(low[u], low[v]);

                        if (low[v] >= dfs_num[u])
                        {
                            var land = new HashSet<int>();
                            (int a, int b) edge;
                            do
                            {
                                edge = edgeStack.Pop();
                                land.Add(edge.Item1);
                                land.Add(edge.Item2);
                            } while (!(edge.Item1 == u && edge.Item2 == v));

                            if (land.Count >= 3)  // musimy miec jaka droge mozliwa wiec przynajmniej 3 wiezcholki
                                maxSize = Math.Max(maxSize, land.Count);
                        }
                        children++;
                    }
                    else if (v != parent && dfs_num[v] < dfs_num[u])
                    {
                        edgeStack.Push((u, v));
                        low[u] = Math.Min(low[u], dfs_num[v]);
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                if (!visited[i])
                    DFS(i, -1, ref myTime);
            }

            return maxSize;
        }

        

        /// <summary>
        /// Etap 2: Wierzchołek znajdujący się w największej liczbie krain (2.5p)
        /// 
        /// Funcja zwraca wierzchołek znajdujący się w największej liczbie krain.
        /// 
        /// W przypadku remisu należy zwrócić wierzchołek o mniejszym numerze.
        /// </summary>
        /// <param name="G"></param>
        /// <returns></returns>
        public int VertexInMostProvinces(Graph G)
        {
            int n = G.VertexCount;
            int time = 0;
            int[] dfs_num = new int[n];
            int[] low = new int[n];
            bool[] visited = new bool[n];
            Stack<(int, int)> edgeStack = new Stack<(int, int)>();
            int[] count = new int[n];  // licznik krain dla każdego wierzchołka

            void DFS(int u, int parent, ref int t)
            {
                visited[u] = true;
                dfs_num[u] = low[u] = ++t;

                foreach (int v in G.OutNeighbors(u))
                {
                    if (!visited[v])
                    {
                        edgeStack.Push((u, v));
                        DFS(v, u, ref t);
                        low[u] = Math.Min(low[u], low[v]);

                        if (low[v] >= dfs_num[u])
                        {
                            var land = new HashSet<int>();
                            (int a, int b) edge;
                            do
                            {
                                edge = edgeStack.Pop();
                                land.Add(edge.a);
                                land.Add(edge.b);
                            } while (!(edge.a == u && edge.b == v));

                            if (land.Count >= 3)  // kraina to co najmniej 3 wierzchołki
                            {
                                foreach (var node in land)
                                    count[node]++;
                            }
                        }
                    }
                    else if (v != parent && dfs_num[v] < dfs_num[u])
                    {
                        edgeStack.Push((u, v));
                        low[u] = Math.Min(low[u], dfs_num[v]);
                    }
                }
            }

            for (int i = 0; i < n; i++)
                if (!visited[i])
                    DFS(i, -1, ref time);

            // Znajdź wierzchołek o największym count, w razie remisu najmniejszy indeks
            int bestVertex = -1;
            int maxCount = -1;
            for (int i = 0; i < n; i++)
            {
                if (count[i] > maxCount)
                {
                    maxCount = count[i];
                    bestVertex = i;
                }
            }

            return bestVertex;
        }
 
    }
}