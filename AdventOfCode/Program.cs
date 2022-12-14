using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            RunDay1();
            RunDay2();
            RunDay3();
            RunDay4();
            RunDay5();
            RunDay6();
            RunDay7();
            RunDay8();
            RunDay9();
            RunDay10();
            //RunDay11();
            RunDay12();
            RunDay13();
            RunDay14();
            Console.ReadKey();
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
                Point currentPoint = new Point(0, 0), previousPoint = new Point(0,0);
                
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

            for (int y= 0; y < lines.Count; y++ )
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
                        visited[point.Y,point.X] = true;
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
                else if(line.Contains("Starting items"))
                {
                    monkeys.Last().Items = new Queue<int>(line.Substring(line.IndexOf(':')+1).Split(',').Select(x => int.Parse(x)).ToList());
                    items.AddRange(line.Substring(line.IndexOf(':') + 1).Split(',').Select(x => int.Parse(x)).Select(x => new MonkeyItem(x, monkeys.Last())));
                }
                else if (line.Contains("Operation"))
                {
                    monkeys.Last().Operation = line.Substring(line.IndexOf(':') + 1).Replace("new =", string.Empty);
                }
                else if (line.Contains("Test"))
                {
                    monkeys.Last().TestValue = int.Parse(line.Substring(line.IndexOf(':') + 1).Replace("divisible by",string.Empty));
                }
                else if (line.Contains("If true"))
                {
                    monkeys.Last().DestIfTrue = int.Parse(line.Substring(line.IndexOf(':') + 1).Replace("throw to monkey",string.Empty));
                }
                else if (line.Contains("If false"))
                {
                    monkeys.Last().DestIfFalse = int.Parse(line.Substring(line.IndexOf(':') + 1).Replace("throw to monkey",string.Empty));
                }
            }

            foreach (var item in items)
            {
                foreach (var monkey in monkeys)
                {
                    item.ModValueByMonkey.Add(monkey, item.StartingWorryLevel % monkey.TestValue);
                }
            }

            for (int round = 0; round<20; round++)
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


                var horzPosition = (cycle % 40) -1;
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
                            headKnot.Move(-1,0); 
                            break;
                        case 'R':
                            headKnot.Move(1,0); 
                            break;
                        case 'U':
                            headKnot.Move(0,1); 
                            break;
                        case 'D':
                            headKnot.Move(0,-1); 
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
                    if (i == 0 || j == 0 || i == line.Length-1 || j == line.Length-1)
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
                else if(line.StartsWith("$ ls"))
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
            var total1= allFolders.Where(x => x.GetSize() <= 100000).Sum(x => x.GetSize());

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
            for (int j = 0; j< 9; j++)
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
                    var character = line.Substring(0, Math.Min(line.Length,4));
                    if (!string.IsNullOrWhiteSpace(character))
                    {
                        stacks[col].Add(character[1]);
                    }

                    
                    if (line.Length< 4)
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
                    var priority2  = lines[i].Intersect(lines[i - 1]).Intersect(lines[i - 2]).Single();
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
