using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class CubeLocation
    {

        public CubeLocation(Point location, SideName sideName, Heading heading)
        {
            Location = location;
            SideName = sideName;
            HeadingOnSide = heading;
        }

        public Point Location { get; set; }
        public SideName SideName { get; set; }
        public Heading HeadingOnSide { get; set; }

        public override string ToString()
        {
            return $"Location: ({Location.X},{Location.Y}); Side: {SideName}, heading: {HeadingOnSide}";
        }
    }
}
