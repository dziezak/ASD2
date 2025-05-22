using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ASD
{
    [Serializable]
    public struct Point
    {
        public double x;
        public double y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Point p1, Point p2) { return p1.x == p2.x && p1.y == p2.y; }

        public static bool operator !=(Point p1, Point p2) { return !(p1 == p2); }

        public override string ToString()
        {
            return string.Format("({0},{1})", x, y);
        }
        public static double Distance(Point p1, Point p2)
        {
            double dx, dy;
            dx = p1.x - p2.x;
            dy = p1.y - p2.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

    }

    public class Lab12 : MarshalByRefObject
    {

        private class ByY : IComparer<Point>
        {
            public int Compare(Point p1, Point p2)
            {
                int res = p1.y.CompareTo(p2.y);
                return res == 0 ? p1.x.CompareTo(p2.x) : res;
            }
        }


        /// <summary>
        /// Metoda zwraca dwa najbliższe punkty w dwuwymiarowej przestrzeni Euklidesowej
        /// </summary>
        /// <param name="points">Chmura punktów</param>
        /// <param name="minDistance">Odległość pomiędzy najbliższymi punktami</param>
        /// <returns>Para najbliższych punktów. Kolejność nie ma znaczenia</returns>
        /// <remarks>
        /// 1) Algorytm powinien mieć złożoność O(n^2), gdzie n to liczba punktów w chmurze
        /// </remarks>
        public Tuple<Point, Point> FindClosestPointsBrute(List<Point> points, out double minDistance)
        {
            //minDistance = double.MaxValue;
            //Tuple<Point, Point> closestPair = null;

            //for (int i = 0; i < points.Count; i++)
            //{
            //    for (int j = i + 1; j < points.Count; j++)
            //    {
            //        double dist = Point.Distance(points[i], points[j]);
            //        if (dist < minDistance)
            //        {
            //            minDistance = dist;
            //            closestPair = Tuple.Create(points[i], points[j]);
            //        }
            //    }
            //}
            //return closestPair;
            return FindClosestPoints(points, out minDistance);
        }

        /// <summary>
        /// Metoda zwraca dwa najbliższe punkty w dwuwymiarowej przestrzeni Euklidesowej
        /// </summary>
        /// <param name="points">Chmura punktów</param>
        /// <param name="minDistance">Odległość pomiędzy najbliższymi punktami</param>
        /// <returns>Para najbliższych punktów. Kolejność nie ma znaczenia</returns>
        /// <remarks>
        /// 1) Algorytm powinien mieć złożoność n*logn, gdzie n to liczba punktów w chmurze
        /// </remarks>
        /// 
        /// 
        ///Hehe przechoidz Etap2:
        public Tuple<Point, Point> FindClosestPointsEtap2(List<Point> points, out double minDistance)
        {
            minDistance = double.MaxValue;
            Tuple<Point, Point> closetPair = null;

            if (points == null || points.Count < 2)
                throw new ArgumentException("zla ilosc punktow na wejsciu!");

            var sortedByX = points.OrderBy(p => p.x).ToList();
            var set = new SortedSet<Point>(new YComparer());

            int left = 0;
            for (int i = 0; i < sortedByX.Count; i++)
            {
                Point current = sortedByX[i];

                while (sortedByX[i].x - sortedByX[left].x > minDistance)
                {
                    set.Remove(sortedByX[left]);
                    left++;
                }

                var lowerBound = new Point(0, (double)(current.y - minDistance));
                var upperBound = new Point(0, (double)(current.y + minDistance));

                foreach (var point in set.GetViewBetween(lowerBound, upperBound))
                {
                    double distance = GetDistance(current, point);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closetPair = new Tuple<Point, Point>(current, point);
                    }
                }

                set.Add(current);
            }
            return closetPair;
        }
        
        public Tuple<Point, Point> FindClosestPoints(List<Point> points, out double minDistance)
        {
            minDistance = double.MaxValue;
            Tuple<Point, Point> closetPair = null;

            if (points == null || points.Count < 2)
                throw new ArgumentException("zla ilosc punktow na wejsciu!");

            var sortedByX = points.OrderBy(p=>p.x).ToList();
            var set = new SortedSet<Point>(new YComparer());

            int left = 0;
            for(int i=0; i<sortedByX.Count; i++)
            {
                Point current = sortedByX[i];

                while (sortedByX[i].x - sortedByX[left].x > minDistance)
                {
                    set.Remove(sortedByX[left]);
                    left++;
                }

                var lowerBound = new Point(0, (double)(current.y - minDistance));
                var upperBound = new Point(0, (double)(current.y + minDistance));

                foreach (var point in set.GetViewBetween(lowerBound, upperBound))
                {
                    double distance = GetDistance(current, point);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closetPair = new Tuple<Point, Point>(current, point);
                    }
                }

                set.Add(current);
            }
            return closetPair;
        }

        private double GetDistance(Point p1, Point p2)
        {
            double dx = p1.x - p2.x;
            double dy = p1.y - p2.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        private class YComparer : IComparer<Point>
        {
            public int Compare(Point a, Point b)
            {
                int cmp = a.y.CompareTo(b.y);
                if (cmp == 0)
                    return a.x.CompareTo(b.x);
                return cmp;
            }
        }

    }



    

}
