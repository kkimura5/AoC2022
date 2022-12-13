using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class PacketItem 
    {
        private string line;
        public PacketItem(string line)
        {
            this.line = line;
            IsList = line.StartsWith("[") && line.EndsWith("]");
            IsInt = !IsList;

            if (IsList)
            {
                ProcessList(line);
            }
        }

        private void ProcessList(string line)
        {
            string innerText = line.Substring(1, line.Length - 2);
            var matches = Regex.Matches(innerText, ",");
            var previousMatchIndex = 0;
            foreach (Match match in matches)
            {
                var itemText = innerText.Substring(previousMatchIndex, match.Index - previousMatchIndex);
                if (itemText.Count(x => x == '[') == itemText.Count(x => x == ']'))
                {
                    Items.Add(new PacketItem(itemText));
                    previousMatchIndex = match.Index + 1;
                }

                if (!match.NextMatch().Success)
                {
                    itemText = innerText.Substring(match.Index + 1);
                    if (itemText.Count(x => x == '[') == itemText.Count(x => x == ']'))
                    {
                        Items.Add(new PacketItem(itemText));
                    }
                }
            }

            if (Items.Count == 0 && !string.IsNullOrEmpty(innerText))
            {
                Items.Add(new PacketItem(innerText));
            }
        }

        public List<PacketItem> Items { get; set; } = new List<PacketItem>();

        public bool IsList { get; set; }
        public bool IsInt { get; set; }

        public int IntValue => int.Parse(line);


        public void ConvertToList()
        {
            if (!IsList)
            {
                line = $"[{line}]";
                IsList = true;
                IsInt = false;
                ProcessList(line);
            }
        }
        public bool? IsInCorrectOrder(PacketItem other)
        {
            int i = 0;
            for (; i < Math.Min(Items.Count, other.Items.Count); i++)
            {
                var thisItem = Items[i];
                var thatItem = other.Items[i];
                if (thisItem.IsInt && thatItem.IsInt && thisItem.IntValue != thatItem.IntValue)
                {
                    return thisItem.IntValue < thatItem.IntValue;
                }
                else if (thisItem.IsList || thatItem.IsList)
                {
                    if (!thisItem.IsList)
                    {
                        thisItem.ConvertToList();
                    }
                    else if (!thatItem.IsList)
                    {
                        thatItem.ConvertToList();
                    }

                    bool? innerCompare = thisItem.IsInCorrectOrder(thatItem);
                    if (innerCompare.HasValue)
                    {
                        return innerCompare.Value;
                    }
                }
            }

            if (Items.Count != other.Items.Count)
            {
                return Items.Count < other.Items.Count;
            }

            return null;
        }
        public override string ToString()
        {
            if (IsInt)
            {
                return $"Int: {IntValue}; {line}";
            }
            else
            {
                return $"List: Item count {Items.Count}; {line}";
            }
        }        
    }
}
