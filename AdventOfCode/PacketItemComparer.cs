using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class PacketItemComparer : IComparer<PacketItem>
    {
        public int Compare(PacketItem x, PacketItem y)
        {
            if (x.IsInCorrectOrder(y) == true)
            {
                return -1;
            }
            else if (x.IsInCorrectOrder(y) == false)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

    }
}
