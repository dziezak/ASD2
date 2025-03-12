// See https://aka.ms/new-console-template for more information


using System.Net.WebSockets;
using ASD.Graphs;
using ASD.Graphs;

internal class Program
{
    static DiGraph<int> GraphTest1(int n)
    {
        DiGraph<int> g = new DiGraph<int>(n);
        for (int i = 0; i < n; i++)
        {
            g.AddEdge(i, (i + 1) % n);
        }
        return g;
    }

    static DiGraph<int> GraphTest2(int n)
    {
        DiGraph<int> g = new DiGraph<int>(n);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                g.AddEdge(i, j);
            }
        }
        return g;
    }

    static void PrintfGraf(DiGraph<int> g)
    {
        List<Edge<int>> list = g.DFS().SearchAll().ToList();
        foreach (var item in list)
        {
            Console.WriteLine(item.To + " " + item.From);
        }
    }

    static void PrintfGraf2(DiGraph<int> g)
    {
        for (int i = 0; i < g.VertexCount; i++)
        {
            Console.Write(i + ":");
            foreach (var e in g.OutNeighbors(i))
            {
                Console.Write(e + ",");
            }

            Console.WriteLine();
        }
    }

    static int Main()
    {
        int n = 5;
        DiGraph<int> g = GraphTest1(n);
        PrintfGraf2(g);
       return 0;
    }
    
}