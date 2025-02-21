using System;
using System.Runtime.Intrinsics.Arm;

namespace ASD
{
    class CrossoutChecker
    {
        private bool[,]? erasable; // czy możliwe
        private int[,]? minCrossouts; //minimalna ilosc operacji aby wytrzec 
        private int[,]? minRemainder; //obliczy najmniejszy pozostaly wzorzec
        /// <summary>
        /// Sprawdza, czy podana lista wzorców zawiera wzorzec x
        /// </summary>
        /// <param name="patterns">Lista wzorców</param>
        /// <param name="x">Jedyny znak szukanego wzorca</param>
        /// <returns></returns>
        bool comparePattern(char[][] patterns, char x)
        {
            foreach (char[] pat in patterns)
            {
                if (pat.Length == 1 && pat[0] == x)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Sprawdza, czy podana lista wzorców zawiera wzorzec xy
        /// </summary>
        /// <param name="patterns">Lista wzorców</param>
        /// <param name="x">Pierwszy znak szukanego wzorca</param>
        /// <param name="y">Drugi znak szukanego wzorca</param>
        /// <returns></returns>
        bool comparePattern(char[][] patterns, char x, char y)
        {
            foreach (char[] pat in patterns)
            {
                if (pat.GetLength(0) == 2 && pat[0] == x && pat[1] == y)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Metoda sprawdza, czy podany ciąg znaków można sprowadzić do ciągu pustego przez skreślanie zadanych wzorców.
        /// Zakładamy, że każdy wzorzec składa się z jednego lub dwóch znaków!
        /// </summary>
        /// <param name="sequence">Ciąg znaków</param>
        /// <param name="patterns">Lista wzorców</param>
        /// <param name="crossoutsNumber">Minimalna liczba skreśleń gwarantująca sukces lub int.MaxValue, jeżeli się nie da</param>
        /// <returns></returns>
        public bool Erasable(char[] sequence, char[][] patterns, out int crossoutsNumber)
        {
            bool wys = false;
            int n = sequence.Length;
            erasable = new bool[n, n];
            minCrossouts = new int[n, n];

            if (wys)
            {
                Console.WriteLine("To jest sequence:"); 
                for(int i=0; i<n; i++)
                    Console.Write(sequence[i]);
                Console.WriteLine();
            }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    minCrossouts[i, j] = int.MaxValue;

            for(int l=1; l<=n; l++)
            {
                for(int i=0; i+l-1<n; i++) // rozmaptrujemy przedzial [i,j] o dlugosci l
                {
                    int j = i + l - 1; // tu -1 bo jak kalendarz
                    if(((l == 1 ) && comparePattern(patterns, sequence[i])) ||
                        ((l == 2) && comparePattern(patterns, sequence[i], sequence[i+1])))
                    {
                        erasable[i, j] = true;
                        minCrossouts[i, j] = 1;
                        //continue;
                    }

                    for(int k = i; k<j; k++)
                    {
                        if(erasable[i, k] && erasable[k+1, j])
                        {
                            erasable[i, j] = true;
                            minCrossouts[i, j] = minCrossouts[i, k] + minCrossouts[k + 1, j];
                        }
                    }

                    if( i<j && i+1 <= j-1)
                    {
                        if (erasable[i+1, j - 1] && comparePattern( patterns, sequence[i], sequence[j]))
                        {
                            erasable[i, j] = true;
                            minCrossouts[i, j] = minCrossouts[i + 1, j - 1] + 1;
                        }
                    }
                }
            }
            if(wys) 
                for(int i= 0; i < n; i++)
                {
                    for(int j= 0; j < n; j++)
                    {
                        if(erasable[i, j])
                            Console.Write("1 ");
                        else 
                            Console.Write("0 ");
                    }
                    Console.Write('\n');
                }
            
            crossoutsNumber = erasable[0, n - 1] ? minCrossouts[0, n - 1] : int.MaxValue;
            return erasable[0, n - 1];
        }

        /// <summary>
        /// Metoda sprawdza, jaka jest minimalna długość ciągu, który można uzyskać z podanego poprzez skreślanie zadanych wzorców.
        /// Zakładamy, że każdy wzorzec składa się z jednego lub dwóch znaków!
        /// </summary>
        /// <param name="sequence">Ciąg znaków</param>
        /// <param name="patterns">Lista wzorców</param>
        /// <returns></returns>
        public int MinimumRemainder(char[] sequence, char[][] patterns)
        {
            int n = sequence.Length;
            minRemainder = new int[n,n];
            for (int l = 1; l <= n; l++)
            {
                for(int i = 0; i + l -1 <n; i++)
                {
                    int j = i + l - 1;
                    if (((l == 1) && comparePattern(patterns, sequence[i])) ||
                        ((l == 2) && comparePattern(patterns, sequence[i], sequence[i+1])))
                    {
                        minRemainder[i, j] = 0;
                    }
                    else
                    {
                        minRemainder[i, j] = l;
                        for(int k = i; k <j; k++)
                        {
                            minRemainder[i, j] = Math.Min(minRemainder[i, j], minRemainder[i, k] + minRemainder[k + 1, j]);
                        }
                    }
                }
            }
            return minRemainder[0, n - 1];
        }
    }
}
