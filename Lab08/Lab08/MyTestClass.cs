using System;
using System.Collections.Generic;

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
    }
}