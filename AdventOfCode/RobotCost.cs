using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class RobotCost
    {
        public RobotCost(int oreCost, int clayCost, int obsidianCost)
        {
            OreCost = oreCost;
            ClayCost = clayCost;
            ObsidianCost = obsidianCost;
        }

        public int OreCost { get; set; }
        public int ClayCost { get; set; }
        public int ObsidianCost { get; set; }
    }
}
