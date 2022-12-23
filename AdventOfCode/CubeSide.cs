using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode
{
    public class CubeSide
    {
        public CubeSide(SideName sideName, List<string> lines, Point point, int rotation)
        {
            SideName = sideName;
            CornerPoint = point;
            Rotation = rotation;

            if (rotation == 0)
            {
                Lines = lines;
            }
            else if (rotation == 90)
            {
                Lines = new List<string>();
                
                for (int i = 0; i < lines.First().Length; i++)
                {
                    Lines.Add(string.Join(string.Empty, lines.Select(x => x[i]).Reverse()));
                }
            }
            else if (rotation == -90)
            {
                Lines = new List<string>();

                for (int i = lines.First().Length-1; i >=0 ; i--)
                {
                    Lines.Add(string.Join(string.Empty, lines.Select(x => x[i])));
                }
            }
        }

        public override string ToString()
        {
            return $"{SideName}: Corner: {CornerPoint}, Rotation {Rotation}";
        }

        public SideName SideName { get; set; }
        public List<string> Lines { get; set; }
        public Point CornerPoint { get; private set; }
        public int Rotation { get; private set; }

        public Heading RotateBaseHeading(Heading heading)
        {
            var headings = new List<Heading>() { Heading.Right, Heading.Down, Heading.Left, Heading.Up };
            return headings[(headings.IndexOf(heading) + Rotation / 90) % 4];
        }
    }
}