using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Line
    {
        public Line(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public List<Point> GetPoints()
        {
            if (IsHorizontal)
            {
                return Enumerable.Range(Math.Min(Point1.X, Point2.X), Math.Abs(Point1.X - Point2.X) +1)
                    .Select(x => new Point(x, Point1.Y)).ToList();
            }
            else
            {
                return Enumerable.Range(Math.Min(Point1.Y, Point2.Y), Math.Abs(Point1.Y - Point2.Y) + 1)
                    .Select(y => new Point(Point1.X, y)).ToList();
            }
        }

        public Point Point1 { get; private set; }
        public Point Point2 { get; private set; }
        public bool IsHorizontal => Point1.Y == Point2.Y;
        public bool IsVertical => Point1.X == Point2.X;
        public int MaxY()
        {
            return Math.Max(Point1.Y, Point2.Y);
        }
    }
}
