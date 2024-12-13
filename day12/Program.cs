
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

string[] input = File.ReadAllLines("in.txt");

Dictionary<Point, char> map = new Dictionary<Point, char>();

for (int i = 0; i < input.Length; i++)
  for (int j = 0; j < input[0].Length; j++)
    map.Add(new Point(j, i), input[i][j]);

HashSet<Point> visited = new HashSet<Point>();

Point[] dirs =
{
  new Point(0, -1),
  new Point(0, 1),
  new Point(-1, 0),
  new Point(1, 0)
};

Dictionary<List<Point>, Tuple<int, int>> regions = new Dictionary<List<Point>, Tuple<int, int>>();

foreach (var point in map.Keys)
{
  if (visited.Contains(point))
    continue;

  char currentPlant = map[point];

  List<Point> region = new List<Point>();
  int perimeter = 0;
  Dictionary<Tuple<Point, Point>, List<Point>> sides = new Dictionary<Tuple<Point, Point>, List<Point>>();

  HashSet<PointF> outerSides = new HashSet<PointF>();

  Queue<Point> queue = new Queue<Point>();
  queue.Enqueue(point);

  while (queue.Any())
  {
    Point current = queue.Dequeue();

    if (visited.Contains(current))
      continue;

    region.Add(current);
    visited.Add(current);

    int currentPer = 0;
    foreach (var d in dirs)
    {
      Point next = new Point(current.X + d.X, current.Y + d.Y);
      if (IsOnMap(next) && map[current] == map[next])
      {
        queue.Enqueue(next);
      }
      else
      {
        Point side = new Point(Math.Abs(d.X) * current.X, Math.Abs(d.Y) * current.Y);
        var sideDetails = new Tuple<Point, Point>(side, d);
        if (!sides.ContainsKey(sideDetails))
          sides.Add(sideDetails, new List<Point>());
        sides[sideDetails].Add(next);
        currentPer++;

        PointF outerSide = new PointF(current.X + (float)d.X / 5, current.Y + (float)d.Y / 5);
        outerSides.Add(outerSide);
      }
    }

    perimeter += currentPer;
  }

  HashSet<PointF> visitedSides = new HashSet<PointF>();
  int sideCount2 = 0;
  foreach(var os in outerSides)
  {
    if(visitedSides.Contains(os)) 
      continue;

    sideCount2++;
    List<PointF> directions = new List<PointF>();
    if(Math.Round(os.X) - os.X != 0)
    {
      directions.Add(new Point(0,-1));
      directions.Add(new Point(0, 1));
    }
    else
    {
      directions.Add(new Point(-1,0));
      directions.Add(new Point(1, 0));
    }

    foreach(var dir in directions)
    {
      PointF current = os;
      while(outerSides.Contains(current))
      {
        visitedSides.Add(current);
        current = new PointF(current.X + dir.X, current.Y + dir.Y);
      }
    }
  }

  regions.Add(region, new Tuple<int, int>(perimeter, sideCount2));
}

Console.WriteLine(regions.Sum(r => r.Key.Count * r.Value.Item1));
Console.WriteLine(regions.Sum(r => r.Key.Count * r.Value.Item2));

bool IsOnMap(Point point)
{
  return point.X >= 0 && point.Y >= 0 && point.X < input[0].Length && point.Y < input.Length;
}

bool IsOnSide(Point point, char c)
{
  foreach(var dir in dirs)
  {
    Point p = new Point(point.X + dir.X , point.Y + dir.Y);
    if(!IsOnMap(p) || map[p] != c)
      return true;
  }

  return false;
}