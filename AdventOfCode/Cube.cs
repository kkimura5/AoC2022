using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Cube
    {
        public Cube(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Cube(string cubeText)
        {
            var nums = cubeText.Split(',').Select(x => int.Parse(x)).ToList();
            X = nums[0];
            Y = nums[1];
            Z = nums[2];
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int CoveredSides { get; set; }

        public Cube Offset(int dx, int dy, int dz)
        {
            return new Cube(X + dx, Y + dy, Z + dz);
        }

        public bool IsCoincident(Cube cube)
        {
            return X == cube.X && Y == cube.Y && Z == cube.Z;
        }

        public bool IsAdjacent(Cube cube)
        {
            var adjacentX = IsAdjacentX(cube);
            var adjacentY = IsAdjacentY(cube);
            var adjacentZ = IsAdjacentZ(cube);
            return adjacentX || adjacentY || adjacentZ;
        }

        public bool IsAdjacentZ(Cube cube)
        {
            return Math.Abs(cube.Z - Z) == 1 && cube.X == X && cube.Y == Y;
        }

        public bool IsAdjacentY(Cube cube)
        {
            return Math.Abs(cube.Y - Y) == 1 && cube.X == X && cube.Z == Z;
        }

        public bool IsAdjacentX(Cube cube)
        {
            return Math.Abs(cube.X - X) == 1 && cube.Y == Y && cube.Z == Z;
        }

        public override string ToString()
        {
            return $"{X},{Y},{Z}";
        }        
    }
}
