using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Path
    {

        public Path(Point start, char height)
        {
            CurrentPoint = start;
            Height = height;
            NumSteps = 0;
        }

        public Path()
        {

        }

        public int NumSteps { get; set; }
        public Point CurrentPoint { get; set; }
        public char Height { get; set; }

        public Path Copy()
        {
            return new Path()
            {
                NumSteps = NumSteps,
                Height = Height,
                CurrentPoint = CurrentPoint
            };
        }

        public List<Point> GetNewPoints()
        {
            var points = new List<Point>()
            {
                new Point(CurrentPoint.X-1, CurrentPoint.Y),
                new Point(CurrentPoint.X+1, CurrentPoint.Y),
                new Point(CurrentPoint.X, CurrentPoint.Y-1),
                new Point(CurrentPoint.X, CurrentPoint.Y+1),
            };

            return points;
        }
    }
}
