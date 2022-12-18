using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Valve
    {
        public Valve(string name, int flowRate)
        {
            Name = name;
            FlowRate = flowRate;
        }
        public void Reset()
        {
            IsOpen = false;
            OpenTime = 0;
        }

        public void Open(int minute)
        {
            IsOpen = true;
            OpenTime = minute;
        }

        public int GetScore(int totalMinutes)
        {
            return Math.Max(0, (totalMinutes - OpenTime) * FlowRate);
        }

        public List<Valve> LinkedValves { get; set; } = new List<Valve>();
        public bool IsOpen { get; set; }

        public int OpenTime { get; private set; }

        public string Name { get; private set; }
        public int FlowRate { get; private set; }

        public override string ToString()
        {
            return $"Valve {Name}; Flow {FlowRate}; Opened {OpenTime}; Linked to {string.Join(", ", LinkedValves.Select(x => x.Name))}";
        }
    }
}
