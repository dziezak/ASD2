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

            int[] serviceTurnoffDay =
                Enumerable.Range(0, n).Select(x => x == 3 || x == 4 || x == 1 ? 2 : K + 1).ToArray();


            for (int i = 0; i < n; i++)
            {
                Console.Write($"{i}, ");
            }

            Console.WriteLine();
            for (int i = 0; i < n; i++)
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


            (int numberOfInfectedServices, int[] listOfInfectedServices) = test.Stage2(g, K, s, serviceTurnoffDay);
            Console.WriteLine(numberOfInfectedServices);


        }

        static void niemain2(string[] args)
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
            for (int i = 0; i < n; i++)
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

            (int numberOfInfectedServices, int[] listOfInfectedServices) = test.Stage2(g, K, s, serviceTurnoffDay);
            Console.WriteLine($"wynik: {numberOfInfectedServices}");
        }

        //Stage 3:
        static void Main1(string[] args)
        {
            int K = 2;
            int n = 3;
            Graph g = new Graph(n);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            var test = new Lab04();
            int[] s = new int[] { 0 };
            int[] serviceTurnoffDay = new int[] { 2, K + 1, K + 1 };
            int[] serviceTurnonDay = new int[] { 2, K + 1, K + 1 };

            for (int i = 0; i < n; i++)
            {
                Console.Write($"{i}, ");
            }

            Console.WriteLine();
            for (int i = 0; i < n; i++)
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
            for (int i = 0; i < n; i++)
            {
                if (i >= 10)
                {
                    Console.Write($" {serviceTurnonDay[i]}, ");

                }
                else
                {
                    Console.Write($"{serviceTurnonDay[i]}, ");
                }
            }

            Console.WriteLine();
            (int numberOfInfectedServices, int[] listOfInfectedServices) =
                test.Stage3(g, K, s, serviceTurnoffDay, serviceTurnonDay);
            Console.WriteLine($"wynik: {numberOfInfectedServices}");
            foreach (var service in listOfInfectedServices)
            {
                Console.Write($"{service}, ");
            }
            // odp 1 : services{0};
        }

        static void Main2(string[] args) //2
        {
            int K = 2;
            int n = 3;
            Graph g = new Graph(n);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            int[] s = new int[] { 0 };
            int[] serviceTurnoffDay = new int[] { 1, K + 1, K + 1 };
            int[] serviceTurnonDay = new int[] { 1, K + 1, K + 1 };
            var test = new Lab04();
            for (int i = 0; i < n; i++)
            {
                Console.Write($"{i}, ");
            }

            Console.WriteLine();
            for (int i = 0; i < n; i++)
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
            for (int i = 0; i < n; i++)
            {
                if (i >= 10)
                {
                    Console.Write($" {serviceTurnonDay[i]}, ");

                }
                else
                {
                    Console.Write($"{serviceTurnonDay[i]}, ");
                }
            }

            Console.WriteLine();
            (int numberOfInfectedServices, int[] listOfInfectedServices) =
                test.Stage3(g, K, s, serviceTurnoffDay, serviceTurnonDay);
            //Console.WriteLine($"wynik: {numberOfInfectedServices}");
            /*
            foreach (var service in listOfInfectedServices)
            {
                Console.Write($"{service}, ");
            }
            */
            // odp 2 : services{0, 1};
        }

        static void Main3(string[] args) //Test linia v4
        {
            int K = 3;
            int n = 3;
            Graph g = new Graph(n);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            int[] s = new int[] { 0 };
            int[] serviceTurnoffDay = new int[] { K + 1, 1, K + 1 };
            int[] serviceTurnonDay = new int[] { K + 1, 2, K + 1 };
            var test = new Lab04();
            Console.Write("ser:");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"{i}, ");
            }

            Console.WriteLine();
            Console.Write("off:");
            for (int i = 0; i < n; i++)
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
            Console.Write("on :");
            for (int i = 0; i < n; i++)
            {
                if (i >= 10)
                {
                    Console.Write($" {serviceTurnonDay[i]}, ");

                }
                else
                {
                    Console.Write($"{serviceTurnonDay[i]}, ");
                }
            }

            Console.WriteLine();
            (int numberOfInfectedServices, int[] listOfInfectedServices) =
                test.Stage3(g, K, s, serviceTurnoffDay, serviceTurnonDay);
            Console.WriteLine($"wynik: {numberOfInfectedServices}");
            foreach (var service in listOfInfectedServices)
            {
                Console.Write($"{service}, ");
            }
            // odp 2 : services{0, 1};
        }
        static void Main4(string[] args) //Test propagacji v3
        {
            int K = 5;
            int n = 4;
            Graph g = new Graph(n);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            int[] s = new int[] { 0 };
            int[] serviceTurnoffDay = new int[] {1, 1, K + 1, K + 1 };
            int[] serviceTurnonDay = new int[] { 1, 2, K + 1, K + 1 };
            var test = new Lab04();
            Console.Write("ser:");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"{i}, ");
            }

            Console.WriteLine();
            Console.Write("off:");
            for (int i = 0; i < n; i++)
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
            Console.Write("on :");
            for (int i = 0; i < n; i++)
            {
                if (i >= 10)
                {
                    Console.Write($" {serviceTurnonDay[i]}, ");

                }
                else
                {
                    Console.Write($"{serviceTurnonDay[i]}, ");
                }
            }

            Console.WriteLine();
            (int numberOfInfectedServices, int[] listOfInfectedServices) =
                test.Stage3(g, K, s, serviceTurnoffDay, serviceTurnonDay);
            Console.WriteLine($"wynik: {numberOfInfectedServices}");
            foreach (var service in listOfInfectedServices)
            {
                Console.Write($"{service}, ");
            }
            // odp 4 : services{0, 1, 2, 3};
        }

        static void Main(string[] args) // Test szefa
        {
            Graph h = new Graph(2);
            h.AddEdge(0, 1);
            int[] s = new int[] { 0 };
            int[] serviceTurnoffDay = new int[] { 4, 1 };
            int[] serviceTurnonDay = new int[] { 10, 7 };
            int n = 2;
            int m = 1;
            int K = 9;
            //addStage3(new Stage3TestCase(new int[] { 0 }, new int[] { 4, 1 }, new int[] { 10, 7 }, h, 2, 1, 9, 1, new int[] { 0 }, 1, "Test szefa"));
            var test = new Lab04();
            test.Stage3(h, K, s, serviceTurnoffDay, serviceTurnonDay);
            //odp = 1 ser: {0}

        }
    }
}
