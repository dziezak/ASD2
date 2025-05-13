namespace Lab10;

public class SudokuSolver
{
    const int N = 9;

    static int[,] board =
    {
        { 5, 3, 0, 0, 7, 0, 0, 0, 0 },
        { 6, 0, 0, 1, 9, 5, 0, 0, 0 },
        { 0, 9, 8, 0, 0, 0, 0, 6, 0 },

        { 8, 0, 0, 0, 6, 0, 0, 0, 3 },
        { 4, 0, 0, 8, 0, 3, 0, 0, 1 },
        { 7, 0, 0, 0, 2, 0, 0, 0, 6 },

        { 0, 6, 0, 0, 0, 0, 2, 8, 0 },
        { 0, 0, 0, 4, 1, 9, 0, 0, 5 },
        { 0, 0, 0, 0, 8, 0, 0, 7, 9 }
    };

    static bool Solve()
    {
        for (int row = 0; row < N; row++)
        {
            for (int col = 0; col < N; col++)
            {
                if (board[row, col] == 0)
                {
                    for (int number = 1; number <= N; number++)
                    {
                        if (IsSafe(row, col, number))
                        {
                            board[row, col] = number;
                            if (Solve())
                                return true;
                            board[row, col] = 0;
                        }
                    }
                    return false; // czy potrzebne?
                }
            }
        }
        return true; // pelna plansza
    }

    static bool IsSafe(int row, int col, int number)
    {
        for (int x = 0; x < N; x++)
            if (board[row, x] == number)
                return false;
        
        for(int y = 0; y < N; y++)
            if (board[y, col] == number)
                return false;
        int startRow = row - row % 3;
        int startCol = col - col % 3;
        for(int i=0; i<3; i++)
            for(int j=0; j<3; j++)
                if(board[startRow + i, startCol + j] == number)
                    return false;
        return true;
    }

    static void PrintBoard()
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();      
        }
    }

    public static void Main()
    {
        if (Solve())
            PrintBoard();
        else
            Console.WriteLine("No solution");
    }
}