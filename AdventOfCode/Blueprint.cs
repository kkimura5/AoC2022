﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Blueprint
    {
        private int totalMinutes;
        public Blueprint(string line, int totalMinutes)
        {
            this.totalMinutes = totalMinutes;
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

        public List<string> GetMetalPreferences(RobotState robotState)
        {
            var clayNeeded = ObsidianRobotCost.ClayCost;
            var obsidianNeeded = GeodeRobotCost.ObsidianCost;
            var preferences = new List<string>() { "geode" };

            if (OreRobotCost.OreCost < ClayRobotCost.OreCost && robotState.OreRobotCount == 1)
            {
                // if ore costs less, get ore early on. Don't bother later.
                preferences.Add("ore");
            }
            else if (robotState.ClayRobotCount == 0)
            {
                preferences.Add("clay");
            }
            else if (robotState.ClayRobotCount > 0 && robotState.ObsidianRobotCount == 0)
            {
                // prioritize getting 1 obsidian robot as soon as enough clay
                preferences.Add("obsidian");
                if (CanBuyClayRobot(robotState))
                {
                    // only buy clay if better for getting obsidian
                    var numTurnsForObsidian = (int)Math.Ceiling((double)(ObsidianRobotCost.ClayCost - robotState.ClayCount) / robotState.ClayRobotCount);
                    var numTurnsIfClayRobotBought = (int)Math.Ceiling((double)(ObsidianRobotCost.ClayCost - robotState.ClayCount - robotState.ClayRobotCount) / (robotState.ClayRobotCount + 1)) + 1;
                    numTurnsIfClayRobotBought = Math.Max(numTurnsIfClayRobotBought, (int)Math.Ceiling((double)(ObsidianRobotCost.OreCost - robotState.OreCount + ClayRobotCost.OreCost) / robotState.OreRobotCount));
                    if (numTurnsForObsidian >= numTurnsIfClayRobotBought)
                    {
                        preferences.Insert(preferences.IndexOf("obsidian"), "clay");
                    }
                }
            }
            else
            {
                var minutesRemaining = totalMinutes - robotState.CurrentMinute;

                var numTurnsForGeodeBasedOnOre = (int)Math.Ceiling((double)(GeodeRobotCost.OreCost - robotState.OreCount + ObsidianRobotCost.OreCost) / robotState.OreRobotCount);
                var numTurnsForGeode = (int)Math.Ceiling((double)(GeodeRobotCost.ObsidianCost - robotState.ObsidianCount) / robotState.ObsidianRobotCount);
                var numTurnsIfObsidianRobotBought = (int)Math.Ceiling((double)(GeodeRobotCost.ObsidianCost - robotState.ObsidianCount - robotState.ObsidianRobotCount) / (robotState.ObsidianRobotCount + 1)) + 1;
                numTurnsIfObsidianRobotBought = Math.Max(numTurnsIfObsidianRobotBought, numTurnsForGeodeBasedOnOre);

                var remaining = (GeodeRobotCost.ObsidianCost - robotState.ObsidianCount) % robotState.ObsidianRobotCount;
                var nextGeodeTurns = (int)Math.Ceiling((double)(GeodeRobotCost.ObsidianCost - remaining) / robotState.ObsidianRobotCount);
                var remaining2 = (GeodeRobotCost.ObsidianCost - robotState.ObsidianCount + robotState.ObsidianRobotCount) % (robotState.ObsidianRobotCount + 1);
                var nextGeodeTurnsIfObsidianBought = (int)Math.Ceiling((double)(GeodeRobotCost.ObsidianCost - remaining2) / (robotState.ObsidianRobotCount + 1));
                nextGeodeTurnsIfObsidianBought = Math.Max(nextGeodeTurnsIfObsidianBought, numTurnsForGeodeBasedOnOre);

                var score1 = (minutesRemaining - numTurnsForGeode) + (minutesRemaining - numTurnsForGeode - nextGeodeTurns);
                var score2 = (minutesRemaining - numTurnsIfObsidianRobotBought) + (minutesRemaining - numTurnsIfObsidianRobotBought - nextGeodeTurnsIfObsidianBought);
                if (score2 > score1)
                {
                    if (CanBuyObsidianRobot(robotState))
                    {
                        preferences.Add("obsidian");
                    }
                    else if (CanBuyClayRobot(robotState))
                    {
                        var numTurnsForObsidian = (int)Math.Ceiling((double)(ObsidianRobotCost.ClayCost - robotState.ClayCount) / robotState.ClayRobotCount);
                        var numTurnsIfClayRobotBought = (int)Math.Ceiling((double)(ObsidianRobotCost.ClayCost - robotState.ClayCount - robotState.ClayRobotCount) / (robotState.ClayRobotCount + 1)) + 1;
                        numTurnsIfClayRobotBought = Math.Max(numTurnsIfClayRobotBought, (int)Math.Ceiling((double)(ObsidianRobotCost.OreCost - robotState.OreCount) / robotState.OreRobotCount));
                        if (numTurnsForObsidian >= numTurnsIfClayRobotBought)
                        {
                            preferences.Add("clay");
                        }
                    }
                }
            }

            return preferences;
        }
    }
}
