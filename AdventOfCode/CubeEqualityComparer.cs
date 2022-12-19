using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class CubeEqualityComparer : IEqualityComparer<Cube>
    {
        public bool Equals(Cube x, Cube y)
        {
            return x.IsCoincident(y);
        }

        public int GetHashCode(Cube obj)
        {
            return obj.X ^ obj.Y ^ obj.Z;
        }
    }
}
