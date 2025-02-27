using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace ASD
{
    public class Lab02 : MarshalByRefObject
    {
        /// <summary>
        /// Etap 1 - Wyznaczenie ścieżki (seam) o minimalnym sumarycznym score.
        /// Ścieżka przebiega od górnego do dolnego wiersza obrazu.
        /// </summary>
        /// <param name="S">macierz score o wymiarach H x W, gdzie S[i, j] reprezentuje "ważność" piksela w wierszu i i kolumnie j</param>
        /// <returns>
        /// (int cost, (int, int)[] seam) - 
        /// cost: minimalny łączny koszt ścieżki (suma wartości pikseli);
        /// seam: tablica pozycji pikseli (włącznie z pikselem z pierwszego i ostatniego wiersza) tworzących ścieżkę.
        /// </returns>
        public (int cost, (int i, int j)[] seam) Stage1(int[,] S)
        {
            int H = S.GetLength(0);
            int W = S.GetLength(1);
            int[,] dp = new int[H, W];

            (int,int)[,] position = new (int,int)[H, W];

            (int, int)[] seam = new (int, int)[H];


            for(int j = 0; j<W; j++)
            {
                dp[0, j] = S[0, j];
                position[0, j] = (0, j);
            }



            //pomysl prostszy: dodajmey koszt wymagany czyli S[i, j]
            //i robimy deafaut z jako góre i potem lecmy lewo albo prawo jeśli się da
            for (int i=1; i<H; i++)
            {
                for(int j=0; j<W; j++)
                {
                    dp[i, j] = S[i, j] + dp[i - 1, j];
                    position[i, j] = (i - 1, j);
                    if (j > 0 && S[i, j] + dp[i-1, j-1] < dp[i, j])
                    {
                        dp[i,j] = S[i, j] + dp[i -1, j - 1];
                        position[i, j] = (i - 1, j-1);
                    }
                    if (j < W-1 && S[i, j] + dp[i-1, j+1] < dp[i, j])
                    {
                        dp[i, j] = S[i, j] + dp[i - 1, j + 1];
                        position[i, j] = (i - 1, j + 1);
                    }
                }
            }

            //wyznaczam index dla dla najmniejszej sciezki:
            int index = 0;
            int odp = int.MaxValue;
            for(int j=0; j<W; j++)
            {
                if(odp > dp[H-1, j])
                {
                    odp = dp[H-1, j];
                    index = j;
                }
            }
            seam[H - 1] = (H - 1, index);
            //znacznie prosciej niz wczeniej bo bez if'ow!!! 
            // zdobywamy teras sciezke: fajny pomsyl -> jak mamy wsm elementy to
            //pomzemy zmienic index na obecny j taki jaki tam jest zapisany
            int previousI, previousJ;
            for (int i = H - 1; i > 0; i--)
            {
                (previousI, previousJ) = position[i, index];
                seam[i - 1] = (previousI, previousJ);
                index = previousJ;
            }
            (previousI, previousJ) = position[0, index];
            seam[0] = (previousI, previousJ);
            return (odp, seam);
        }

        /// <summary>
        /// Etap 2 - Wyznaczenie ścieżki (seam) o minimalnym sumarycznym score z uwzględnieniem kary za zmianę kierunku.
        /// Przy każdym przejściu, gdy kierunek ruchu różni się od poprzedniego, do łącznego kosztu dodawana jest kara K.
        /// Pierwszy krok (z pierwszego wiersza) nie podlega karze.
        /// </summary>
        /// <param name="S">macierz score o wymiarach H x W</param>
        /// <param name="K">kara za zmianę kierunku (K >= 1)</param>
        /// <returns>
        /// (int cost, (int, int)[] seam) - 
        /// cost: minimalny łączny koszt ścieżki (suma wartości pikseli oraz naliczonych kar);
        /// seam: tablica pozycji pikseli tworzących ścieżkę.
        /// </returns>

        //Pomsył: Robimy to samo, tylko bierzemy jeszcze pod uwage skad idziemy ( jako 3 wymiar)
        public (int cost, (int i, int j)[] seam) Stage2(int[,] S, int K)
        {
            int H = S.GetLength(0);
            int W = S.GetLength(1);
            int[,,] dp = new int[H, W, 3];

            (int, int)[,] position = new (int, int)[H, W];
            (int, int)[] seam = new (int, int)[H];


            //uzupelnijmy gore
            for (int j = 0; j < W; j++)
            {
                for (int dim = 0; dim < 3; dim++)
                    dp[0, j, dim] = S[0, j];
                position[0, j] = (0, j);
            }

            for (int i = 1; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    for (int dim = 0; dim < 3; dim++)
                    {
                        dp[i, j, dim] = int.MaxValue - 1000000; // zobaczymy czy -1 jest wazne wsm
                    }
                }
            }

            //uzupelnijmu poziom 1 ( czyli drugi od gory) bo bez Kar jest
            for (int j = 0; j < W; j++)
            {
                for (int dim = 0; dim < 3; dim++)
                {
                    for(int dimWczes = 0; dimWczes<3; dimWczes++)
                    {
                        if (!((j == 0) && (dim == 0)) && !((j == W - 1) && (dim == 2)))
                        {
                            dp[1, j, dim] = Math.Min(dp[0, j + dim-1, dimWczes] + S[1, j], dp[1, j, dim]);
                        }
                    }
                }
            }

            for(int i=2; i<H; i++)
            {
                for(int j=0; j<W; j++)
                {
                    for(int dim = 0; dim < 3; dim++)
                    {
                        for(int dimWczes = 0; dimWczes<3; dimWczes++)
                        {
                            if (!((j == 0) && (dim == 0)) && !((j == W - 1) && (dim == 2)))
                            {
                                int kara = (dimWczes == dim) ? 0 : K;
                                dp[i, j, dim] =Math.Min(dp[i-1, j + dim - 1, dimWczes] + kara + S[i, j], dp[i, j, dim]);
                            }
                        }
                    }
                }
            }

            ///wyznaczam index dla dla najmniejszej sciezki:
            int indexJ = 0;
            int indexDim = 0;
            int odp = int.MaxValue;
            for (int j = 0; j < W; j++)
            {
                for(int dim = 0; dim<3; dim++)
                {
                    if (dp[H - 1, j, dim] < odp)
                    {
                        odp = dp[H - 1, j, dim];
                        indexJ = j;
                        indexDim = dim;
                    }
                }
            }
            return (odp, seam);
        }
    }
}
