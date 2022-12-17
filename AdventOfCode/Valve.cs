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
            timeOpened = 0;
        }

        public void Open(int minute)
        {
            IsOpen = true;
            this.timeOpened = minute;
        }

        public int GetScore(int totalMinutes)
        {
            return Math.Max(0, (totalMinutes - timeOpened) * FlowRate);
        }

        public List<Valve> LinkedValves { get; set; } = new List<Valve>();
        public bool IsOpen { get; set; }

        private int timeOpened;

        public string Name { get; private set; }
        public int FlowRate { get; private set; }

        public override string ToString()
        {
            return $"Valve {Name}; Flow {FlowRate}; Opened {timeOpened}; Linked to {string.Join(", ", LinkedValves.Select(x => x.Name))}";
        }
    }
}
