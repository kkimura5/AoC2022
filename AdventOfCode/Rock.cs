using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Rock
    {
        public List<RockRow> RockShape { get; private set; }
        public Rock(string rock)
        {
            switch (rock)
            {
                case "-":
                    RockShape = new List<RockRow>()
                    {
                        new RockRow(0,0),
                        new RockRow(0,0),
                        new RockRow(0,0),
                        new RockRow(4,0),
                    };
                    break;

                case "+":
                    RockShape = new List<RockRow>()
                    {
                        new RockRow(0,0),
                        new RockRow(1,1),
                        new RockRow(3,0),
                        new RockRow(1,1),
                    };
                    break;

                case "L":
                    RockShape = new List<RockRow>()
                    {
                        new RockRow(0,0),
                        new RockRow(1,2),
                        new RockRow(1,2),
                        new RockRow(3,0),
                    };
                    break;

                case "|":
                    RockShape = new List<RockRow>()
                    {
                        new RockRow(1,0),
                        new RockRow(1,0),
                        new RockRow(1,0),
                        new RockRow(1,0),
                    };
                    break;

                case "o":
                    RockShape = new List<RockRow>()
                    {
                        new RockRow(0,0),
                        new RockRow(0,0),
                        new RockRow(2,0),
                        new RockRow(2,0),
                    };
                    break;
            }
        }

        public int DropHeight { get; private set; }
        public int SidePosition { get; private set; }

        public void MoveRight(Tower tower)
        {
            if (tower.CanMoveRight(this)) 
            {
                SidePosition++;
            }
            else
            {
                //do nothing;
            }

        }

        public void MoveLeft(Tower tower)
        {
            if (tower.CanMoveLeft(this)) 
            {
                SidePosition--;
            }
            else
            {
                //do nothing;
            }
        }

        public void Drop(Tower tower)
        {
            if (tower.CanMoveDown(this))
            {
                DropHeight--;
            }
            else
            {
                tower.AddRock(this);
                IsDropComplete = true;
            }
        }

        public bool IsDropComplete { get; private set; }

        public void StartDrop(int startHeight)
        {
            DropHeight = startHeight;
            SidePosition = 2;
            IsDropComplete = false;
        }
    }
}