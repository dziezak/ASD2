//#define Reszta
using System.Runtime.Intrinsics.Arm;
using System.Threading.Channels;


namespace Programowanie_dynamiczne
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if Reszta
            int n = 15, k = 5;
            int[]? dp = new int[n+1];
            //int[] c = new int[k];
            int[] c = { 2, 3, 12, 11, 25}; // moenty


            /*
            for (int i = 0;  i < k; i++) // tworzymy randomowe reszty
            {
                c[i] = i+2;
            }
            */
            


            for(int j=1; j<=n; j++)
            {
                dp[j] = int.MaxValue - 1;
            }
            dp[0] = 0;


            for(int j=0; j<k ; j++)
            {
                for(int i=c[j]; i<=n; i++)
                {
                    dp[i] = Math.Min(dp[i - c[j]] + 1, dp[i]);
                }
            }
            for(int j  = 1; j<=n; j++)
                Console.Write(j + " ");
            Console.WriteLine();
            for(int j=1; j<=n;j++)
            {
                Console.Write(dp[j]+" ");
            }
#endif
            
        }

    }
}
