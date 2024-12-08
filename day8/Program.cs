using System.Drawing;

var input = File.ReadAllLines("in.txt");
input = input.Select(l => l.Replace("#", ".")).ToArray();

Dictionary<char, List<Point>> map = new Dictionary<char, List<Point>>();
HashSet<Point> antenas = new HashSet<Point>();
for (int i = 0; i < input.Length; i++)
{
  for (int j = 0; j < input[0].Length; j++)
    if (input[i][j] != '.')
    {
      if (map.ContainsKey(input[i][j]))
        map[input[i][j]].Add(new Point(j, i));
      else
        map.Add(input[i][j], new List<Point>() { new Point(j, i) });

      antenas.Add(new Point(j, i));
    }
}

HashSet<Tuple<Point, Point>> antiAntenas = new HashSet<Tuple<Point, Point>>();

foreach (var freaq in map.Keys)
{
  foreach (var antena in map[freaq])
  {
    foreach (var otherAntena in map[freaq])
    {
      if (antena == otherAntena)
        continue;

      Point antiAntena = ComputeOppositeLocation(antena, otherAntena);
      if (IsValid(antiAntena))
        antiAntenas.Add(new Tuple<Point, Point>(antena, antiAntena));
    }
  }
}

// Part 1
Console.WriteLine(antiAntenas.Select(aa => aa.Item2).Distinct().Count());

HashSet<Point> taa = new HashSet<Point>();
foreach (var freaq in map.Keys)
{
  foreach (var antena in map[freaq])
  {
    foreach (var otherAntena in map[freaq])
    {
      if (antena == otherAntena)
        continue;

      int xDiff = otherAntena.X - antena.X;
      int yDiff = otherAntena.Y - antena.Y;

      int xStep = xDiff;
      int yStep = yDiff;

      for (int i = 2; i < Math.Max(xDiff, yDiff) / 2; i++)
      {
        while (xStep % i == 0 && yStep % i == 0)
        {
          xStep /= i;
          yStep /= i;
        }
      }

      Point current = antena;
      while (IsValid(current))
      {
        taa.Add(current);
        current = new Point(current.X + xStep, current.Y + yStep);
      }

      current = antena;
      while (IsValid(current))
      {
        taa.Add(current);
        current = new Point(current.X - xStep, current.Y - yStep);
      }
    }
  }
}

Console.WriteLine(taa.Count());

Point ComputeOppositeLocation(Point p1, Point p2)
{
  return new Point(2 * p2.X - p1.X, 2 * p2.Y - p1.Y);
}

bool IsValid(Point p1)
{
  return p1.X >= 0 && p1.X < input[0].Length
      && p1.Y >= 0 && p1.Y < input.Length;
}