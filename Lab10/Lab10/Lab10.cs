using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ASD
{
    class PathRecord
    {
        public List<int> Nodes;     // wierzchołki ścieżki
        public HashSet<int> Used;   // zbiór użytych wierzchołków
        public int Start => Nodes[0];
        public int End => Nodes[^1];
        public List<int> Colors;
    }
    
    

    public class Lab10 : MarshalByRefObject
    {
        /// <summary>
        /// Szukanie najdłuższego powtórzenia w zadanym kolorowaniu grafu.
        /// </summary>
        /// <param name="G">Graf prosty</param>
        /// <param name="color">Kolorowanie wierzchołków G (color[v] to kolor wierzchołka v)</param>
        /// <returns>Ścieżka, na której występuje powtórzenie (numery kolejnych wierzchołków)</returns>
        /// <remarks>W przypadku braku powtórzeń należy zwrócić null lub tablicę o długości 0</remarks>
        public int[] FindLongestRepetitionETAP1(Graph G, int[] color)
        {
            int n = color.Length;
            List<int> longestPath = null;

            void Backtrack(List<int> path, HashSet<int> visited)
            {
                int len = path.Count;
                if (len % 2 == 0 && len > 0)
                {
                    int k = len / 2;
                    bool match = true;
                    for (int i = 0; i < k; i++)
                    {
                        if (color[path[i]] != color[path[k + i]]) // pominmy pozniej kolor ktory jest tylko raz w calym grafie
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                    {
                        if (longestPath == null || len > longestPath.Count)
                        {
                            longestPath = new List<int>(path);
                        }
                    }
                }
                //tutaj moze jakies ograniczenie sie przyda np. if(len bardzo duzze to koniec?) return;

                int u = path[len - 1];
                foreach (int v in G.OutNeighbors(u))
                {
                    if (!visited.Contains(v))
                    {
                        visited.Add(v);
                        path.Add(v);
                        Backtrack(path, visited);
                        path.RemoveAt(path.Count - 1);
                        visited.Remove(v);
                    }
                }
            }

            for(int start = 0; start < n; start++)
            {
                var path = new List<int> { start};
                var visited = new HashSet<int> { start};
                Backtrack(path, visited);
            }

            return longestPath?.ToArray() ?? new int[0];
        }

        public int[] FindLongestRepetitionEtap2(Graph G, int[] color)
        {
            int n = color.Length;
            List<int> longestPath = null;
            int maxRepetitionLength = 0;

            var colorFreq = new Dictionary<int, int>();
            foreach(int c in color){
                if (!colorFreq.ContainsKey(c)) colorFreq[c] = 0;
                colorFreq[c]++;
            }


            void Backtrack(List<int> path, HashSet<int> visited)
            {
                int len = path.Count;
                if (len % 2 == 0 && len > 0)
                {
                    int k = len / 2;
                    bool match = true;
                    for (int i = 0; i < k; i++)
                    {
                        if (color[path[i]] != color[path[k + i]]) 
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                    {
                        if (longestPath == null || len > longestPath.Count)
                        {
                            longestPath = new List<int>(path);
                        }
                    }
                }
                //tutaj moze jakies ograniczenie sie przyda np. if(len >= bardzo duzze to koniec?) return;
                

                int u = path[len - 1];
                foreach (int v in G.OutNeighbors(u))
                {
                    if (!visited.Contains(v))
                    {
                        if (colorFreq[color[v]] == 1) continue; // mega fajne?
                        visited.Add(v);
                        path.Add(v);
                        Backtrack(path, visited);
                        path.RemoveAt(path.Count - 1);
                        visited.Remove(v);
                    }
                }
            }

            var startVertices = Enumerable.Range(0, n)
                .Where(i => colorFreq[color[i]] > 1)
                .OrderByDescending(i => colorFreq[color[i]])
                .ToList();
            //for (int start = 0; start < n; start++)
            foreach (int start in startVertices)
            {
                if (colorFreq[color[start]] == 1) continue;
                var path = new List<int> { start };
                var visited = new HashSet<int> { start };
                Backtrack(path, visited);
            }

            return longestPath?.ToArray() ?? new int[0];
        }
        
        //zbyt mocne rozwiazanie:
        public int[] FindLongestRepetitionTooPowerful(Graph G, int[] color)
        {
            int n = color.Length;
            var colorFreq = new Dictionary<int, int>();
            foreach (int c in color)
            {
                if (!colorFreq.ContainsKey(c)) colorFreq[c] = 0;
                colorFreq[c]++;
            }

            var allColors = new HashSet<int>(color);
            int singleColors = colorFreq.Count(kv => kv.Value == 1);
            int maxLen = (n - singleColors) / 2;
            int maxSafeLen = Math.Min(5, maxLen); // można ustawić nawet na 6-7

            List<int[]> allColorSequences = new();
            for (int len = 1; len <= maxSafeLen; len++)
            {
                var seqs = GenerateColorSequences(len, allColors, colorFreq);
                allColorSequences.AddRange(seqs.Select(s => s.ToArray()));
            }

            List<PathRecord> candidatePaths = new();
            foreach (var seq in allColorSequences)
            {
                var paths = FindAllMatchingPaths(G, color, seq);
                candidatePaths.AddRange(paths);
            }

            List<int> bestDoublePath = new();

            bool AreNeighbors(int u, int v) => G.OutNeighbors(u).Contains(v);

            foreach (var group in candidatePaths.GroupBy(p => string.Join("-", p.Colors)))
            {
                var paths = group.ToList();
                int m = paths.Count;
                for (int i = 0; i < m; i++)
                {
                    for (int j = i + 1; j < m; j++)
                    {
                        if (!paths[i].Used.Overlaps(paths[j].Used))
                        {
                            if (AreNeighbors(paths[i].End, paths[j].Start) || AreNeighbors(paths[j].End, paths[i].Start))
                            {
                                var combined = paths[i].Nodes.Concat(paths[j].Nodes).ToList();
                                if (combined.Count > bestDoublePath.Count)
                                    bestDoublePath = combined;
                            }
                        }
                    }
                }
            }

            return bestDoublePath.ToArray();
        }
        List<PathRecord> FindAllMatchingPaths(Graph G, int[] color, int[] sequence)
        {
            List<PathRecord> result = new();
            int n = color.Length;

            for (int start = 0; start < n; start++)
            {
                if (color[start] != sequence[0]) continue;
                DFS(start, 0, new List<int> { start }, new HashSet<int> { start });
            }

            void DFS(int current, int index, List<int> path, HashSet<int> used)
            {
                if (index == sequence.Length - 1)
                {
                    result.Add(new PathRecord
                    {
                        Nodes = new List<int>(path),
                        Used = new HashSet<int>(used),
                        Colors = path.Select(v => color[v]).ToList()
                    });
                    return;
                }

                foreach (var neighbor in G.OutNeighbors(current))
                {
                    if (used.Contains(neighbor)) continue;
                    if (color[neighbor] != sequence[index + 1]) continue;
                    path.Add(neighbor);
                    used.Add(neighbor);
                    DFS(neighbor, index + 1, path, used);
                    path.RemoveAt(path.Count - 1);
                    used.Remove(neighbor);
                }
            }

            return result;
        }
        public static List<List<int>> GenerateColorSequences(int length, HashSet<int> allColors, Dictionary<int, int> colorFreq)
        {
            var result = new List<List<int>>();
            var current = new int[length];

            void Backtrack(int pos)
            {
                if (pos == length)
                {
                    var count = new Dictionary<int, int>();
                    for (int i = 0; i < length; i++)
                    {
                        int c = current[i];
                        if (!count.ContainsKey(c)) count[c] = 0;
                        count[c]++;
                    }

                    foreach (var kv in count)
                    {
                        if (!colorFreq.TryGetValue(kv.Key, out int available) || available < kv.Value * 2)
                            return; // za mało kolorów do powtórzenia
                    }

                    result.Add(current.ToList());
                    return;
                }

                foreach (int color in allColors)
                {
                    current[pos] = color;
                    Backtrack(pos + 1);
                }
            }

            Backtrack(0);
            return result;
        }


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

            /*
            if (longestPath.Length > 0)
            {
                foreach(var v in longestPath)
                {
                    Console.Write(color[v]+","); 
                }
                Console.WriteLine();
            }
            */
            return longestPath?.ToArray() ?? Array.Empty<int>();
        }
    }
}




