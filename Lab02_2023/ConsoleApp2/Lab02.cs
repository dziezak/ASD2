using System;
using System.Collections.Generic;
using System.Text;

namespace Lab02
{
    public class PatternMatching : MarshalByRefObject
    {
        /// <summary>
        /// Etap 1 - wyznaczenie trasy, zgodnie z którą robot przemieści się z pozycji poczatkowej (0,0) na pozycję docelową (-n-1, m-1)
        /// </summary>
        /// <param name="n">wysokość prostokąta</param>
        /// <param name="m">szerokość prostokąta</param>
        /// <param name="obstacles">tablica ze współrzędnymi przeszkód</param>
        /// <returns>krotka (bool result, string path) - result ma wartość true jeżeli trasa istnieje, false wpp., path to wynikowa trasa</returns>
        public (bool result, string path) Lab02Stage1(int n, int m, (int, int)[] obstacles)
        {
            HashSet<(int, int)> obstacleSet = new HashSet<(int, int)>(obstacles);
            Queue<((int, int) position, string path)> queue = new Queue<((int, int), string)>();
            queue.Enqueue(((0, 0), ""));
            HashSet<(int, int)> visited = new HashSet<(int, int)> { (0,0)};
            int[] dx = { 1, 0 };
            int[] dy = { 0, 1 };
            char[] moves = { 'D', 'R' };

            while(queue.Count > 0)
            {
                var (curr, path) = queue.Dequeue();
                int x = curr.Item1, y = curr.Item2;

                if(x == n-1 &&  y == m-1)
                    return (true, path);

                for(int i=0; i<2; i++)
                {
                    int newX = x + dx[i];
                    int newY = y + dy[i];
                    if(newX < n && newY < m && !obstacleSet.Contains((newX, newY)) && !visited.Contains((newX, newY)))
                    {
                        visited.Add((newX, newY));
                        queue.Enqueue(((newX, newY), path + moves[i]));
                    }
                }
            }
            return (false, "");
        }

        /// <summary>
        /// Etap 2 - wyznaczenie trasy realizującej zadany wzorzec, zgodnie z którą robot przemieści się z pozycji poczatkowej (0,0) na pozycję docelową (-n-1, m-1)
        /// </summary>
        /// <param name="n">wysokość prostokąta</param>
        /// <param name="m">szerokość prostokąta</param>
        /// <param name="pattern">zadany wzorzec</param>
        /// <param name="obstacles">tablica ze współrzędnymi przeszkód</param>
        /// <returns>krotka (bool result, string path) - result ma wartość true jeżeli trasa istnieje, false wpp., path to wynikowa trasa</returns>
        public (bool result, string path) Lab02Stage2(int n, int m, string pattern, (int, int)[] obstacles)
        {
            HashSet<(int, int)> obstacleSet = new HashSet<(int, int)>(obstacles);
            Queue<((int, int) position, string path, int patternIndex)> queue = new Queue<((int, int), string, int)>();
            queue.Enqueue(((0, 0), "", 0));
            HashSet<(int, int, int)> visited = new HashSet<(int, int, int)> { (0, 0, 0) };

            while (queue.Count > 0)
            {
                var (current, path, patternIndex) = queue.Dequeue();
                int x = current.Item1, y = current.Item2;

                if (x == n - 1 && y == m - 1 && patternIndex == pattern.Length && path.Length == n+m-2)
                    return (true, path);
                if (patternIndex < pattern.Length && path.Length < n+m)
                {
                    char move = pattern[patternIndex];

                    void TryEnqueue(int newX, int newY, string moveChar, int newPatternIndex)
                    {
                        if (newX < n && newY < m && !obstacleSet.Contains((newX, newY)) && !visited.Contains((newX, newY, newPatternIndex)))
                        {
                            visited.Add((newX, newY, newPatternIndex));
                            Console.WriteLine($"{path + moveChar}, {newX + newY}");
                            queue.Enqueue(((newX, newY), path + moveChar, newPatternIndex));
                        }
                    }

                    if (move == 'D') TryEnqueue(x + 1, y, "D", patternIndex+1);
                    else if (move == 'R') TryEnqueue(x, y + 1, "R", patternIndex+1);
                    else if (move == '?')
                    {
                        TryEnqueue(x + 1, y, "D", patternIndex+1);
                        TryEnqueue(x, y + 1, "R", patternIndex+1);
                    }
                    else if (move == '*')
                    {
                        for (int i = x + 1; i < n && !obstacleSet.Contains((i, y)); i++)
                        {
                            string addPath = new string('D', i - x);
                            TryEnqueue(i, y, addPath, patternIndex+1);
                            //TryEnqueue(i, y, "D", patternIndex+1);
                        }
                    
                        for (int j = y + 1; j < m && !obstacleSet.Contains((x, j)); j++)
                        {
                            string addPath = new string('R', j - y); 
                            TryEnqueue(x, j, addPath, patternIndex+1);    
                            //TryEnqueue(x, j, "R", patternIndex+1);    
                        }
                    }
                }

            }
            return (false, "");
        }
    }
}