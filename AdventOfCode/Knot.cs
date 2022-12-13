using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Knot
    {
        public Knot()
        {
        }

        public Knot(Knot parentKnot)
        {
            parentKnot.FollowingKnot = this;
        }

        public Point Position { get; set; } = new Point(0, 0);
        public Knot FollowingKnot { get; set; }

        public void Move(int x, int y)
        {
            Position = new Point(Position.X + x, Position.Y + y);
            FollowingKnot?.ReactToNewPosition(Position);
        }

        public void ReactToNewPosition(Point headPosition)
        {
            if (headPosition.X == Position.X &&
                        headPosition.Y != Position.Y)
            {
                var diff = headPosition.Y - Position.Y;
                if (Math.Abs(diff) == 2)
                {
                    Move(0, Math.Sign(diff) * 1);
                }
            }
            else if (headPosition.X != Position.X &&
                headPosition.Y == Position.Y)
            {
                var diff = headPosition.X - Position.X;
                if (Math.Abs(diff) == 2)
                {
                    Move(Math.Sign(diff) * 1, 0);
                }
            }
            else if (headPosition.X != Position.X &&
                headPosition.Y != Position.Y)
            {
                var diffX = headPosition.X - Position.X;
                var diffY = headPosition.Y - Position.Y;
                if (Math.Abs(diffX) == 2 || Math.Abs(diffY) == 2)
                {
                    Move(Math.Sign(diffX) * 1,Math.Sign(diffY) * 1);
                }
            }
        }
    }
}
