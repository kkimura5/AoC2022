using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class DoublePath
    {
        public DoublePath(List<Valve> list)
        {
            YourPath = list.ToList();
            ElephantPath = list.ToList();
        }

        public DoublePath(List<Valve> yourPath, List<Valve> elephantPath)
        {
            YourPath = yourPath;
            ElephantPath = elephantPath;
        }

        public List<Valve> YourPath { get; }
        public List<Valve> ElephantPath { get; }
        public List<Valve> AllValves => YourPath.Concat(ElephantPath).ToList();

        internal DoublePath CreateNext(Valve yourNext, Valve elephantNext)
        {
            return new DoublePath(YourPath.Concat(new List<Valve>() { yourNext }).ToList(),
                ElephantPath.Concat(new List<Valve>() { elephantNext }).ToList());
        }
    }
}
