using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class SandCastle
    {
        bool[,] occupiedMap;
        private int floorvalue;

        public SandCastle(List<Line> lines)
        {
            Lines = lines;
            var points = lines.SelectMany(x => x.GetPoints()).Distinct().ToList();
            var xRange = points.Max(x => x.X);
            var yRange = points.Max(x => x.Y);
            occupiedMap = new bool[xRange + 500, yRange + 3];
            
            foreach (var point in points)
            {
                occupiedMap[point.X, point.Y] = true;
            }

            floorvalue = yRange + 2;
        }

        public bool IsUsingFloor { get; set; }

        public void DropSand(Point startingPoint)
        {
            var currentPoint = startingPoint;

            while (true)
            {
                var nextPoint = GetNextPoint(currentPoint);

                if (nextPoint == currentPoint)
                {
                    occupiedMap[currentPoint.X, currentPoint.Y] = true;
                    IsDone = currentPoint == startingPoint;
                    SandCount++;
                    break;
                } 
                else if (!IsUsingFloor && IsBelowAllLines(currentPoint))
                {
                    IsDone = true;
                    break;
                }
                else
                {
                    currentPoint = nextPoint;
                }
            }
        }

        private Point GetNextPoint(Point currentPoint)
        {
            var point1 = new Point(currentPoint.X, currentPoint.Y + 1);
            var point2 = new Point(currentPoint.X - 1, currentPoint.Y + 1);
            var point3 = new Point(currentPoint.X + 1, currentPoint.Y + 1);

            if (!occupiedMap[point1.X, point1.Y] && (!IsUsingFloor || point1.Y != floorvalue))
            {
                return point1;
            }
            else if (!occupiedMap[point2.X, point2.Y] && (!IsUsingFloor || point2.Y != floorvalue))
            {
                return point2;
            }
            else if (!occupiedMap[point3.X, point3.Y] && (!IsUsingFloor || point3.Y != floorvalue))
            {
                return point3;
            }
            else
            {
                return currentPoint;
            }
        }

        public long SandCount { get; set; }
        public List<Line> Lines { get; }
        public bool IsDone { get; set; }

        public bool IsBelowAllLines(Point point)
        {
            return Lines.All(x => point.Y > x.MaxY());
        }
    }
}
