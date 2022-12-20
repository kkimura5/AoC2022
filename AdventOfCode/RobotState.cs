using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class RobotState
    {
        private Dictionary<string, int> toBuy = new Dictionary<string, int>();
        public RobotState()
        {
            toBuy.Add("ore", 0);
            toBuy.Add("clay", 0);
            toBuy.Add("obsidian", 0);
            toBuy.Add("geode", 0);
        }
        public int OreRobotCount { get; set; }
        public int ClayRobotCount { get; set; }
        public int ObsidianRobotCount { get; set; }
        public int GeodeRobotCount { get; set; }
        public int CurrentMinute { get; set; }
        public int OreCount { get; set; }
        public int ObsidianCount { get; set; }
        public int ClayCount { get; set; }
        public int GeodeCount { get; set; }

        public void Buy(string robotType)
        {
            toBuy[robotType]++;
        }

        public void EndMinute()
        {
            OreCount += OreRobotCount;
            ClayCount += ClayRobotCount;
            ObsidianCount += ObsidianRobotCount;
            GeodeCount += GeodeRobotCount;
            CurrentMinute++;

            OreRobotCount += toBuy["ore"];
            ClayRobotCount += toBuy["clay"];
            ObsidianRobotCount += toBuy["obsidian"];
            GeodeRobotCount += toBuy["geode"];

            toBuy["ore"] = 0;
            toBuy["clay"] = 0;
            toBuy["obsidian"] = 0;
            toBuy["geode"] = 0;
        }
    }
}
