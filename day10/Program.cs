using System.Drawing;

string[] input = File.ReadAllLines("in.txt");
Dictionary<Point, int> map = new Dictionary<Point, int>();
for (int i = 0; i < input.Length; i++)
  for (int j = 0; j < input[0].Length; j++)
    map.Add(new Point(j, i), input[i][j] - '0');

Point[] directions =
{
  new Point(0, 1),
  new Point(0, -1),
  new Point(1, 0),
  new Point(-1, 0),
};

int count = 0;
int countAll = 0;
var startPoints = map.Where(p => p.Value == 0).Select(p => p.Key);

foreach (var point in startPoints)
{
  List<Point> trails = new List<Point>();
  HashSet<Point> visited = new HashSet<Point>();

  Stack<(Point, HashSet<Point>)> queue = new Stack<(Point, HashSet<Point>)>();
  queue.Push((point, new HashSet<Point>()));

  while(queue.Any())
  {
    var current = queue.Pop();
    current.Item2.Add(current.Item1);


    if (map.ContainsKey(current.Item1) && map[current.Item1] == 9)
    {
      trails.Add(current.Item1);
      continue;
    }

    visited.Add(current.Item1);

    foreach (var dir in directions)
    {
      Point newPos = new Point(current.Item1.X + dir.X, current.Item1.Y + dir.Y);
      if (IsOnMap(newPos) && map[newPos] - map[current.Item1] == 1 && !current.Item2.Contains(newPos))
      {
        HashSet<Point> trail = new HashSet<Point>(current.Item2);
        trail.Add(current.Item1);
        queue.Push((newPos, trail));
      }
    }
  }

  count += trails.Distinct().Count();
  countAll += trails.Count;
}

Console.WriteLine(count);
Console.WriteLine(countAll);

bool IsOnMap(Point point)
{
  return point.X >= 0 && point.X < input[0].Length && point.Y >= 0 && point.Y < input.Length;
}