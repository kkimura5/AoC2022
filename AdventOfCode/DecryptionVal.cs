using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
  public  class DecryptionVal
    {
        public DecryptionVal(long value)
        {
            Value = value;
        }

        public long Value { get; private set; }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
