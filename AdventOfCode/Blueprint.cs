using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Blueprint
    {
        public Blueprint(string line)
        {
            var pattern = @"Blueprint (?<blueprintNum>\d+): Each ore robot costs (?<oreRobotCost>\d+) ore. Each clay robot costs (?<clayRobotCost>\d+) ore. Each obsidian robot costs (?<obsidianRobotCostOre>\d+) ore and (?<obsidianRobotCostClay>\d+) clay. Each geode robot costs (?<geodeRobotCostOre>\d+) ore and (?<geodeRobotCostObsidian>\d+) obsidian.";
            var match = Regex.Match(line, pattern);

            Number = int.Parse(match.Groups["blueprintNum"].Value);
            OreRobotCost = new RobotCost(int.Parse(match.Groups["oreRobotCost"].Value), 0, 0);
            ClayRobotCost = new RobotCost(int.Parse(match.Groups["clayRobotCost"].Value), 0, 0);
            ObsidianRobotCost = new RobotCost(int.Parse(match.Groups["obsidianRobotCostOre"].Value), int.Parse(match.Groups["obsidianRobotCostClay"].Value), 0);
            GeodeRobotCost = new RobotCost(int.Parse(match.Groups["geodeRobotCostOre"].Value), 0, int.Parse(match.Groups["geodeRobotCostObsidian"].Value));
        }

        public int Number { get; set; }
        public RobotCost OreRobotCost { get; set; }
        public RobotCost ClayRobotCost { get; set; }
        public RobotCost ObsidianRobotCost { get; set; }
        public RobotCost GeodeRobotCost { get; set; }
        public int Qualitylevel { get; internal set; }

        public int GetTotalCost()
        {
            return OreRobotCost.OreCost
                + 3 * ClayRobotCost.OreCost
                + 2 * (ObsidianRobotCost.ClayCost + ObsidianRobotCost.OreCost)
                + GeodeRobotCost.OreCost + GeodeRobotCost.ObsidianCost;
        }
        public bool CanBuy(RobotState robotState)
        {
            return CanBuyClayRobot(robotState) || CanBuyObsidianRobot(robotState) || CanBuyOreRobot(robotState) || CanBuyGeodeRobot(robotState);
        }

        public bool CanBuyGeodeRobot(RobotState robotState)
        {
            return robotState.OreCount >= GeodeRobotCost.OreCost &&
                robotState.ObsidianCount >= GeodeRobotCost.ObsidianCost &&
                robotState.ClayCount >= GeodeRobotCost.ClayCost;
        }

        public bool CanBuyOreRobot(RobotState robotState)
        {
            return robotState.OreCount >= OreRobotCost.OreCost &&
                robotState.ObsidianCount >= OreRobotCost.ObsidianCost &&
                robotState.ClayCount >= OreRobotCost.ClayCost;
        }

        public bool CanBuyClayRobot(RobotState robotState)
        {
            return robotState.OreCount >= ClayRobotCost.OreCost &&
                 robotState.ObsidianCount >= ClayRobotCost.ObsidianCost &&
                 robotState.ClayCount >= ClayRobotCost.ClayCost;
        }

        public bool CanBuyObsidianRobot(RobotState robotState)
        {
            return robotState.OreCount >= ObsidianRobotCost.OreCost &&
                robotState.ObsidianCount >= ObsidianRobotCost.ObsidianCost &&
                robotState.ClayCount >= ObsidianRobotCost.ClayCost;
        }      
    }
}
