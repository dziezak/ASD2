namespace Lab10;

public class Program4
{
   private static int[,] maze =
   {
      { 1, 0, 0, 0 },
      { 1, 1, 0, 1 },
      { 0, 1, 0, 0 },
      { 1, 1, 1, 1 }
   };

   private static int N = 4;
   static int[,] solution = new int[N, N];

   static bool SolveMaze(int x, int y)
   {
      if (x == N - 1 && y == N - 1 && maze[x, y] == 1)
      {
         solution[x, y] = 1;
         return true;
      }

      if (IsSafe(x, y) && solution[x, y] == 0)
      {
         solution[x, y] = 1;
         
         if(SolveMaze(x, y + 1)) return true;
         if(SolveMaze(x + 1, y)) return true;
         if(SolveMaze(x - 1, y)) return true;
         if(SolveMaze(x, y - 1)) return true;
         
         //nawrot!
         solution[x, y] = 0;
         return false;
      }

      return false;
   }

   static bool IsSafe(int x, int y)
   {
      if (x >= 0 && x < N && y >= 0 && y < N && maze[x, y] == 1) return true;
      return false;
   }

   static void PrintSolution()
   {
      for (int i = 0; i < solution.GetLength(0); i++)
      {
         for (int j = 0; j < solution.GetLength(1); j++)
         {
            Console.Write(solution[i, j] + " ");
         }
         Console.WriteLine();
      }
   }

   static void Main4()
   {
      if(SolveMaze(0, 0))
         PrintSolution();
      else
         Console.WriteLine("Brak rozwiazania");
   }
}