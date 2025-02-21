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
            return (false, "");
        }
    }
}