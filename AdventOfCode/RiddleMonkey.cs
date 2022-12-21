using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class RiddleMonkey
    {
        public RiddleMonkey(string line)
        {
            var pattern = @"(?<name>\w+): (?<arg1>\w{4}) (?<op>[\+\-\*/]) (?<arg2>\w{4})";
            var pattern2 = @"(?<name>\w+): (?<ans>\d+)";

            var match1 = Regex.Match(line,pattern);
            var match2 = Regex.Match(line,pattern2);

            if (match1.Success)
            {
                Name = match1.Groups["name"].Value;
                Operation = match1.Groups["op"].Value.Single();
                OtherMonkeyNames.Add(match1.Groups["arg1"].Value);
                OtherMonkeyNames.Add(match1.Groups["arg2"].Value);
            }
            else if (match2.Success)
            {
                Name = match2.Groups["name"].Value;
                ImmediateValue = long.Parse(match2.Groups["ans"].Value);
            }
        }   

        public char Operation { get; set; }
        public long ImmediateValue { get; set; }
        public long Value => OtherMonkeys.Any() ? CalculateValue() : ImmediateValue;
        public List<string> OtherMonkeyNames { get; set; } = new List<string>();
        private long CalculateValue()
        {
            switch (Operation)
            {
                case '+':
                    return OtherMonkeys[0].Value + OtherMonkeys[1].Value;

                case '-':
                    return OtherMonkeys[0].Value - OtherMonkeys[1].Value;

                case '*':
                    return OtherMonkeys[0].Value * OtherMonkeys[1].Value;

                case '/':
                    return OtherMonkeys[0].Value / OtherMonkeys[1].Value;

                default:
                    throw new Exception();
            }
        }

        public string Name { get; set; }
        public int Type { get; set; }
        public List<RiddleMonkey> OtherMonkeys { get; set; } = new List<RiddleMonkey>();
    }
}