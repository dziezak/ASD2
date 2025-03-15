using System;
using System.Linq;
using System.Text;
//using ASD;
using System.Collections.Generic;
using ASD.Graphs;



namespace ASD
{
    public class Program2
    {
        static void main(string[] args)
        {
            int n = 25;
            int K = 4;
            Graph g = new Graph(n);

            g.AddEdge(0, 1);
            g.AddEdge(1, 6);
            g.AddEdge(6, 14);
            g.AddEdge(14, 15);
            g.AddEdge(1, 5);
            g.AddEdge(5, 13);
            g.AddEdge(13, 15);
            g.AddEdge(0, 2);
            g.AddEdge(2, 7);
            g.AddEdge(7, 16);
            g.AddEdge(16, 18);
            g.AddEdge(2, 8);
            g.AddEdge(8, 17);
            g.AddEdge(17, 18);
            g.AddEdge(0, 4);
            g.AddEdge(4, 9);
            g.AddEdge(9, 19);
            g.AddEdge(19, 21);
            g.AddEdge(4, 10);
            g.AddEdge(10, 20);
            g.AddEdge(20, 21);
            g.AddEdge(0, 3);
            g.AddEdge(3, 11);
            g.AddEdge(11, 22);
            g.AddEdge(22, 24);
            g.AddEdge(3, 12);
            g.AddEdge(12, 23);
            g.AddEdge(23, 24);
            int[] s = new int[] { 0 };
            
            int[] serviceTurnoffDay = Enumerable.Range(0, n).Select(x => x == 3 || x == 4 || x == 1 ? 2 : K + 1).ToArray();


            for (int i = 0; i < n; i++)
            {
                Console.Write($"{i}, ");
            }
            Console.WriteLine();
            for(int i = 0; i < n; i++)
            {
                if (i >= 10)
                {
                    Console.Write($" {serviceTurnoffDay[i]}, ");
                }
                else
                {
                    Console.Write($"{serviceTurnoffDay[i]}, ");
                }
            }

            Console.WriteLine();
            var test = new Lab04();
            
            
            (int numberOfInfectedServices, int[] listOfInfectedServices) = test.Stage2(g, K ,  s, serviceTurnoffDay);
            Console.WriteLine(numberOfInfectedServices);

            
        }

        static void main2(string[] args)
        { 
            int K = 2;
            int n = 3;
            Graph g = new Graph(n);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2); 
            int[] s = new int[] { 0 };
            int[] serviceTurnoffDay = new int[] { 2, K + 1, K + 1 };
            var test = new Lab04();
            
            for (int i = 0; i < n; i++)
            {
                Console.Write($"{i}, ");
            }
            Console.WriteLine();
            for(int i = 0; i < n; i++)
            {
                if (i >= 10)
                {
                    Console.Write($" {serviceTurnoffDay[i]}, ");
                }
                else
                {
                    Console.Write($"{serviceTurnoffDay[i]}, ");
                }
            }

            Console.WriteLine();
                
            (int numberOfInfectedServices, int[] listOfInfectedServices) = test.Stage2(g, K ,  s, serviceTurnoffDay);
            Console.WriteLine($"wynik: {numberOfInfectedServices}");
        }

    }
}
