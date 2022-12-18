using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class TowerRow
    {
        public TowerRow(int width, long height)
        {
            FilledStates = Enumerable.Repeat(false, width).ToList();
            Height = height;
        }
        public long Height { get; private set; }
        public List<bool> FilledStates { get; private set; }

        public override string ToString()
        {
            return $"{Height}: {string.Join(string.Empty, FilledStates.Select(x => x ? "#" : "."))}";
        }
    }
}