using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Monkey
    {

        public Monkey(int index)
        {
            Index = index;
        }
        public int Index { get; private set; }
        public Queue<int> Items { get; internal set; } = new Queue<int>();
        public string Operation { get; internal set; }
        public int TestValue { get; internal set; }
        public int DestIfFalse { get; internal set; }
        public int DestIfTrue { get; internal set; }
        public int InspectionCount { get; internal set; }

        public override string ToString()
        {
            return $"Monkey {Index}: Count {InspectionCount}";
        }
    }
}
