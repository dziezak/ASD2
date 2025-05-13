using ASD.Graphs;
using System;
using System.Collections.Generic;

namespace Lab10
{
    public class Lab10Solution : MarshalByRefObject
    {
        /// <summary>
        /// Wariant 1: Znajdź najtańszy zbiór wierzchołków grafu G 
        /// rozdzielający wszystkie pary wierzchołków z listy fanclubs 
        /// </summary>
        /// <param name="G">Graf prosty</param>
        /// <param name="fanclubs">Lista wierzchołków, które należy rozdzielić</param>
        /// <param name="cost">cost[v] to koszt użycia wierzchołka v; koszty są nieujemne</param>
        /// <param name="maxBudget">Górne ograniczenie na koszt rozwiązania</param>
        /// <returns></returns>
        public List<int> FindSeparatingSet(Graph G, List<int> fanclubs, int[] cost, int maxBudget)
        {
            Graph RemoveVertices(HashSet<int> removed)
            {
                var newG = new Graph(G);
                for (int u = 0; u < G.VertexCount; u++)
                {
                    if(removed.Contains(u)) continue;
                    foreach (var v in G.OutNeighbors(u))
                    {
                        if (!removed.Contains(v))
                            newG.AddEdge(u, v);
                    }
                }
                return newG;
            }
            Graph G2 = new Graph(G);

            Dictionary<int, int> GetComponentIds()
            {
                var visited = new bool[G.VertexCount];
                var comp = new Dictionary<int, int>();
                int id = 0;
                for (int i = 0; i < G.VertexCount; i++)
                {
                    if (!visited[i])
                    {
                        DFS(i, id++, visited, comp);
                    }
                }
                return comp;
            }

            void DFS(int v, int id, bool[] visited, Dictionary<int, int> comp)
            {
                visited[id] = true;
                comp[v] = id;
                foreach (var nei in G.OutNeighbors(v))
                {
                    if (!visited[nei])
                    {
                        DFS(nei, id, visited, comp);
                    }
                }
            }
            
            
            return null;
        }



        /// <summary>
        /// Wariant 2: Znajdź najtańszy spójny zbiór wierzchołków grafu G 
        /// rozdzielający wszystkie pary wierzchołków z listy fanclubs 
        /// </summary>
        /// <param name="G">Graf prosty</param>
        /// <param name="fanclubs">Lista wierzchołków, które należy rozdzielić</param>
        /// <param name="cost">cost[v] to koszt użycia wierzchołka v; koszty są nieujemne</param>
        /// <param name="maxBudget">Górne ograniczenie na koszt rozwiązania</param>
        /// <returns></returns>
        public List<int> FindConnectedSeparatingSet(Graph G, List<int> fanclubs, int[] cost, int maxBudget)
        {
            return null;
        }
    }
}
