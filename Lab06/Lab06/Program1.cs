using System;
using System.Diagnostics;
using ASD.Graphs;

namespace ASD;

public class Program1
{
    static void AddDirectedEdge(DiGraph<int> G, Graph<int> C, int from, int to, int weight, int cost)
    {
        G.AddEdge(from, to, weight);
        C.AddEdge(from, to, cost);
    }

    static (DiGraph<int> G, Graph<int> C, int[] waitTime) GenerateRandomExample(int n, int m, int minTime, int maxTime, int minCost, int maxCost, int seed, bool directed = false)
    {
        DiGraph<int> G = new DiGraph<int>(n);
        Graph<int> C = new Graph<int>(n);
        int[] waitTime = new int[n];

        Random random = new Random(seed);

        for (int i = 0; i < n; ++i)
            waitTime[i] = random.Next(minTime, maxTime);

        for (int i = 0; i < m; ++i)
        {
            int from = random.Next(n), to = random.Next(n);
            while (from == to)
                to = random.Next(n);
            if (directed)
                AddDirectedEdge(G, C, from, to, random.Next(minTime, maxTime), random.Next(minCost, maxCost));
            else
                AddDirectedEdge(G, C, from, to, random.Next(minTime, maxTime), random.Next(minCost, maxCost));
        }

        return (G, C, waitTime);
    }

    static void PrintGraph(DiGraph<int> G)
    {
        for (int i = 0; i < G.VertexCount; i++)
        {
            Console.Write($"{i}: ");
            foreach( var vertex in G.OutNeighbors(i))
               Console.Write($"{vertex}, ");
            Console.WriteLine();
        }
    }

    static void Main0()
    {
        
        //int n = 10000, m = 4 * n;
        int n = 5, m = 4 * n;
        (var G, var C, var waitTime) = GenerateRandomExample(n, m, 1, 200, 0, 50, 2137);
        int s = 0, t = 4;
        Console.WriteLine("Wygenerowany graf:");
        PrintGraph(G);
        //Stage2.TestCases.Add(new Lab06Stage2TestCase(G, C, waitTime, s, t, 2739, 115, 10, $"Graf losowy n={n}"));
        //int expectedLength = 2739;
        //int expectedCost = 115;
        
        
        Lab06 test = new Lab06();
        //(int l, int c, int[] path) 
        (int l, int c, int[] path)? odp = test.Stage2(G, C, waitTime, s, t)!;
        if(odp == null)
            Console.WriteLine("No problem");
        Console.WriteLine("UDALO SIE ZROBIC PRZYKLAD");
        //Debug.Assert(odp.Value.l == expectedLength && odp.Value.c == expectedCost);
    }
}