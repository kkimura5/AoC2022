using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class MonkeyItem
    {
        public MonkeyItem(int worryLevel, Monkey monkey)
        {
            StartingWorryLevel = worryLevel;
            CurrentMonkey = monkey;
        }

        public int StartingWorryLevel { get; set; }
        public Dictionary<Monkey, int> ModValueByMonkey { get; } = new Dictionary<Monkey, int>();
        public Monkey CurrentMonkey { get; set; }
    }
}
