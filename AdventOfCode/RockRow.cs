using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class RockRow
    {
        public RockRow(int width, int offset)
        {
            Width = width;
            Offset = offset;
        }
        public int Width { get; set; }
        public int Offset { get; set; }
    }
}
