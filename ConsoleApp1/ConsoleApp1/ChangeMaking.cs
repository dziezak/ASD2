﻿
using Microsoft.Win32.SafeHandles;
using System;
using System.Reflection.Metadata;

namespace ASD
{

    class ChangeMaking
    {

        /// <summary>
        /// Metoda wyznacza rozwiązanie problemu wydawania reszty przy pomocy minimalnej liczby monet
        /// bez ograniczeń na liczbę monet danego rodzaju
        /// </summary>
        /// <param name="amount">Kwota reszty do wydania</param>
        /// <param name="coins">Dostępne nominały monet</param>
        /// <param name="change">Liczby monet danego nominału użytych przy wydawaniu reszty</param>
        /// <returns>Minimalna liczba monet potrzebnych do wydania reszty</returns>
        /// <remarks>
        /// coins[i]  - nominał monety i-tego rodzaju
        /// change[i] - liczba monet i-tego rodzaju (nominału) użyta w rozwiązaniu
        /// Jeśli dostepnymi monetami nie da się wydać danej kwoty to change = null,
        /// a metoda również zwraca null
        ///
        /// Wskazówka/wymaganie:
        /// Dodatkowa uzyta pamięć powinna (musi) być proporcjonalna do wartości amount ( czyli rzędu o(amount) )
        /// </remarks>
        public int? NoLimitsDynamic(int amount, int[] coins, out int[] change)
        {
            int[] T = new int[amount+1];
            int[] P = new int[amount+1];

            for(int i=1; i<=amount; i++)
            {
                T[i] = int.MaxValue;
                P[i] = -1;
            }
            T[0] = 0;
            for (int kk = 1; kk <= amount; kk++)
            {
                foreach (var coin in coins)
                {
                    if (kk >= coin) 
                    {
                        int c = T[kk - coin] + 1;  
                        if (c < T[kk] && c >=0)  
                        {
                            T[kk] = c;
                            P[kk] = coin;
                        }
                    }
                }
            }

            if (T[amount] == int.MaxValue)
            {
                change = null;
                return null;
            }

            change = new int[coins.Length];
            int currentAmount = amount;

            while (currentAmount > 0)
            {
                int coin = P[currentAmount];  
                int coinIndex = Array.IndexOf(coins, coin);
                change[coinIndex]++;
                currentAmount -= coin;
            }

            return T[amount];
        }

        /// <summary>
        /// Metoda wyznacza rozwiązanie problemu wydawania reszty przy pomocy minimalnej liczby monet
        /// z uwzględnieniem ograniczeń na liczbę monet danego rodzaju
        /// </summary>
        /// <param name="amount">Kwota reszty do wydania</param>
        /// <param name="coins">Dostępne nominały monet</param>
        /// <param name="limits">Liczba dostępnych monet danego nomimału</param>
        /// <param name="change">Liczby monet danego nominału użytych przy wydawaniu reszty</param>
        /// <returns>Minimalna liczba monet potrzebnych do wydania reszty</returns>
        /// <remarks>
        /// coins[i]  - nominał monety i-tego rodzaju
        /// limits[i] - dostepna liczba monet i-tego rodzaju (nominału)
        /// change[i] - liczba monet i-tego rodzaju (nominału) użyta w rozwiązaniu
        /// Jeśli dostepnymi monetami nie da się wydać danej kwoty to change = null,
        /// a metoda również zwraca null
        ///
        /// Wskazówka/wymaganie:
        /// Dodatkowa uzyta pamięć powinna (musi) być proporcjonalna do wartości iloczynu amount*(liczba rodzajów monet)
        /// ( czyli rzędu o(amount*(liczba rodzajów monet)) )
        /// </remarks>
       
        public int? Dynamic(int amount, int[] coins, int[] limits, out int[] change)
        {
            int n = coins.Length;
            int[,] T = new int[n + 1, amount + 1];
            int[,] P = new int[n + 1, amount + 1];
            for(int i=1; i<=amount; i++)
            {
                T[0, i] = int.MaxValue;
                P[0, i] = -1;
            }

            for(int i=1; i<=n; i++)
            {
                for(int j=1; j<=amount; j++)
                {
                    T[i, j] = T[i - 1, j];
                    P[i, j] = -1;
                    if (coins[i-1] <= j) {
                        for (int kk = 1; kk <= limits[i-1] && coins[i-1]*kk <= j; kk++)
                        {
                            if (T[i-1, j - kk * coins[i-1]] != int.MaxValue)
                            {
                                int newCount = T[i - 1, j - kk * coins[i - 1]] + kk;
                                if(newCount < T[i, j])
                                {
                                    T[i, j] = newCount;
                                    P[i, j] = kk;
                                }
                            }
                        }
                    }
                }
            }
            if (T[n, amount] == int.MaxValue)
            {
                change = null;
                return null;
            }

            change = new int[n];
            int remainingAmount = amount;

            for(int i=n; i>0 && remainingAmount>0; i--)
            {
                if (P[i, remainingAmount] != -1)
                {
                    change[i-1] = P[i, remainingAmount];
                    remainingAmount -= change[i - 1] * coins[i - 1];
                }
            }
            return T[n, amount];
        }

    }

}
