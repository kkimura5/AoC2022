using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Sensor
    {
        public Sensor(string line)
        {
            var pattern = @"Sensor at x=(?<locx>-?\d+), y=(?<locy>-?\d+): closest beacon is at x=(?<beaconx>-?\d+), y=(?<beacony>-?\d+)";
            var match = Regex.Match(line, pattern);
            Location = new Point(int.Parse(match.Groups["locx"].Value), int.Parse(match.Groups["locy"].Value));
            ClosestBeacon = new Point(int.Parse(match.Groups["beaconx"].Value), int.Parse(match.Groups["beacony"].Value));
        }

        public Point Location { get; set; }
        public Point ClosestBeacon { get; private set; }
        public long Distance => Math.Abs(Location.X - ClosestBeacon.X) + Math.Abs(Location.Y - ClosestBeacon.Y);

        public Tuple<long, long> GetXRangeWithNoBeacon(int row)
        {
            var difference = Math.Abs(Location.Y - row);
            if (Distance < difference)
            {
                return new Tuple<long, long>(0, 0);
            }

            var numValues = 2 * (Distance - difference) + 1;

            return new Tuple<long, long>(Location.X - (numValues - 1) / 2, Location.X + (numValues - 1) / 2);
        }

    }
}
