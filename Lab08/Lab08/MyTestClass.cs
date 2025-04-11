using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD
{
    public class MyTestClass
    {
        static void Main0(string[] args)
        {
            Console.WriteLine("Przyklad z tresci zadania");
            int[,] P = new int[,]
            {
                {1, 1, 0, 0},
                {1, 1, 1, 1},
                {1, 1, 0, 1}
            };
            (int row, int col)[] M = new (int row, int col)[] { (2, 1), (2, 3) };
            int[] W = new int[] { 1000, 400 };
            int k = 100;
            int bestCost = 700;
            List<List<int>> solutions = new List<List<int>>()
            {
                new List<int>(){0}
            };
            var test = new Lab08();
            var ( bestProfit, saved) = test.Stage2(P, M, W, k); // Plansza, Machines, Worth, Koszt // goscia nazwy ig
            Console.WriteLine($"Best Profit = {bestProfit}, should be {bestCost}");
            Console.WriteLine("Saved machines:");
            foreach (var m in saved)
            {
                Console.Write($"{m} ");
            }
            Console.WriteLine("\nMaschines that SHOULD be saved:");
            int numer = 1;
            foreach (var m in solutions)
            {
                Console.Write($"solution #{numer++}: "); 
                foreach (var w in m)
                {
                    Console.Write($"{w} ");
                }
                Console.WriteLine();
            }
        }


       
        /*
        static public void Main1(string[] args)
        {
            (int[,] P, (int row, int col)[] M, int[] W, int k) randomStageII(int h, int w, int numMachines, int maxP, int maxW, int maxK, int seed)
            {
                Random random = new Random(seed);
                int[,] P = new int[h, w];
                for (int r = 0; r < h; ++r)
                {
                    for (int c = 0; c < w; ++c)
                    {
                        // P[r, c] = random.Next(0, maxP+1);
                        P[r, c] = random.Next(0, maxP + 1);
                    }
                }
                (int row, int col)[] M = GenerateDistinctPairs(h, w, numMachines, random);
                int[] W = new int[numMachines];
                for(int i = 0; i < numMachines; ++i)
                {
                    W[i] = random.Next(1, maxW+1);
                }
                int k = random.Next(1, maxK+1);
                return (P, M, W, k);
            }
            (int row, int col)[] GenerateDistinctPairs(int h, int w, int n, Random random)
            {
                var distinctPairs = new HashSet<(int row, int col)>();

                while (distinctPairs.Count < n)
                {
                    int x = random.Next(0, h);
                    int y = random.Next(0, w);

                    distinctPairs.Add((x, y));
                }

                return distinctPairs.ToArray();
            }
            var (P, M, W, k) = randomStageII(30, 10, 12, 8, 100, 5, 948);
            List<List<int>> solutions = new List<List<int>>() { new List<int>() {1, 8}, new List<int>() { 0, 1, 8 } };
            int bestCost = 85;
            // Utils.generateAllStage2Solutions(P, M, W, k, "out2.txt");
            var test = new Lab08();
            var(bestProfit, saved) = test.Stage2(P, M, W, k);
            if(bestCost == bestProfit) Console.Write("Test 7 OK\n"); 
        }
        */
       
    }
}