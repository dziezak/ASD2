namespace Lab10;
using System;

class EightQueens
{
    const int N = 4;
    static int[] board = new int[N];
    public static int number = 0;

    static void Solve(int row)
    {
        if (row == N)
        {
            Console.WriteLine(number++);
            PrintBoard();
            return;
        }

        for (int col = 0; col < N; col++)
        {
            if (IsSafe(row, col))
            {
                board[row] = col;
                Solve(row + 1);
            }
        }
    }

    static bool IsSafe(int row, int col)
    {
        for (int i = 0; i < row; i++)
        {
            if (board[i] == col || Math.Abs(board[i] - col) == Math.Abs(i - row))
                return false;
        }
        return true;
    }

    static void PrintBoard()
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (board[i] == j) Console.Write("Q ");
                else Console.Write(". ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    static void Main3()
    {
        Solve(0);
    }
}
