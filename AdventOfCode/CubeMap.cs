using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class CubeMap
    {
        private List<string> lines;

        public CubeMap(List<string> lines, int sidelength)
        {
            this.lines = lines;
            //var index1 = lines.First().IndexOf('.');
            //var index2 = lines.Skip(sidelength).First().IndexOf('.');
            //var index3 = lines.Skip(2 * sidelength).First().IndexOf('.');


            var index1 = lines.First().IndexOf('.');
            var index2 = lines.Skip(sidelength).First().IndexOf('.');
            var index3 = lines.Skip(2 * sidelength).First().IndexOf('.');
            var index4 = lines.Skip(3 * sidelength).First().IndexOf('.');

            //Sides.Add(new CubeSide(SideName.Top, lines.Take(sidelength).Select(x => x.Substring(index1, sidelength)).ToList(), 
            //    new Point(index1, 0), 0));
            //Sides.Add(new CubeSide(SideName.Back, lines.Skip(sidelength).Take(sidelength).Select(x => x.Substring(index2, sidelength)).ToList(), 
            //    new Point(index2, sidelength), 0));
            //Sides.Add(new CubeSide(SideName.Left, lines.Skip(sidelength).Take(sidelength).Select(x => x.Substring(index2 + sidelength, sidelength)).ToList(), 
            //    new Point(index2 + sidelength, sidelength), 0));
            //Sides.Add(new CubeSide(SideName.Front, lines.Skip(sidelength).Take(sidelength).Select(x => x.Substring(index2 + 2 * sidelength, sidelength)).ToList(), 
            //    new Point(index2 + 2 * sidelength, sidelength), 0));
            //Sides.Add(new CubeSide(SideName.Bottom, lines.Skip(2 * sidelength).Take(sidelength).Select(x => x.Substring(index3, sidelength)).ToList(), 
            //    new Point(index3, 2 * sidelength), 0));
            //Sides.Add(new CubeSide(SideName.Right, lines.Skip(2 * sidelength).Take(sidelength).Select(x => x.Substring(index3 + sidelength, sidelength)).ToList(), 
            //    new Point(index3 + sidelength, 2*sidelength), -90));

            Sides.Add(new CubeSide(SideName.Top, lines.Take(sidelength).Select(x => x.Substring(index1, sidelength)).ToList(), new Point(index1, 0), 0));
            Sides.Add(new CubeSide(SideName.Right, lines.Take(sidelength).Select(x => x.Substring(index1 + sidelength, sidelength)).ToList(), new Point(index1 + sidelength, 0), 90));
            Sides.Add(new CubeSide(SideName.Front, lines.Skip(sidelength).Take(sidelength).Select(x => x.Substring(index2, sidelength)).ToList(), new Point(index2, sidelength), 0));
            Sides.Add(new CubeSide(SideName.Left, lines.Skip(2 * sidelength).Take(sidelength).Select(x => x.Substring(index3, sidelength)).ToList(), new Point(index3, 2 * sidelength), 90));
            Sides.Add(new CubeSide(SideName.Bottom, lines.Skip(2 * sidelength).Take(sidelength).Select(x => x.Substring(index3 + sidelength, sidelength)).ToList(), new Point(index3 + sidelength, 2 * sidelength), 0));
            Sides.Add(new CubeSide(SideName.Back, lines.Skip(3 * sidelength).Take(sidelength).Select(x => x.Substring(index4, sidelength)).ToList(), new Point(index4, 3 * sidelength), 90));
        }

        public List<CubeSide> Sides { get; private set; } = new List<CubeSide>();

        public CubeSide GetNextCubeSide(SideName cubeSide, Heading heading)
        {
            switch (cubeSide)
            {
                case SideName.Top:
                    switch (heading)
                    {
                        case Heading.Right:
                            return Sides.Single(x => x.SideName == SideName.Right);
                        case Heading.Down:
                            return Sides.Single(x => x.SideName == SideName.Front);
                        case Heading.Left:
                            return Sides.Single(x => x.SideName == SideName.Left);
                        case Heading.Up:
                            return Sides.Single(x => x.SideName == SideName.Back);
                    }

                    break;

                case SideName.Bottom:
                    switch (heading)
                    {
                        case Heading.Right:
                            return Sides.Single(x => x.SideName == SideName.Right);
                        case Heading.Down:
                            return Sides.Single(x => x.SideName == SideName.Back);
                        case Heading.Left:
                            return Sides.Single(x => x.SideName == SideName.Left);
                        case Heading.Up:
                            return Sides.Single(x => x.SideName == SideName.Front);
                    }

                    break;
                case SideName.Left:
                    switch (heading)
                    {
                        case Heading.Right:
                            return Sides.Single(x => x.SideName == SideName.Front);
                        case Heading.Down:
                            return Sides.Single(x => x.SideName == SideName.Bottom);
                        case Heading.Left:
                            return Sides.Single(x => x.SideName == SideName.Back);
                        case Heading.Up:
                            return Sides.Single(x => x.SideName == SideName.Top);
                    }

                    break;
                case SideName.Right:
                    switch (heading)
                    {
                        case Heading.Right:
                            return Sides.Single(x => x.SideName == SideName.Back);
                        case Heading.Down:
                            return Sides.Single(x => x.SideName == SideName.Bottom);
                        case Heading.Left:
                            return Sides.Single(x => x.SideName == SideName.Front);
                        case Heading.Up:
                            return Sides.Single(x => x.SideName == SideName.Top);
                    }

                    break;
                case SideName.Back:
                    switch (heading)
                    {
                        case Heading.Right:
                            return Sides.Single(x => x.SideName == SideName.Left);
                        case Heading.Down:
                            return Sides.Single(x => x.SideName == SideName.Bottom);
                        case Heading.Left:
                            return Sides.Single(x => x.SideName == SideName.Right);
                        case Heading.Up:
                            return Sides.Single(x => x.SideName == SideName.Top);
                    }

                    break;
                case SideName.Front:
                    switch (heading)
                    {
                        case Heading.Right:
                            return Sides.Single(x => x.SideName == SideName.Right);
                        case Heading.Down:
                            return Sides.Single(x => x.SideName == SideName.Bottom);
                        case Heading.Left:
                            return Sides.Single(x => x.SideName == SideName.Left);
                        case Heading.Up:
                            return Sides.Single(x => x.SideName == SideName.Top);
                    }

                    break;

            }

            throw new Exception();
        }

        public Point GetNextPoint(SideName currentSide, Point currentPosition, Heading heading)
        {
            var nextCubeSide = GetNextCubeSide(currentSide, heading);

            int endIndex = nextCubeSide.Lines.Count - 1;
            switch (heading)
            {
                case Heading.Right:
                    switch (currentSide)
                    {
                        case SideName.Right:
                        case SideName.Left:
                        case SideName.Front:
                        case SideName.Back:
                            return new Point(0, currentPosition.Y);

                        case SideName.Top:
                            return new Point(endIndex - currentPosition.Y, 0);

                        case SideName.Bottom:
                            return new Point(currentPosition.Y, endIndex);
                    }
                    break;

                case Heading.Left:
                    switch (currentSide)
                    {
                        case SideName.Right:
                        case SideName.Left:
                        case SideName.Front:
                        case SideName.Back:
                            return new Point(endIndex, currentPosition.Y);

                        case SideName.Top:
                            return new Point(currentPosition.Y, 0);

                        case SideName.Bottom:
                            return new Point(endIndex - currentPosition.Y, endIndex);
                    }
                    break;


                case Heading.Down:
                    switch (currentSide)
                    {
                        case SideName.Right:
                            return new Point(endIndex, currentPosition.X);

                        case SideName.Left:
                            return new Point(0, endIndex - currentPosition.X);
                        case SideName.Front:
                            return new Point(currentPosition.X, 0);
                        case SideName.Back:
                            return new Point(endIndex - currentPosition.X, endIndex);

                        case SideName.Top:
                            return new Point(currentPosition.X, 0);

                        case SideName.Bottom:
                            return new Point(endIndex - currentPosition.X, endIndex);
                    }
                    break;
                case Heading.Up:
                    switch (currentSide)
                    {
                        case SideName.Right:
                            return new Point(endIndex, endIndex - currentPosition.X);
                        case SideName.Left:
                            return new Point(0, currentPosition.X);

                        case SideName.Front:
                            return new Point(currentPosition.X, endIndex);
                        case SideName.Back:
                            return new Point(endIndex - currentPosition.X, 0);

                        case SideName.Top:
                            return new Point(endIndex - currentPosition.X, 0);
                        case SideName.Bottom:
                            return new Point(currentPosition.X, endIndex);
                    }
                    break;
            }

            throw new Exception();
        }

        public Heading GetNextHeading(SideName currentSide, Heading heading)
        {
            if (heading == Heading.Left || heading == Heading.Right)
            {
                switch (currentSide)
                {
                    case SideName.Front:
                    case SideName.Back:
                    case SideName.Left:
                    case SideName.Right:
                        return heading;

                    case SideName.Top:
                        return Heading.Down;
                    case SideName.Bottom:
                        return Heading.Up;
                }
            }
            else
            {
                switch (currentSide)
                {
                    case SideName.Front:
                        return heading;

                    case SideName.Back:
                        if (heading == Heading.Up)
                        {
                            return Heading.Down;
                        }
                        else
                        {
                            return Heading.Up;
                        }

                    case SideName.Left:
                        return Heading.Right;
                        

                    case SideName.Right:
                        return Heading.Left;
                        
                    case SideName.Top:
                        return Heading.Down;
                    case SideName.Bottom:
                        return Heading.Up;

                }
            }

            throw new Exception();
        }
    }
}
