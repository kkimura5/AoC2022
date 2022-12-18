using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Tower
    {
        private int towerWidth;
        public Tower(int width)
        {
            this.towerWidth = width;
            Heights = Enumerable.Repeat(0, width).ToList();
        }

        public List<TowerRow> TowerRows { get; private set; } = new List<TowerRow>();
        public List<int> Heights { get; private set; }

        internal void AddRock(Rock rock)
        {
            for (int i = rock.RockShape.Count-1; i >=0; i--)
            {
                var rowHeight = rock.DropHeight + rock.RockShape.Count - 1 - i;
                var rockRow = rock.RockShape[i];
                var towerRow = TowerRows.SingleOrDefault(x => x.Height == rowHeight);
                if (towerRow is null)
                {
                    towerRow = new TowerRow(towerWidth, rowHeight);
                    TowerRows.Add(towerRow);
                }
                
                foreach (var column in Enumerable.Range(rock.SidePosition + rockRow.Offset, rockRow.Width))
                {
                    towerRow.FilledStates[column] = true;
                    Heights[column] = Math.Max(Heights[column], rowHeight + 1);
                }
            }

            ClearUnnecessaryRows();
        }

        private void ClearUnnecessaryRows()
        {
            foreach (var towerRow in TowerRows.OrderByDescending(x => x.Height).ToList())
            {
                if (towerRow.FilledStates.Count(x => x) == towerRow.FilledStates.Count)
                {
                    TowerRows = TowerRows.Where(x => x.Height >= towerRow.Height).ToList();
                    return;
                }
                else if (towerRow.FilledStates.Count(x => x) == towerRow.FilledStates.Count - 1)
                {
                    var openIndices = towerRow.FilledStates.Where(x => !x).Select((x, i) => i).ToList();
                    if(TowerRows.Any(x => x.Height > towerRow.Height && openIndices.All(i => x.FilledStates[i])))
                    {
                        TowerRows = TowerRows.Where(x => x.Height >= towerRow.Height).ToList();
                        return;
                    }
                }
            }

            TowerRows = TowerRows.OrderByDescending(x => x.Height).Take(100).ToList();
        }

        internal bool CanMoveRight(Rock rock)
        {
            for (int i = 0; i < rock.RockShape.Count; i++)
            {
                var rowHeight = rock.DropHeight + rock.RockShape.Count - 1 - i;
                var rockRow = rock.RockShape[i];
                var towerRow = TowerRows.SingleOrDefault(x => x.Height == rowHeight);

                if (rockRow.Width == 0)
                {
                    continue;
                }

                if ((rock.SidePosition + rockRow.Offset + rockRow.Width) >= towerWidth)
                {
                    return false;
                }
                else if (towerRow != null)
                {
                    if (towerRow.FilledStates[rock.SidePosition + rockRow.Offset + rockRow.Width])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        internal bool CanMoveLeft(Rock rock)
        {
            for (int i = 0; i < rock.RockShape.Count; i++)
            {
                var rowHeight = rock.DropHeight + rock.RockShape.Count - 1 - i;
                var rockRow = rock.RockShape[i];
                var towerRow = TowerRows.SingleOrDefault(x => x.Height == rowHeight);

                if (rockRow.Width == 0)
                {
                    continue;
                }

                if ((rock.SidePosition + rockRow.Offset) == 0)
                {
                    return false;
                }
                else if (towerRow != null)
                {
                    if (towerRow.FilledStates[rock.SidePosition + rockRow.Offset - 1])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        internal bool CanMoveDown(Rock rock)
        {
            if (rock.DropHeight == 0)
            {
                return false;
            }

            for (int i = rock.RockShape.Count - 1; i >= 0; i--)
            {
                var rowHeight = rock.DropHeight + rock.RockShape.Count - 1 - i;
                var rockRow = rock.RockShape[i];
                var towerRow = TowerRows.SingleOrDefault(x => x.Height == rowHeight - 1);

                if (towerRow is null || rockRow.Width == 0)
                {
                    continue;
                }

                if (Enumerable.Range(rock.SidePosition + rockRow.Offset, rockRow.Width).Any(x => towerRow.FilledStates[x]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
