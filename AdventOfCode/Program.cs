using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;

namespace AdventOfCode
{
    class Program
    {
        private static Dictionary<string, int> distanceCache = new Dictionary<string, int>();
        private static Dictionary<string, bool> wayOutCache = new Dictionary<string, bool>();
        static void Main(string[] args)
        {
            //RunDay1();
            //RunDay2();
            //RunDay3();
            //RunDay4();
            //RunDay5();
            //RunDay6();
            //RunDay7();
            //RunDay8();
            //RunDay9();
            //RunDay10();
            //RunDay11();
            //RunDay12();
            //RunDay13();
            //RunDay14();
            //RunDay15();
            //RunDay16();
            //RunDay17();
            //RunDay18();
            //RunDay19();
            //RunDay20();
            //RunDay21();
            //RunDay22();
            //RunDay23();
            RunDay24();
            Console.ReadKey();
        }

        private static void RunDay24()
        {
            var map = File.ReadAllLines(".\\Input\\Day24.txt").ToList();

            var blizzardMap = InitializeBlizzardMap(map);
            var startingPosition = new Point(1, 0);
            var endPosition = new Point(map.Last().IndexOf('.'), map.Count - 1);
            var total = GetMinTotalMoves(map, startingPosition, endPosition, ref blizzardMap);
            Console.WriteLine($"Day 24 part 1: {total}");

            total += GetMinTotalMoves(map, endPosition, startingPosition, ref blizzardMap);
            total += GetMinTotalMoves(map, startingPosition, endPosition, ref blizzardMap);
            Console.WriteLine($"Day 24 part 2: {total}");
        }

        private static int GetMinTotalMoves(List<string> map, Point startingPosition, Point endPosition, ref List<Heading>[,] blizzardMap)
        {
            var paths = new List<List<Point>> { new List<Point>() { startingPosition } };
            while (!paths.Any(x => x.Last() == endPosition))
            {
                //PrintMap(blizzardMap, map.First().Length, map.Count);
                var nextMap = IterateBlizzards(blizzardMap, map.First().Length, map.Count);
                var nextPaths = new List<List<Point>>();
                foreach (var path in paths.ToList())
                {
                    var currentPosition = path.Last();
                    var possibleNextMoves = new List<Point>()
                    {
                        new Point(currentPosition.X, currentPosition.Y),
                        new Point(currentPosition.X, currentPosition.Y+1),
                        new Point(currentPosition.X, currentPosition.Y-1),
                        new Point(currentPosition.X+1, currentPosition.Y),
                        new Point(currentPosition.X-1, currentPosition.Y),
                    };

                    var newPaths = possibleNextMoves.Where(x => IsInBounds(x, map))
                        .Where(x => x == startingPosition || x == endPosition || (nextMap[x.X, x.Y] != null && !nextMap[x.X, x.Y].Any()))
                        .Select(x => path.Concat(new List<Point>() { x }).ToList()).ToList();

                    nextPaths.AddRange(newPaths.Where(x => !nextPaths.Any(y => y.Last() == x.Last())));
                }

                blizzardMap = nextMap;
                paths = nextPaths;
            }

            return paths.Where(x => x.Last() == endPosition).Min(x => x.Count) - 1;
        }

        private static void PrintMap(List<Heading>[,] blizzardMap, int totalX, int totalY)
        {
            for (int y = 0; y < totalY ; y++)
            {
                for (int x = 0; x < totalX ; x++)
                {
                    if (y == 0 || x == 0 || x == totalX - 1 || y == totalY - 1)
                    {
                        Console.Write('#');
                    }
                    else if (!blizzardMap[x, y].Any())
                    {
                        Console.Write('.');
                    }
                    else if (blizzardMap[x, y].Count > 1)
                    {
                        Console.Write($"{blizzardMap[x, y].Count}".First());
                    }
                    else
                    {
                        switch(blizzardMap[x, y].Single())
                        {
                            case Heading.Down:
                                Console.Write('v');
                                break;
                            case Heading.Up:
                                Console.Write('^');
                                break;
                            case Heading.Left:
                                Console.Write('<');
                                break;
                            case Heading.Right:
                                Console.Write('>');
                                break;
                        }
                    }
                }

                Console.WriteLine();
            }
        }

        private static bool IsInBounds(Point point, List<string> map)
        {
            var isInXBounds = point.X > 0 && point.X < map.First().Length - 1; 
            var isInYBounds = point.Y > 0 && point.Y < map.Count - 1;
            var isAtStartPosition = point == new Point(map.First().IndexOf('.'), 0);
            var isAtEndPosition = point == new Point(map.Last().IndexOf('.'), map.Count - 1);

            return (isInXBounds && isInYBounds) || isAtEndPosition || isAtStartPosition;
        }

        private static List<Heading>[,] IterateBlizzards(List<Heading>[,] blizzardMap, int totalX, int totalY)
        {
            var newMap = new List<Heading>[totalX, totalY];
            for (int y = 1; y < totalY - 1; y++)
            {
                for (int x = 1; x < totalX - 1; x++)
                {
                    if (newMap[x, y] is null)
                    {
                        newMap[x, y] = new List<Heading>();
                    }

                    foreach(var blizzard in blizzardMap[x, y])
                    {
                        Point newPoint = new Point();
                        switch (blizzard)
                        {
                            case Heading.Right:
                                newPoint = new Point(x + 1, y);
                                if (newPoint.X > totalX - 2)
                                {
                                    newPoint = new Point(1, y);
                                }
                                break;

                            case Heading.Left:
                                newPoint = new Point(x - 1, y);
                                if (newPoint.X < 1)
                                {
                                    newPoint = new Point(totalX - 2, y);
                                }
                                break;

                            case Heading.Down:
                                newPoint = new Point(x, y + 1);
                                if (newPoint.Y > totalY - 2)
                                {
                                    newPoint = new Point(x, 1);
                                }
                                break;

                            case Heading.Up:
                                newPoint = new Point(x, y - 1);
                                if (newPoint.Y < 1)
                                {
                                    newPoint = new Point(x, totalY - 2);
                                }
                                break;
                        }

                        if (newMap[newPoint.X, newPoint.Y] is null)
                        {
                            newMap[newPoint.X, newPoint.Y] = new List<Heading>();
                        }

                        newMap[newPoint.X, newPoint.Y].Add(blizzard);
                    }
                }
            }

            return newMap;
        }

        private static List<Heading>[,] InitializeBlizzardMap(List<string> map)
        {
            var blizzardMap = new List<Heading>[map.First().Length, map.Count];

            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map.First().Length; x++)
                {
                    if (map[y][x] == '#')
                    {
                        continue;
                    }

                    blizzardMap[x, y] = new List<Heading>();
                    switch (map[y][x])
                    {
                        case '>':
                            blizzardMap[x, y].Add(Heading.Right);
                            break;
                        case '<':
                            blizzardMap[x, y].Add(Heading.Left);
                            break;

                        case 'v':
                            blizzardMap[x, y].Add(Heading.Down);
                            break;
                        case '^':
                            blizzardMap[x, y].Add(Heading.Up);
                            break;
                    }
                }
            }

            return blizzardMap;
        }

        private static void RunDay23()
        {
            var lines = File.ReadAllLines(".\\Input\\Day23.txt").ToList();
            var numRounds = 1000;

            var mapSize = lines.Count + 2 * numRounds;
            var map = new bool[mapSize, mapSize];
            var minX = mapSize;
            var maxX = 0;
            var minY = mapSize;
            var maxY = 0;

            for (int y = numRounds; y < lines.Count + numRounds; y++)
            {
                var line = lines[y - numRounds];
                for (int x = numRounds; x < line.Length + numRounds; x++)
                {
                    map[x, y] = line[x - numRounds] == '#';
                    if (map[x, y])
                    {
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }

            var directions = new List<Direction>
            {
                Direction.North,
                Direction.South,
                Direction.West,
                Direction.East,
            };

            int round = 0;
            while (true)
            {
                var proposedNewPoints = new Dictionary<Point, Point>();
                var blockedPoints = new List<Point>();
                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        if (map[x, y])
                        {
                            if (WillMove(map, x, y))
                            {
                                Point point = GetProposedPoint(map, x, y, directions, round);
                                if (proposedNewPoints.Values.Any(p => p == point))
                                {
                                    proposedNewPoints.Remove(proposedNewPoints.Single(kvp => kvp.Value == point).Key);
                                    blockedPoints.Add(point);
                                }

                                if (point != new Point(-1, -1) && !blockedPoints.Contains(point))
                                {
                                    proposedNewPoints.Add(new Point(x, y), point);
                                }
                            }

                        }
                    }
                }
                
                foreach (var newPoint in proposedNewPoints)
                {
                    map[newPoint.Key.X, newPoint.Key.Y] = false;
                    map[newPoint.Value.X, newPoint.Value.Y] = true;
                    minX = Math.Min(minX, newPoint.Value.X);
                    minY = Math.Min(minY, newPoint.Value.Y);
                    maxX = Math.Max(maxX, newPoint.Value.X);
                    maxY = Math.Max(maxY, newPoint.Value.Y);
                }

                round++;
                if (round == 10)
                {
                    int count = GetCount(mapSize, map);
                    Console.WriteLine($"Day 23 Part 1: {count}");
                }

                if (!proposedNewPoints.Any())
                {
                    break;
                }

                Console.WriteLine($"Round {round + 1}, {proposedNewPoints.Count} moved.");
            }

            Console.WriteLine($"Day 23 Part 2: {round}");
        }

        private static int GetCount(int mapSize, bool[,] map)
        {
            var minX = mapSize;
            var maxX = 0;
            var minY = mapSize;
            var maxY = 0;

            for (int y = 0; y < mapSize; y++)
            {
                for (int x = 0; x < mapSize; x++)
                {
                    if (map[x, y])
                    {
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }

            var count = 0;
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (!map[x, y])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static Point GetProposedPoint(bool[,] map, int x, int y, List<Direction> directions, int round)
        {
            for (int i = 0; i < directions.Count; i++)
            {
                var proposedDirection = directions[(i + round) % 4];

                switch(proposedDirection)
                {
                    case Direction.North:
                        if (!map[x - 1, y - 1] && !map[x, y - 1] && !map[x + 1, y - 1])
                        {
                            return new Point(x, y - 1);
                        }
                        break;

                    case Direction.South:
                        if (!map[x - 1, y + 1] && !map[x, y + 1] && !map[x + 1, y + 1])
                        {
                            return new Point(x, y + 1);
                        }
                        break;

                    case Direction.East:
                        if (!map[x + 1, y - 1] && !map[x + 1, y ] && !map[x + 1, y + 1])
                        {
                            return new Point(x + 1, y);
                        }
                        break;

                    case Direction.West:
                        if (!map[x - 1, y -1 ] && !map[x - 1, y] && !map[x - 1, y + 1])
                        {
                            return new Point(x - 1, y);
                        }
                        break;

                }

            }

            return new Point(-1, -1);
        }

        private static bool WillMove(bool[,] map, int x, int y)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i == x && j == y)
                    {
                        continue;
                    }

                    if (map[i, j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void RunDay22()
        {
            var lines = File.ReadAllLines(".\\Input\\Day22.txt").ToList();
            var map = lines.Take(lines.Count - 2).ToList();
            int sideLength = 50;
            //int sideLength = 4;
            var cubeMap = new CubeMap(map, sideLength);
            var directions = lines.Last();
            var currentPosition1 = new Point(lines.First().IndexOf('.'), 0);
            var currentPosition2 = new CubeLocation(new Point(0, 0), SideName.Top, Heading.Right);

            var heading = Heading.Right;
            var numMoves = 0;
            while (!string.IsNullOrEmpty(directions))
            {
                var pattern1 = "^[LR]";
                var pattern2 = @"^\d+";
                var dirMatch = Regex.Match(directions, pattern1);
                var moveMatch = Regex.Match(directions, pattern2);

                if (dirMatch.Success)
                {
                    heading = GetNewHeading(heading, dirMatch.Value);
                    currentPosition2.HeadingOnSide = GetNewHeading(currentPosition2.HeadingOnSide, dirMatch.Value);
                    directions = directions.Substring(1);
                }
                else if (moveMatch.Success)
                {
                    directions = directions.Substring(moveMatch.Value.Length);

                    var numSpacesToMove = int.Parse(moveMatch.Value);
                    string relevantLine = string.Empty;
                    int index = 0;
                    switch (heading)
                    {
                        case Heading.Right:
                        case Heading.Left:
                            relevantLine = map[currentPosition1.Y];
                            index = currentPosition1.X;
                            break;

                        case Heading.Down:
                        case Heading.Up:
                            relevantLine = string.Join(string.Empty, map.Select(row => row.Length > currentPosition1.X ? row[currentPosition1.X] : ' '));
                            index = currentPosition1.Y;
                            break;
                    }

                    var newPosition = Move(heading, numSpacesToMove, index, relevantLine);
                    if (heading == Heading.Down || heading == Heading.Up)
                    {
                        currentPosition1 = new Point(currentPosition1.X, newPosition);
                    }
                    else
                    {
                        currentPosition1 = new Point(newPosition, currentPosition1.Y);
                    }

                    if (map[currentPosition1.Y][currentPosition1.X] != '.')
                    {
                        throw new Exception();
                    }

                    var cubeSide = cubeMap.Sides.Single(x => x.SideName == currentPosition2.SideName);                    
                    currentPosition2 = MoveCubeSide(currentPosition2, numSpacesToMove, cubeMap);
                    numMoves++;
                }
            }

            var cubePosition = currentPosition2.Location;
            var finalCubeSide = cubeMap.Sides.Single(X => X.SideName == currentPosition2.SideName);
            var finalHeading = currentPosition2.HeadingOnSide;
            if (finalCubeSide.Rotation == 90)
            {
                cubePosition = new Point(cubePosition.Y, sideLength - cubePosition.X);
                finalHeading = GetNewHeading(finalHeading, "L");
            }
            else if (finalCubeSide.Rotation == -90)
            {
                cubePosition = new Point(sideLength - cubePosition.Y, cubePosition.X);
                finalHeading = GetNewHeading(finalHeading, "R");
            }

            cubePosition.Offset(finalCubeSide.CornerPoint.X, finalCubeSide.CornerPoint.Y);

            var total = (currentPosition1.X + 1) * 4 + (currentPosition1.Y + 1) * 1000 + Math.Abs((int)heading) - 1;
            var total2 = (cubePosition.X + 1) * 4 + (cubePosition.Y + 1) * 1000 + Math.Abs((int)finalHeading) - 1;
            Console.WriteLine($"Day 22 part 1: {total}");
            Console.WriteLine($"Day 22 part 2: {total2}");
        }

        private static CubeLocation MoveCubeSide(CubeLocation currentLocation, int numSpacesToMove, CubeMap cubeMap)
        {
            var currentCubeSide = cubeMap.Sides.Single(x => x.SideName == currentLocation.SideName);
            var currentPosition = currentLocation.Location;
            var heading = currentLocation.HeadingOnSide;
            var map = currentCubeSide.Lines;
            string relevantLine = string.Empty;
            int index = 0;

            Action<int> offsetAction = null;
            switch (heading)
            {
                case Heading.Right:
                case Heading.Left:
                    relevantLine = map[currentPosition.Y];
                    index = currentPosition.X;
                    offsetAction = dx => currentPosition = new Point(currentPosition.X + dx, currentPosition.Y);
                    break;

                case Heading.Down:
                case Heading.Up:
                    relevantLine = string.Join(string.Empty, map.Select(row => row.Length > currentPosition.X ? row[currentPosition.X] : ' '));
                    index = currentPosition.Y;
                    offsetAction = dy => currentPosition = new Point(currentPosition.X , currentPosition.Y + dy);
                    break;
            }

            string nextChars;
            if (heading < 0)
            {
                nextChars = string.Join(string.Empty, relevantLine.Substring(0, index + 1).Reverse());
            }
            else
            {
                nextChars = relevantLine.Substring(index);
            }

            var spaces = nextChars.Skip(1).Take(Math.Min(numSpacesToMove, nextChars.Count() - 1)).ToList();
            if (spaces.Any(x => x == '#'))
            {
                offsetAction.Invoke(Math.Sign((int)heading) * spaces.IndexOf('#'));
                return new CubeLocation(currentPosition, currentLocation.SideName, currentLocation.HeadingOnSide);
            }
            else if (spaces.All(x => x == '.') && spaces.Count == numSpacesToMove)
            {
                offsetAction.Invoke(Math.Sign((int)heading) * numSpacesToMove);
                return new CubeLocation(currentPosition, currentLocation.SideName, currentLocation.HeadingOnSide);
            }
            else
            {
                int spacesToEnd = spaces.Count;
                var remainingSpacesToMove = numSpacesToMove - spacesToEnd;
                offsetAction.Invoke(Math.Sign((int)heading) * spacesToEnd);
                var nextCubeSide = cubeMap.GetNextCubeSide(currentCubeSide.SideName, heading);
                var nextPoint = cubeMap.GetNextPoint(currentCubeSide.SideName, currentPosition, heading);
                var nextHeading = cubeMap.GetNextHeading(currentCubeSide.SideName, heading);

                if (nextCubeSide.Lines[nextPoint.Y][nextPoint.X] == '#')
                {
                    return new CubeLocation(currentPosition, currentLocation.SideName, currentLocation.HeadingOnSide);
                }
                else if (remainingSpacesToMove == 1)
                {                    
                    return new CubeLocation(nextPoint, nextCubeSide.SideName, nextHeading);
                }
                else
                {
                    var newLocation = new CubeLocation(nextPoint, nextCubeSide.SideName, nextHeading);
                    return MoveCubeSide(newLocation, remainingSpacesToMove - 1, cubeMap);
                }
            }
        }

        private static Heading GetNewHeading(Heading heading, string dirMatchValue)
        {
            var headings = new List<Heading> { Heading.Right, Heading.Down, Heading.Left, Heading.Up };
            var currentIndex = headings.IndexOf(heading);
            switch (dirMatchValue)
            {
                case "L":
                    currentIndex--;
                    if (currentIndex < 0)
                    {
                        currentIndex = 3;
                    }
                    break;

                case "R":
                    currentIndex++;
                    currentIndex %= 4;
                    break;
            }

            return headings[currentIndex];
        }

        private static int Move(Heading heading, int numSpacesToMove, int index, string relevantLine)
        {
            string nextChars;
            string otherEnd;
            if (heading < 0)
            {
                nextChars = string.Join(string.Empty, relevantLine.Substring(0, index + 1).Reverse());
                otherEnd = string.Join(string.Empty, relevantLine.Reverse());
            }
            else
            {
                nextChars = relevantLine.Substring(index);
                otherEnd = relevantLine;
            }

            var spaces = nextChars.Skip(1).Take(Math.Min(numSpacesToMove, nextChars.Count() - 1)).ToList();
            if (spaces.Any(x => x == '#'))
            {
                return index + Math.Sign((int)heading) * spaces.IndexOf('#');
            }
            else if (spaces.All(x => x == '.') && spaces.Count == numSpacesToMove)
            {
                return index + Math.Sign((int)heading) * numSpacesToMove;
            }
            else
            {
                int spacesToEnd;
                if (spaces.Any(x => x == ' '))
                {
                    spacesToEnd = spaces.IndexOf(' ');
                }
                else
                {
                    spacesToEnd = spaces.Count;
                }

                var remainingSpacesToMove = numSpacesToMove - spacesToEnd;

                var firstSpace = otherEnd.IndexOf('.');
                var firstWall = otherEnd.IndexOf('#');
                if (firstWall < firstSpace)
                {
                    return index + Math.Sign((int)heading) * spacesToEnd;
                }
                else if (remainingSpacesToMove == 1)
                {
                    return otherEnd != relevantLine ? relevantLine.Length - firstSpace - 1 : firstSpace;
                }                
                else
                {
                    if (otherEnd != relevantLine)
                    {
                        return Move(heading, remainingSpacesToMove - 1, relevantLine.Length - firstSpace - 1, relevantLine);
                    }
                    else
                    {
                        return Move(heading, remainingSpacesToMove - 1, firstSpace, relevantLine);
                    }
                }
            }
        }

        private static void RunDay21()
        {
            var lines = File.ReadAllLines(".\\Input\\Day21.txt").ToList();

            var monkeys = lines.Select(x => new RiddleMonkey(x)).ToList();

            foreach (var monkey in monkeys)
            {
                monkey.OtherMonkeys = monkey.OtherMonkeyNames.Select(name => monkeys.Single(m => m.Name == name)).ToList();
            }

            var root = monkeys.Single(x => x.Name == "root");
            Console.WriteLine($"Day 21 part 1: {root.Value}");

            var op1 = root.OtherMonkeys[0];
            var op2 = root.OtherMonkeys[1];
            var humn = monkeys.Single(x => x.Name == "humn");
            long i = 0;
            var iterationsize = 100000000000;
            while (op1.Value != op2.Value)
            {
                while (op1.Value >= op2.Value)
                {
                    i += iterationsize;
                    humn.ImmediateValue = i;
                }

                Console.WriteLine($"{i} : {op1.Value} {op2.Value}");
                i -= iterationsize;
                humn.ImmediateValue = i;
                iterationsize /= 10;
            }

            Console.WriteLine($"Day 21 part 2: {humn.ImmediateValue}");
        }

        private static void RunDay20()
        {
            var lines = File.ReadAllLines(".\\Input\\Day20.txt").Select(x => new DecryptionVal(long.Parse(x))).ToList();

            var newLines = MixNumbers(lines.ToList(), lines);
            Console.WriteLine($"Day 20 part 1: {CalculateGrove(newLines)}");

            lines = lines.Select(x => new DecryptionVal(x.Value * 811589153)).ToList();
            newLines = lines.ToList();
            for (int i = 0; i< 10; i++)
            {
                newLines = MixNumbers(newLines.ToList(), lines);
            }

            Console.WriteLine($"Day 20 part 2: {CalculateGrove(newLines)}");
        }

        private static long CalculateGrove(List<DecryptionVal> values)
        {
            var startIndex = values.IndexOf(values.Single(x => x.Value == 0));
            var index1 = (startIndex + 1000) % values.Count;
            var index2 = (startIndex + 2000) % values.Count;
            var index3 = (startIndex + 3000) % values.Count;
            var grove = values[index1].Value + values[index2].Value + values[index3].Value;
            return grove;
        }

        private static List<DecryptionVal> MixNumbers(List<DecryptionVal> values, List<DecryptionVal> originalOrder)
        {
            var newValues = values.ToList();
            for (int index = 0; index < originalOrder.Count; index++)
            {
                var value = originalOrder[index];
                if (value.Value == 0)
                {
                    continue;
                }

                var currentIndex = newValues.IndexOf(value);
                var spacesToMove = Math.Sign(value.Value) * (Math.Abs(value.Value) % (values.Count - 1));
                var newIndex = (int)(currentIndex + spacesToMove);

                if (newIndex < 0)
                {
                    newIndex += values.Count - 1;
                }
                else if (newIndex == 0 && currentIndex != 0)
                {
                    newIndex = values.Count - 1;
                }
                else if (newIndex > values.Count - 1)
                {
                    newIndex = (newIndex % values.Count) + 1;
                }

                if (newIndex > currentIndex)
                {
                    var tempList = newValues.Take(currentIndex).Concat(newValues.Skip(currentIndex + 1).Take(newIndex - currentIndex)).ToList();
                    tempList.Add(value);
                    tempList.AddRange(newValues.Skip(newIndex + 1).Take(values.Count - newIndex));
                    newValues = tempList;
                }
                else if (newIndex < currentIndex)
                {
                    var tempList = newValues.Take(newIndex).ToList();
                    tempList.Add(value);
                    tempList.AddRange(newValues.Skip(newIndex).Take(currentIndex - newIndex));
                    tempList.AddRange(newValues.Skip(currentIndex + 1).Take(newValues.Count - currentIndex));
                    newValues = tempList;
                }

            }

            return newValues;
        }

        private static void RunDay19()
        {
            var lines = File.ReadAllLines(".\\Input\\Day19.txt").ToList();
                       
            RunDay19Part1(lines);
            RunDay19Part2(lines);
        }

        private static void RunDay19Part2(List<string> lines)
        {
            var numMinutes = 32;
            var blueprints = lines.Select(x => new Blueprint(x)).Take(3).ToList();
            long result = 1;
            foreach (var blueprint in blueprints)
            {
                var robotStates = RunBlueprint(numMinutes, blueprint);

                result *= robotStates.Max(x => x.GeodeCount);
                Console.WriteLine($"{blueprint.Number}: {robotStates.Max(x => x.GeodeCount)}");
            }

            Console.WriteLine($"Day 19 part 2: {result}");
        }

        private static void RunDay19Part1(List<string> lines)
        {
            var numMinutes = 24;
            var blueprints = lines.Select(x => new Blueprint(x)).ToList();
            foreach (var blueprint in blueprints)
            {
                var robotStates = RunBlueprint(numMinutes, blueprint);

                blueprint.Qualitylevel = robotStates.Max(x => x.GeodeCount) * blueprint.Number;
                Console.WriteLine($"{blueprint.Number}: {blueprint.Qualitylevel}");
            }

            Console.WriteLine($"Day 19 part 1: {blueprints.Sum(x => x.Qualitylevel)}");
        }

        private static List<RobotState> RunBlueprint(int numMinutes, Blueprint blueprint)
        {
            var robotStates = new List<RobotState>();
            var startingState = new RobotState();
            startingState.OreRobotCount = 1;
            robotStates.Add(startingState);
            var firstObsidianTurn = 0;
            var firstGeodeTurn = 0;

            while (robotStates.Any(x => x.CurrentMinute < numMinutes))
            {
                int currentMinute = robotStates.First().CurrentMinute;
                foreach (var robotState in robotStates.Where(x => x.CurrentMinute < numMinutes).ToList())
                {
                    if (blueprint.CanBuy(robotState))
                    {
                        if (blueprint.CanBuyOreRobot(robotState) && robotState.CurrentMinute < numMinutes / 2 && (currentMinute < firstGeodeTurn || firstGeodeTurn == 0) && (currentMinute < firstObsidianTurn + numMinutes / 10 || firstObsidianTurn == 0))
                        {
                            var newState = robotState.Copy();
                            newState.Buy("ore");
                            newState.OreCount -= blueprint.OreRobotCost.OreCost;
                            robotStates.Add(newState);
                            newState.EndMinute();
                        }

                        if (blueprint.CanBuyClayRobot(robotState) && (currentMinute < firstGeodeTurn + numMinutes / 4 || firstGeodeTurn == 0))
                        {
                            var newState = robotState.Copy();
                            newState.Buy("clay");
                            newState.OreCount -= blueprint.ClayRobotCost.OreCost;
                            robotStates.Add(newState);
                            newState.EndMinute();
                        }

                        if (blueprint.CanBuyObsidianRobot(robotState) && (currentMinute < firstGeodeTurn + numMinutes / 4 || firstGeodeTurn == 0))
                        {
                            var newState = robotState.Copy();
                            newState.Buy("obsidian");
                            newState.OreCount -= blueprint.ObsidianRobotCost.OreCost;
                            newState.ClayCount -= blueprint.ObsidianRobotCost.ClayCost;
                            robotStates.Add(newState);
                            newState.EndMinute();

                            if (firstObsidianTurn == 0)
                            {
                                firstObsidianTurn = currentMinute;
                            }
                        }

                        if (blueprint.CanBuyGeodeRobot(robotState))
                        {
                            var newState = robotState.Copy();
                            newState.Buy("geode");
                            newState.OreCount -= blueprint.GeodeRobotCost.OreCost;
                            newState.ObsidianCount -= blueprint.GeodeRobotCost.ObsidianCost;
                            robotStates.Add(newState);
                            newState.EndMinute();

                            if (firstGeodeTurn == 0)
                            {
                                firstGeodeTurn = currentMinute;
                            }
                        }
                    }

                    robotState.EndMinute();
                }

                if (currentMinute == numMinutes/2 && robotStates.Any(x => x.ClayCount > 0))
                {
                    robotStates = robotStates.Where(x => x.ClayRobotCount > 0).ToList();
                }

                if (currentMinute == firstGeodeTurn + numMinutes / 5 && robotStates.Any(x => x.GeodeCount > 0))
                {
                    robotStates = robotStates.Where(x => x.GeodeRobotCount > 0).ToList();
                }

                if (robotStates.All(x => x.GeodeCount > 0))
                {
                    var maxGeodeCount = robotStates.Max(x => x.GeodeCount);
                    var maxGeodeRobotcount = robotStates.Max(x => x.GeodeRobotCount);

                    robotStates = robotStates.Where(x => x.GeodeCount >= maxGeodeCount - 1 || x.GeodeRobotCount >= maxGeodeRobotcount - 1).ToList();
                }

                if (robotStates.Count > 1000000)
                {
                    robotStates = robotStates.OrderByDescending(x => x.Score).Take(1000000).ToList();
                }
            }

            return robotStates;
        }

        private static void RunDay18()
        {
            var lines = File.ReadAllLines(".\\Input\\Day18.txt").ToList();
            var cubes = new List<Cube>();
            foreach (var line in lines)
            {
                var cube = new Cube(line);
                var adjacentCubes = cubes.Where(x => x.IsAdjacent(cube)).ToList();
                cube.CoveredSides += adjacentCubes.Count;
                adjacentCubes.ForEach(x => x.CoveredSides++);

                cubes.Add(cube);
            }

            int numSides = cubes.Count * 6 - cubes.Sum(x => x.CoveredSides);
            Console.WriteLine($"Day 18 part 1: {numSides}");

            int innerSides = 0;
            var emptyCubes = GetEmptyCubes(cubes).Distinct(new CubeEqualityComparer()).ToList();
            foreach (var emptyCube in emptyCubes)
            {
                if (!DoesPathOutExist(emptyCube, cubes))
                {
                    var adjacentCubes = cubes.Where(x => x.IsAdjacent(emptyCube)).ToList();
                    innerSides += adjacentCubes.Count;
                }
            }

            Console.WriteLine($"Day 18 part 2: {numSides - innerSides}");
        }

        private static bool DoesPathOutExist(Cube emptyCube, List<Cube> cubes)
        {
            var maxX = cubes.Max(x => x.X);
            var maxY = cubes.Max(x => x.Y);
            var maxZ = cubes.Max(x => x.Z);
            var minX = cubes.Min(x => x.X);
            var minY = cubes.Min(x => x.Y);
            var minZ = cubes.Min(x => x.Z);

            var allPaths = new List<Cube> { emptyCube };
            var isFree = false;
            var visited = new List<Cube>() { emptyCube };
            while (!isFree)
            {
                foreach (var path in allPaths.ToList())
                {
                    var newCubes = new List<Cube>()
                    {
                        path.Offset(-1, 0, 0),
                        path.Offset(1, 0, 0),
                        path.Offset(0, 1, 0),
                        path.Offset(0, -1, 0),
                        path.Offset(0, 0, 1),
                        path.Offset(0, 0, -1)
                    };

                    var newPaths = newCubes.Where(x => !cubes.Any(c => c.IsCoincident(x)) && !visited.Any(v => v.IsCoincident(x))).ToList();
                    var existingWayOut = newPaths.FirstOrDefault(x => wayOutCache.ContainsKey(x.ToString()));
                    if (existingWayOut != null)
                    {
                        return wayOutCache[existingWayOut.ToString()];
                    }

                    visited.AddRange(newPaths);
                    visited = visited.Distinct(new CubeEqualityComparer()).ToList();
                    allPaths.Remove(path);
                    allPaths.AddRange(newPaths);
                }

                isFree = !allPaths.Any() || allPaths.Any(p => !p.X.IsBetween(minX, maxX) ||
                                      !p.Y.IsBetween(minY, maxY) ||
                                      !p.Z.IsBetween(minZ, maxZ));
            }

            foreach (var cube in visited)
            {
                wayOutCache.Add(cube.ToString(), allPaths.Any());
            }

            return allPaths.Any();
        }

        private static List<Cube> GetEmptyCubes(List<Cube> cubes)
        {
            var emptyCubes = new List<Cube>();
            foreach (var cube in cubes)
            {
                for (int i = 0; i < 3; i++)
                {
                    switch (i)
                    {
                        case 0:
                            var adjacentCubes = cubes.Where(x => x.IsAdjacentX(cube)).ToList();
                            if (!adjacentCubes.Any())
                            {
                                emptyCubes.Add(cube.Offset(1, 0, 0));
                                emptyCubes.Add(cube.Offset(-1, 0, 0));
                            }
                            else if (adjacentCubes.Count == 1)
                            {
                                if (cubes.Any(x => x.X == cube.X + 1 && cube.Y == x.Y && cube.Z == x.Z))
                                {
                                    emptyCubes.Add(cube.Offset(-1, 0, 0));
                                }
                                else
                                {
                                    emptyCubes.Add(cube.Offset(1, 0, 0));
                                }
                            }

                            break;
                        case 1:
                            var adjacentCubesY = cubes.Where(x => x.IsAdjacentY(cube)).ToList();
                            if (!adjacentCubesY.Any())
                            {
                                emptyCubes.Add(cube.Offset(0, 1, 0));
                                emptyCubes.Add(cube.Offset(0, -1, 0));
                            }
                            else if (adjacentCubesY.Count == 1)
                            {
                                if (cubes.Any(x => x.X == cube.X && cube.Y + 1 == x.Y && cube.Z == x.Z))
                                {
                                    emptyCubes.Add(cube.Offset(0, -1, 0));
                                }
                                else
                                {
                                    emptyCubes.Add(cube.Offset(0, 1, 0));
                                }
                            }
                            break;
                        case 2:

                            var adjacentCubesZ = cubes.Where(x => x.IsAdjacentZ(cube)).ToList();
                            if (!adjacentCubesZ.Any())
                            {
                                emptyCubes.Add(cube.Offset(0, 0, 1));
                                emptyCubes.Add(cube.Offset(0, 0, -1));
                            }
                            else if (adjacentCubesZ.Count == 1)
                            {
                                if (cubes.Any(x => x.X == cube.X && cube.Y == x.Y && cube.Z + 1 == x.Z))
                                {
                                    emptyCubes.Add(cube.Offset(0, 0, -1));
                                }
                                else
                                {
                                    emptyCubes.Add(cube.Offset(0, 0, 1));
                                }
                            }
                            break;

                    }
                }

            }

            return emptyCubes;
        }

        private static void RunDay17()
        {
            var line = File.ReadAllLines(".\\Input\\Day17.txt").ToList().First();

            var rockOrder = new List<string> { "-", "+", "L", "|", "o" };
            var tower = new Tower(7);
            long rockCount = 0;
            var jetIndex = 0;
            long totalRocks = 2022;
            while (rockCount < totalRocks)
            {
                int index = (int)(rockCount % rockOrder.Count);
                DropRock(tower, rockOrder[index], line, ref jetIndex);
                rockCount++;
            }

            Console.WriteLine($"Day 17 part 1: {tower.Heights.Max()}");

            tower = new Tower(7);
            rockCount = 0;
            jetIndex = 0;
            totalRocks = (long)1e12;

            while (rockCount < 1000)
            {
                int index = (int)(rockCount % rockOrder.Count);
                DropRock(tower, rockOrder[index], line, ref jetIndex);
                rockCount++;
            }

            long lastHeight = tower.Heights.Max();
            long lastRockCount = rockCount;
            var refIndex = jetIndex;

            do
            {
                int index = (int)(rockCount % rockOrder.Count);
                DropRock(tower, rockOrder[index], line, ref jetIndex);
                rockCount++;
            }
            while ((rockCount % 5) > 0 || jetIndex != refIndex);
            var heightDiff = tower.Heights.Max() - lastHeight;
            var rockDiff = rockCount - lastRockCount;

            long numTimes = (totalRocks - rockCount) / rockDiff;
            rockCount += numTimes * rockDiff;
            long totalHeightDiff = numTimes * heightDiff;

            while (rockCount < totalRocks)
            {
                int index = (int)(rockCount % rockOrder.Count);
                DropRock(tower, rockOrder[index], line, ref jetIndex);
                rockCount++;
            }

            Console.WriteLine($"Day 17 part 2: {tower.Heights.Max() + totalHeightDiff}");
        }

        private static void DropRock(Tower tower, string rockType, string line, ref int jetIndex)
        {
            var rock = new Rock(rockType);
            rock.StartDrop(tower.Heights.Max() + 3);
            var isDropping = false;

            while (!rock.IsDropComplete)
            {
                if (isDropping)
                {
                    rock.Drop(tower);
                    isDropping = false;
                }
                else
                {
                    if (line[jetIndex] == '<')
                    {
                        rock.MoveLeft(tower);
                    }
                    else
                    {
                        rock.MoveRight(tower);
                    }

                    jetIndex = (jetIndex + 1) % line.Length;
                    isDropping = true;
                }
            }
        }

        private static void RunDay16()
        {
            var lines = File.ReadAllLines(".\\Input\\Day16.txt").ToList();
            var allValves = new List<Valve>();
            var pattern = @"Valve (?<name>\w+) has flow rate=(?<flowRate>\d+); tunnels? leads? to valves? (?<valves>(\w+(, )?)+)";
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var match = Regex.Match(line, pattern);
                var valve = new Valve(match.Groups["name"].Value, int.Parse(match.Groups["flowRate"].Value));
                allValves.Add(valve);
            }

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var match = Regex.Match(line, pattern);
                var valve = allValves.Single(x => x.Name == match.Groups["name"].Value);
                valve.LinkedValves.AddRange(match.Groups["valves"].Value.Split(',').Select(x => allValves.Single(v => v.Name == x.Trim())));
            }

            int totalMinutes = 30;

            var currentValve = allValves.Single(x => x.Name == "AA");
            var path = GetBestPath(currentValve, allValves, totalMinutes);
            var score = CalculateScore(path, allValves, totalMinutes, false);
            Console.WriteLine($"Day 16 part 1: {score}");

            //totalMinutes = 26;
            //allValves.ForEach(x => x.Reset());

            //var doublepath = GetBestDoublePath(currentValve, allValves, totalMinutes);
            //score = CalculateScore(doublepath, allValves, totalMinutes, false);
            //Console.WriteLine($"Day 16 part 2: {score}");
        }

        private static DoublePath GetBestDoublePath(Valve startValve, List<Valve> allValves, int totalMinutes)
        {
            var validValves = allValves.Where(x => !x.IsOpen && x.FlowRate > 0);
            var currentValve = startValve;
            var paths = new List<DoublePath>();
            int maxScore = 0;
            paths.Add(new DoublePath(new List<Valve>() { startValve }));
            while (!paths.All(x => x.AllValves.Count == validValves.Count() + 1))
            {
                var nextIteration = new List<DoublePath>();
                var found = false;
                foreach (var path in paths.ToList())
                {
                    var remainingValves = validValves.Except(path.YourPath).Except(path.ElephantPath).ToList();
                    if (!remainingValves.Any())
                    {
                        found = true;
                        break;
                    }
                    foreach (var remainingValve in remainingValves)
                    {
                        var enumerable = remainingValves.Where(x => x != remainingValve).ToList();
                        List<DoublePath> newPaths;
                        if (!enumerable.Any())
                        {
                            newPaths = new List<DoublePath>() { path.CreateNext(remainingValve, path.ElephantPath.First()), path.CreateNext(path.YourPath.First(), remainingValve) };
                        }
                        else
                        {
                            newPaths = enumerable.Select(x => path.CreateNext(remainingValve, x)).ToList();
                        }

                        var filteredPaths = new List<DoublePath>();
                        var maxLength = totalMinutes / 2;
                        foreach (var newPath in newPaths)
                        {
                            var yourScore = CalculateScore(newPath.YourPath, allValves, totalMinutes, false);
                            var elephantScore = CalculateScore(newPath.ElephantPath, allValves, totalMinutes, false);
                            if (newPath.YourPath.Max(x => x.OpenTime) >= totalMinutes &&
                                newPath.ElephantPath.Max(x => x.OpenTime) >= totalMinutes)
                            {
                                filteredPaths.Add(newPath);
                            }

                            maxLength = new List<int>() { maxLength, newPath.YourPath.Max(x => x.OpenTime), newPath.YourPath.Max(x => x.OpenTime) }.Max();
                            allValves.ForEach(x => x.Reset());
                        }

                        nextIteration = nextIteration.Concat(newPaths.Except(filteredPaths)).OrderByDescending(x => CalculateScore(x, allValves, Math.Min(maxLength, totalMinutes), true)).Take(500).ToList();
                        maxScore = filteredPaths.Any() ? Math.Max(maxScore, filteredPaths.Max(x => CalculateScore(x, allValves, totalMinutes, true))) : maxScore;
                    }
                }

                if (found)
                {
                    break;
                }

                paths = nextIteration;
            }

            Console.WriteLine($"Possible max score {maxScore}");
            return paths.OrderBy(x => CalculateScore(x, allValves, totalMinutes, true)).Last();
        }
        private static int CalculateScore(DoublePath doublePath, List<Valve> allValves, int totalMinutes, bool reset)
        {
            return CalculateScore(doublePath.YourPath, allValves, totalMinutes, reset) + CalculateScore(doublePath.ElephantPath, allValves, totalMinutes, reset);
        }

        private static List<Valve> GetBestPath(Valve startValve, List<Valve> allValves, int totalMinutes)
        {
            var validValves = allValves.Where(x => !x.IsOpen && x.FlowRate > 0);
            var currentValve = startValve;
            var paths = new List<List<Valve>>();
            paths.Add(new List<Valve>() { startValve });
            while (!paths.All(x => x.Count == validValves.Count() + 1))
            {
                var nextIteration = new List<List<Valve>>();
                foreach (var path in paths.ToList())
                {
                    var remainingValves = validValves.Except(path).ToList();
                    var newPaths = remainingValves.Select(x => path.Concat(new List<Valve>() { x }).ToList()).ToList();
                    nextIteration = nextIteration.Concat(newPaths).OrderByDescending(x => CalculateScore(x, allValves, totalMinutes, true)).Take(50).ToList();
                }

                paths = nextIteration;
            }

            return paths.OrderBy(x => CalculateScore(x, allValves, totalMinutes, true)).Last();
        }

        private static int CalculateScore(List<Valve> path, List<Valve> allValves, int totalMinutes, bool reset)
        {
            var current = path.First();
            var totalDistance = 0;
            foreach (var valve in path.Skip(1))
            {
                var shortestDistance = FindShortestDistance(current, valve, allValves);
                totalDistance += shortestDistance + 1;
                valve.Open(totalDistance);
                current = valve;
            }

            var score = path.Select(x => x.GetScore(totalMinutes)).Sum();
            if (reset)
            {
                path.ForEach(x => x.Reset());
            }

            return score;
        }

        private static int FindShortestDistance(Valve currentValve, Valve validValve, List<Valve> allValves)
        {
            var paths = new List<List<Valve>>() { new List<Valve>() { currentValve } };
            var found = false;
            var key = string.Join(string.Empty, new List<Valve>() { currentValve, validValve }.OrderBy(x => x.Name).Select(x => x.Name));
            if (distanceCache.ContainsKey(key))
            {
                return distanceCache[key];
            }

            while (!found)
            {
                foreach (var path in paths.ToList())
                {
                    var newPaths = path.Last().LinkedValves.Where(x => !path.Contains(x))
                        .Select(x => path.Concat(new List<Valve>() { x }).ToList()).ToList();
                    paths.Remove(path);
                    paths.AddRange(newPaths);
                }

                found = paths.Any(x => x.Last() == validValve);

            }

            int distance = paths.Where(x => x.Last() == validValve).Min(x => x.Count - 1);
            distanceCache[key] = distance;

            return distance;
        }

        private static void RunDay15()
        {
            var lines = File.ReadAllLines(".\\Input\\Day15.txt").ToList();
            var sensors = new List<Sensor>();
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var sensor = new Sensor(line);
                sensors.Add(sensor);
            }

            var xranges = GetXRangesWithNoBeacon(sensors, 2000000);
            var total1 = xranges.Sum(x => x.Item2 - x.Item1 + 1);
            total1 -= sensors.Select(x => x.ClosestBeacon).Where(p => p.Y == 2000000 && xranges.Any(r => ((long)p.X).IsBetween(r.Item1, r.Item2))).Distinct().Count();
            Console.WriteLine($"Day 15 part 1: {total1}");
            //RunDay15Part2(sensors);
        }

        private static void RunDay15Part2(List<Sensor> sensors)
        {
            long total2 = 0;
            for (int y = 0; y <= 4000000; y++)
            {
                var xranges = GetXRangesWithNoBeacon(sensors, y);
                if (xranges.Count > 1)
                {
                    bool found = false;
                    foreach (var range1 in xranges)
                    {
                        var possibleRanges = xranges.Where(range2 => (range1.Item1 - range2.Item2) == 2
                                                || (range2.Item1 - range1.Item2) == 2);
                        foreach (var possibleRange in possibleRanges)
                        {
                            if (possibleRange.Item1.IsBetween(0, 4000000))
                            {
                                total2 = (possibleRange.Item1 - 1) * 4000000 + y;
                                Console.WriteLine($"Day 15 part 2: {total2}");
                                found = true;
                            }
                            else if (possibleRange.Item2.IsBetween(0, 4000000))
                            {
                                total2 = (possibleRange.Item2 + 1) * 4000000 + y;
                                Console.WriteLine($"Day 15 part 2: {total2}");
                                found = true;
                            }
                        }
                    }

                    if (found)
                    {
                        break;
                    }
                }
            }
        }

        private static List<Tuple<long, long>> GetXRangesWithNoBeacon(List<Sensor> sensors, int row)
        {
            var ranges = new List<Tuple<long, long>>();
            foreach (var sensor in sensors)
            {
                var newRange = sensor.GetXRangeWithNoBeacon(row);
                if (newRange.Item1 == 0 && newRange.Item2 == 0)
                {
                    continue;
                }

                bool isRestarting;
                do
                {
                    isRestarting = false;
                    var allRanges = ranges.ToList();
                    foreach (var range in allRanges)
                    {
                        if (CheckForOverlap(range, newRange))
                        {
                            newRange = CombineRanges(range, newRange);
                            ranges.Remove(range);
                            isRestarting = true;
                            break;
                        }
                    }
                }
                while (isRestarting);

                ranges.Add(newRange);
            }

            return ranges;
        }

        private static Tuple<long, long> CombineRanges(Tuple<long, long> range1, Tuple<long, long> range2)
        {
            return new Tuple<long, long>(Math.Min(range1.Item1, range2.Item1), Math.Max(range1.Item2, range2.Item2));
        }

        private static bool CheckForOverlap(Tuple<long, long> range1, Tuple<long, long> range2)
        {
            return range1.Item1.IsBetween(range2.Item1, range2.Item2) ||
                range1.Item2.IsBetween(range2.Item1, range2.Item2) ||
                range2.Item1.IsBetween(range1.Item1, range1.Item2) ||
                range2.Item2.IsBetween(range1.Item1, range1.Item2);
        }

        private static void RunDay14()
        {
            var textLines = File.ReadAllLines(".\\Input\\Day14.txt").ToList();
            var lines = new List<Line>();
            for (int i = 0; i < textLines.Count; i++)
            {
                var textLine = textLines[i];
                var pattern = @"(?<x>\d+),(?<y>\d+)";
                var matches = Regex.Matches(textLine, pattern);
                Point currentPoint = new Point(0, 0), previousPoint = new Point(0, 0);

                foreach (Match match in matches)
                {
                    currentPoint = new Point(int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value));

                    if (previousPoint != new Point(0, 0))
                    {
                        var line = new Line(previousPoint, currentPoint);
                        lines.Add(line);
                    }

                    previousPoint = currentPoint;
                }
            }

            var startingPoint = new Point(500, 0);
            var sandcastle = new SandCastle(lines);
            while (!sandcastle.IsDone)
            {
                sandcastle.DropSand(startingPoint);
            }
            Console.WriteLine($"Day 14 part 1: {sandcastle.SandCount}");

            sandcastle = new SandCastle(lines);
            sandcastle.IsUsingFloor = true;
            while (!sandcastle.IsDone)
            {
                sandcastle.DropSand(startingPoint);
            }

            Console.WriteLine($"Day 14 part 2: {sandcastle.SandCount}");
        }

        private static void RunDay13()
        {
            var lines = File.ReadAllLines(".\\Input\\Day13.txt").ToList();
            long total1 = 0;
            var index = 1;
            var allPacketItems = new List<PacketItem>();
            var divider1 = new PacketItem("[[2]]");
            var divider2 = new PacketItem("[[6]]");
            allPacketItems.Add(divider1);
            allPacketItems.Add(divider2);
            while (lines.Any())
            {
                var packets = lines.Take(2).ToList();

                var left = new PacketItem(packets[0]);
                var right = new PacketItem(packets[1]);

                if (left.IsInCorrectOrder(right) == true)
                {
                    total1 += index;
                }

                lines = lines.Skip(3).ToList();
                index++;

                allPacketItems.Add(left);
                allPacketItems.Add(right);
            }

            var sortedPackets = allPacketItems.OrderBy(x => x, new PacketItemComparer()).ToList();
            var index1 = sortedPackets.FindIndex(x => x == divider1) + 1;
            var index2 = sortedPackets.FindIndex(x => x == divider2) + 1;

            Console.WriteLine($"Day 13 part 1: {total1}");
            Console.WriteLine($"Day 13 part 2: {index1 * index2}");
        }

        private static void RunDay12()
        {
            var lines = File.ReadAllLines(".\\Input\\Day12.txt").ToList();
            var startLine = lines.Single(x => x.Contains('S'));
            var endLine = lines.Single(x => x.Contains('E'));
            int startLineIndex = lines.IndexOf(startLine);
            int endLineIndex = lines.IndexOf(endLine);

            var startPoint = new Point(startLine.IndexOf('S'), startLineIndex);
            var endPoint = new Point(endLine.IndexOf('E'), endLineIndex);
            lines[startLineIndex] = lines[startLineIndex].Replace('S', 'a').Replace('E', 'z');
            lines[endLineIndex] = lines[endLineIndex].Replace('S', 'a').Replace('E', 'z');

            var paths = new List<Path>();
            bool[,] visited = new bool[lines.Count, lines.First().Length];

            for (int y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    var height = line[x];
                    if (height == 'a')
                    {
                        paths.Add(new Path(new Point(x, y), height));
                        visited[y, x] = true;
                    }
                }
            }

            while (paths.All(x => x.CurrentPoint != endPoint))
            {
                var unfinishedPaths = paths.Where(x => x.CurrentPoint != endPoint).ToList();
                foreach (var path in unfinishedPaths)
                {
                    var newPoints = path.GetNewPoints().Where(p => IsValidPoint(lines, p, path.Height) && !visited[p.Y, p.X]).ToList();
                    paths.Remove(path);

                    foreach (var point in newPoints)
                    {
                        var newPath = path.Copy();
                        newPath.CurrentPoint = point;
                        newPath.Height = lines[point.Y][point.X];
                        newPath.NumSteps++;
                        visited[point.Y, point.X] = true;
                        paths.Add(newPath);
                    }
                }
            }

            long total1 = paths.Single(x => x.CurrentPoint == endPoint).NumSteps;

            Console.WriteLine($"Day 12: {total1}");
        }

        private static bool IsValidPoint(List<string> lines, Point p, char height)
        {

            return p.X >= 0 && p.X < lines.First().Length
                && p.Y >= 0 && p.Y < lines.Count
                && lines[p.Y][p.X] <= height + 1;
        }

        private static void RunDay11()
        {
            var lines = File.ReadAllLines(".\\Input\\Day11.txt");
            var monkeys = new List<Monkey>();
            var items = new List<MonkeyItem>();
            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines[i];
                if (line.StartsWith("Monkey"))
                {
                    monkeys.Add(new Monkey(monkeys.Count));
                }
                else if (line.Contains("Starting items"))
                {
                    monkeys.Last().Items = new Queue<int>(line.Substring(line.IndexOf(':') + 1).Split(',').Select(x => int.Parse(x)).ToList());
                    items.AddRange(line.Substring(line.IndexOf(':') + 1).Split(',').Select(x => int.Parse(x)).Select(x => new MonkeyItem(x, monkeys.Last())));
                }
                else if (line.Contains("Operation"))
                {
                    monkeys.Last().Operation = line.Substring(line.IndexOf(':') + 1).Replace("new =", string.Empty);
                }
                else if (line.Contains("Test"))
                {
                    monkeys.Last().TestValue = int.Parse(line.Substring(line.IndexOf(':') + 1).Replace("divisible by", string.Empty));
                }
                else if (line.Contains("If true"))
                {
                    monkeys.Last().DestIfTrue = int.Parse(line.Substring(line.IndexOf(':') + 1).Replace("throw to monkey", string.Empty));
                }
                else if (line.Contains("If false"))
                {
                    monkeys.Last().DestIfFalse = int.Parse(line.Substring(line.IndexOf(':') + 1).Replace("throw to monkey", string.Empty));
                }
            }

            foreach (var item in items)
            {
                foreach (var monkey in monkeys)
                {
                    item.ModValueByMonkey.Add(monkey, item.StartingWorryLevel % monkey.TestValue);
                }
            }

            for (int round = 0; round < 20; round++)
            {
                foreach (var monkey in monkeys)
                {
                    var hasItems = monkey.Items.Any();
                    while (hasItems)
                    {
                        var item = monkey.Items.Dequeue();
                        var dt = new DataTable();
                        item = (int)dt.Compute(monkey.Operation.Replace("old", $"{item}"), "");
                        item = (int)Math.Floor(item / 3.0);
                        var destMonkeyIndex = (item % monkey.TestValue) == 0 ? monkey.DestIfTrue : monkey.DestIfFalse;
                        monkeys.Single(x => x.Index == destMonkeyIndex).Items.Enqueue(item);
                        monkey.InspectionCount++;
                        hasItems = monkey.Items.Any();
                    }
                }
            }

            long total = 1;
            foreach (var monkey in monkeys.OrderByDescending(x => x.InspectionCount).Take(2))
            {
                total *= monkey.InspectionCount;
            }

            Console.WriteLine($"Day 11 part 1: {total}");


            foreach (var monkey in monkeys)
            {
                monkey.Items.Clear();
                monkey.InspectionCount = 0;
            }

            for (int round = 0; round < 10000; round++)
            {
                foreach (var currentMonkey in monkeys)
                {
                    var monkeyItems = items.Where(x => x.CurrentMonkey == currentMonkey).ToList();
                    foreach (var item in monkeyItems)
                    {
                        foreach (var monkey in monkeys)
                        {
                            var dt = new DataTable();
                            item.ModValueByMonkey[monkey] = (int)dt.Compute(currentMonkey.Operation.Replace("old", $"{item.ModValueByMonkey[monkey]}"), "");
                            item.ModValueByMonkey[monkey] %= monkey.TestValue;
                        }

                        var destMonkeyIndex = item.ModValueByMonkey[currentMonkey] == 0 ? currentMonkey.DestIfTrue : currentMonkey.DestIfFalse;
                        item.CurrentMonkey = monkeys.Single(x => x.Index == destMonkeyIndex);
                        currentMonkey.InspectionCount++;
                    }
                }
            }

            total = 1;
            foreach (var monkey in monkeys.OrderByDescending(x => x.InspectionCount).Take(2))
            {
                total *= monkey.InspectionCount;
            }

            Console.WriteLine($"Day 11 part 2: {total}");
        }

        private static void RunDay10()
        {
            var lines = File.ReadAllLines(".\\Input\\Day10.txt");
            long total1 = 0;
            long xVal = 1;
            long cycle = 1;
            int i = 0;
            var toAdd = 0;
            var turnsToWait = 0;
            var pixels = Enumerable.Repeat('.', 240).ToList();
            while (i < lines.Count() || turnsToWait != 0)
            {
                if (turnsToWait == 0)
                {
                    var line = lines[i];
                    if (line.StartsWith("addx"))
                    {
                        toAdd = int.Parse(line.Substring(4));
                        turnsToWait = 1;
                    }

                    i++;
                }
                else
                {
                    turnsToWait--;
                }

                if (cycle == 20 || ((cycle - 20) % 40) == 0)
                {
                    total1 += cycle * xVal;
                }


                var horzPosition = (cycle % 40) - 1;
                if (horzPosition < 0) horzPosition += 40;
                if (Math.Abs(xVal - horzPosition) <= 1)
                {
                    pixels[(int)cycle - 1] = '#';
                }

                if (turnsToWait == 0 && toAdd != 0)
                {
                    xVal += toAdd;
                    toAdd = 0;
                }

                cycle++;
            }


            Console.WriteLine($"Day 10 part 1: {total1}");
            Console.WriteLine($"Day 10 part 2:");

            for (int j = 0; j < pixels.Count / 40; j++)
            {
                Console.WriteLine($"{string.Join("", pixels.Skip(j * 40).Take(40))}");
            }
        }

        private static void RunDay9()
        {
            var lines = File.ReadAllLines(".\\Input\\Day9.txt");
            var positions = new List<Point>();
            var positions2 = new List<Point>();
            var knots = new List<Knot>();
            var headKnot = new Knot();
            knots.Add(headKnot);
            for (int i = 1; i < 10; i++)
            {
                knots.Add(new Knot(knots.Last()));
            }

            positions.Add(knots[1].Position);
            positions2.Add(knots.Last().Position);
            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines[i];

                var direction = line.First();
                var motion = int.Parse(line.Substring(2));

                for (int j = 0; j < motion; j++)
                {
                    switch (direction)
                    {
                        case 'L':
                            headKnot.Move(-1, 0);
                            break;
                        case 'R':
                            headKnot.Move(1, 0);
                            break;
                        case 'U':
                            headKnot.Move(0, 1);
                            break;
                        case 'D':
                            headKnot.Move(0, -1);
                            break;
                    }

                    if (!positions.Contains(knots[1].Position))
                    {
                        positions.Add(knots[1].Position);
                    }

                    if (!positions2.Contains(knots.Last().Position))
                    {
                        positions2.Add(knots.Last().Position);
                    }
                }
            }

            long total1 = positions.Count();
            long total2 = positions2.Count();
            Console.WriteLine($"Day 9 part 1: {total1}");
            Console.WriteLine($"Day 9 part 2: {total2}");
        }

        private static void RunDay8()
        {
            var lines = File.ReadAllLines(".\\Input\\Day8.txt");
            long total1 = 0, total2 = 0;

            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    if (i == 0 || j == 0 || i == line.Length - 1 || j == line.Length - 1)
                    {
                        total1++;
                    }
                    else
                    {
                        var height = int.Parse($"{line[j]}");
                        var dir1 = line.Substring(0, j).Select(x => int.Parse(x.ToString())).ToList();
                        var dir2 = line.Substring(j + 1, line.Length - (j + 1)).Select(x => int.Parse(x.ToString())).ToList();
                        var dir3 = lines.Select(x => int.Parse($"{x[j]}")).Take(i).ToList();
                        var dir4 = lines.Select(x => int.Parse($"{x[j]}")).Skip(i + 1).ToList();

                        if (dir1.All(x => x < height) ||
                            dir2.All(x => x < height) ||
                            dir3.All(x => x < height) ||
                            dir4.All(x => x < height))
                        {
                            total1++;
                        }

                        dir1.Reverse();
                        dir3.Reverse();

                        var value1 = dir1.TakeWhile(x => x < height).Count();
                        if (value1 != dir1.Count())
                            value1++;

                        var value2 = dir2.TakeWhile(x => x < height).Count();
                        if (value2 != dir2.Count())
                            value2++;

                        var value3 = dir3.TakeWhile(x => x < height).Count();
                        if (value3 != dir3.Count())
                            value3++;

                        var value4 = dir4.TakeWhile(x => x < height).Count();
                        if (value4 != dir4.Count())
                            value4++;

                        total2 = Math.Max(total2, value1 * value2 * value3 * value4);
                    }
                }
            }

            Console.WriteLine($"Day 8 part 1: {total1}");
            Console.WriteLine($"Day 8 part 2: {total2}");
        }

        private static void RunDay7()
        {
            var lines = File.ReadAllLines(".\\Input\\Day7.txt");
            var topFolder = new Folder("top");
            var currentFolder = topFolder;
            var pattern1 = @"dir (?<dirName>\w+)";
            var pattern2 = @"(?<size>\d+) .+\z";
            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines[i];

                if (line.StartsWith("$ cd"))
                {
                    currentFolder = ChangeDirectory(topFolder, currentFolder, line);
                }
                else if (line.StartsWith("$ ls"))
                {
                    // do nothing
                }
                else
                {
                    var dirMatch = Regex.Match(line, pattern1);
                    var fileMatch = Regex.Match(line, pattern2);
                    if (dirMatch.Success)
                    {
                        var dirName = dirMatch.Groups["dirName"].Value;
                        if (currentFolder.SubFolders.SingleOrDefault(x => x.Name == dirName) is null)
                        {
                            var newFolder = new Folder(dirName, currentFolder);
                            currentFolder.SubFolders.Add(newFolder);
                        }
                    }
                    else if (fileMatch.Success)
                    {
                        var size = int.Parse(fileMatch.Groups["size"].Value);
                        currentFolder.Files.Add(size);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            var allFolders = topFolder.GetAllFolders();
            var total1 = allFolders.Where(x => x.GetSize() <= 100000).Sum(x => x.GetSize());

            Console.WriteLine($"Day 7 part 1: {total1}");

            var totalSize = topFolder.GetSize();
            var spaceRemaining = 7e7 - totalSize;
            var spaceNeeded = 3e7 - spaceRemaining;
            var folderToDelete = allFolders.Where(x => x.GetSize() > spaceNeeded).OrderBy(x => x.GetSize()).First();

            Console.WriteLine($"Day 7 part 2: {folderToDelete.GetSize()}");
        }

        private static Folder ChangeDirectory(Folder topFolder, Folder currentFolder, string line)
        {
            var pattern3 = @"cd (?<dirName>.+\z)";
            Match match = Regex.Match(line, pattern3);

            string dirName = match.Groups["dirName"].Value;
            switch (dirName)
            {
                case "/":
                    currentFolder = topFolder;
                    break;

                case "..":
                    currentFolder = currentFolder.ParentFolder;
                    break;

                default:
                    currentFolder = currentFolder.SubFolders.SingleOrDefault(x => x.Name == dirName);
                    if (currentFolder is null)
                    {
                        var newFolder = new Folder(dirName, currentFolder);
                        currentFolder.SubFolders.Add(newFolder);
                        currentFolder = newFolder;
                    }

                    break;
            }

            return currentFolder;
        }

        private static void RunDay6()
        {
            var line = File.ReadAllLines(".\\Input\\Day6.txt").Single();
            var total1 = GetStart(4, line);
            var total2 = GetStart(14, line);

            Console.WriteLine($"Day 6 part 1: {total1}");
            Console.WriteLine($"Day 6 part 2: {total2}");
        }

        private static long GetStart(int numChars, string line)
        {
            var total = numChars;
            while (line.Length >= numChars)
            {
                var chars = line.Take(numChars);

                if (chars.Distinct().Count() == chars.Count())
                {
                    break;
                }

                line = line.Substring(1, line.Length - 1);
                total++;
            }

            return total;
        }

        private static void RunDay5()
        {
            var stacks = new List<List<char>>();
            for (int j = 0; j < 9; j++)
            {
                stacks.Add(new List<char>());
            }

            var lines = File.ReadAllLines(".\\Input\\Day5.txt");
            int i = 0;
            for (; i < lines.Count(); i++)
            {
                var line = lines[i];
                if (line.StartsWith(" 1"))
                {
                    i++; i++;
                    break;
                }
                var col = 0;
                while (!string.IsNullOrEmpty(line))
                {
                    var character = line.Substring(0, Math.Min(line.Length, 4));
                    if (!string.IsNullOrWhiteSpace(character))
                    {
                        stacks[col].Add(character[1]);
                    }


                    if (line.Length < 4)
                    {
                        break;
                    }

                    line = line.Substring(4, Math.Max(0, line.Length - 4)).TrimEnd();
                    col++;
                }
            }

            for (; i < lines.Count(); i++)
            {
                var line = lines[i];

                var pattern = @"move (?<count>\d+) from (?<start>\d) to (?<end>\d)";
                var match = Regex.Match(line, pattern);

                if (match.Success)
                {
                    var count = int.Parse(match.Groups["count"].Value);
                    var start = int.Parse(match.Groups["start"].Value) - 1;
                    var end = int.Parse(match.Groups["end"].Value) - 1;

                    //for (int c = 0; c < count; c++)
                    //{
                    //    //stacks[end].Insert(0, stacks[start].First());
                    //    stacks[start].RemoveAt(0);
                    //}

                    stacks[end] = stacks[start].Take(count).Concat(stacks[end]).ToList();
                    stacks[start].RemoveRange(0, count);
                }
            }

            Console.WriteLine($"Day 5: {string.Join(string.Empty, stacks.Select(x => x.FirstOrDefault()))}");
        }

        private static void RunDay4()
        {
            var total1 = 0;
            var total2 = 0;
            var lines = File.ReadAllLines(".\\Input\\Day4.txt");
            foreach (var line in lines)
            {
                var assignments = line.Split(',');

                var ends = assignments[0].Split('-');
                var start = int.Parse(ends.First());
                var end = int.Parse(ends.Last());

                var ends2 = assignments[1].Split('-');
                var start2 = int.Parse(ends2.First());
                var end2 = int.Parse(ends2.Last());

                var set1 = Enumerable.Range(start, end - start + 1);
                var set2 = Enumerable.Range(start2, end2 - start2 + 1);

                int v = set1.Intersect(set2).Count();
                if (v == set2.Count() || v == set1.Count())
                {
                    total1++;
                }

                if (v > 0)
                {
                    total2++;
                }
            }

            Console.WriteLine($"Day 4 part 1: {total1}");
            Console.WriteLine($"Day 4 part 2: {total2}");
        }

        private static void RunDay3()
        {
            var lines = File.ReadAllLines(".\\Input\\Day3.txt");
            long total = 0;
            long total2 = 0;
            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines[i];
                var part1 = line.Substring(0, line.Length / 2);
                var part2 = line.Substring(line.Length / 2, line.Length / 2);

                var priority = part1.Intersect(part2).Single();
                if (priority >= 'a')
                {
                    total += priority - 'a' + 1;
                }
                else
                {
                    total += priority - 'A' + 1 + 26;
                }

                if ((i % 3) == 2)
                {
                    var priority2 = lines[i].Intersect(lines[i - 1]).Intersect(lines[i - 2]).Single();
                    if (priority2 >= 'a')
                    {
                        total2 += priority2 - 'a' + 1;
                    }
                    else
                    {
                        total2 += priority2 - 'A' + 1 + 26;
                    }
                }

            }

            Console.WriteLine($"Day 3 part 1: {total}");
            Console.WriteLine($"Day 3 part 2: {total2}");

        }

        private static void RunDay1()
        {
            var text = File.ReadAllText(".\\Input\\Day1.txt");

            var lines = text.Split('\n');

            var total = 0;
            var values = new List<int>();
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                total += string.IsNullOrEmpty(line.Trim()) ? 0 : int.Parse(line);
                if (string.IsNullOrEmpty(line.Trim()) || i == lines.Length - 1)
                {
                    values.Add(total);
                    total = 0;
                }
            }

            Console.WriteLine($"Day 1 part 1: {values.Max()}");
            Console.WriteLine($"Day 1 part 2: {values.OrderByDescending(x => x).Take(3).Sum()}");
        }

        private static void RunDay2()
        {
            var lines = File.ReadAllLines(".\\Input\\Day2.txt");
            long score1 = 0;
            long score2 = 0;
            foreach (var line in lines)
            {
                var terms = line.Split(' ');
                var them = terms[0].Single() - 'A' + 1;
                var me = terms[1].Single() - 'X' + 1;
                score1 += me;
                if (me == ((them % 3) + 1))
                {
                    score1 += 6;
                }
                else if (me == them)
                {
                    score1 += 3;
                }

                score2 += (me - 1) * 3;
                switch (me)
                {
                    case 1:
                        int v = them - 1;
                        if (v == 0)
                            v += 3;
                        score2 += v;
                        break;
                    case 2:
                        score2 += them;
                        break;
                    case 3:
                        score2 += (them % 3) + 1;
                        break;
                }
            }

            Console.WriteLine($"Day 2 part 1: {score1}");
            Console.WriteLine($"Day 2 part 2: {score2}");
        }
    }
}
