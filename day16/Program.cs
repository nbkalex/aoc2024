using System.Drawing;

string[] input = File.ReadAllLines("in.txt");

HashSet<Point> walls = new HashSet<Point>();

Point start = Point.Empty;
Point end = Point.Empty;

for (int i = 0; i < input.Length; i++)
  for (int j = 0; j < input[i].Length; j++)
  {
    var p = new Point(j, i);
    if (input[i][j] == 'S')
      start = p;
    else if (input[i][j] == 'E')
      end = p;
    else if (input[i][j] == '#')
      walls.Add(p);
  }

Point[] directions =
{
  new Point(0, 1),
  new Point(-1,0),
  new Point(0,-1),
  new Point(1, 0),
};


Stack<(Point, Point, int, HashSet<Point>)> toGo = new Stack<(Point, Point, int, HashSet<Point>)>();
toGo.Push((start, new Point(1, 0), 0, new HashSet<Point>() { start }));

HashSet<Point> bestPaths = null;
int minCost = int.MaxValue;

Dictionary<(Point, Point), int> minCosts = new Dictionary<(Point, Point), int>();

while (toGo.Any())
{
  var current = toGo.Pop();
  foreach (Point dir in directions)
  {
    Point next = new Point(current.Item1.X + dir.X, current.Item1.Y + dir.Y);

    if (walls.Contains(next) || current.Item4.Contains(next))
      continue;

    int additionalCost = dir == current.Item2 ? 0 : 1000;
    int cost = current.Item3 + additionalCost + 1;
    if(cost > minCost)
      continue;

    HashSet<Point> trace = new HashSet<Point>(current.Item4);
    trace.Add(next);

    if (next == end)
    {
      if (cost < minCost)
      {
        minCost = cost;
        bestPaths = trace;
      }

      if (cost == minCost)
      {
        foreach (var p in trace)
          bestPaths.Add(p);
      }

      continue;
    }

    var minHash = (next, dir);

    if (minCosts.ContainsKey(minHash))
    {
      if (minCosts[minHash] >= cost)
      {
        toGo.Push((next, dir, cost, trace));
        minCosts[minHash] = cost;
      }
    }

    else
    {
      toGo.Push((next, dir, cost, trace));
      minCosts[minHash] = cost;
    }
  }
}

Console.WriteLine(minCost);
Console.WriteLine(bestPaths.Count);