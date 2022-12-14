using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
public static    class Helpers
    {
        public static bool IsBetween(this int x, int bound1, int bound2)
        {
            var bounds = new List<int>() { bound1, bound2 };
            return x <= bounds.Max() && x >= bounds.Min();
        }
    }
}
