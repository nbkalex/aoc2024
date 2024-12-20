using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

string[] input = File.ReadAllLines("in.txt");


Point start = Point.Empty;
Point end = Point.Empty;
HashSet<Point> walls = new HashSet<Point>();
for (int i = 0; i < input.Length; i++)
  for (int j = 0; j < input[0].Length; j++)
  {
    var current = new Point(j, i);
    if (input[i][j] == '#')
      walls.Add(current);
    else if (input[i][j] == 'S')
      start = current;
    else if (input[i][j] == 'E')
      end = current;
  }

Point[] directions =
{
  new Point(0,1),
  new Point(0,-1),
  new Point(1,0),
  new Point(-1,0)
};

Point currentPos = start;
List<Point> trace = new List<Point>()
{
  start
};

Dictionary<int, int> maxJumps = new Dictionary<int, int>();

while (currentPos != end)
{
  foreach (var dir in directions)
  {
    Point next = new Point(currentPos.X + dir.X, currentPos.Y + dir.Y);
    if (!walls.Contains(next) && !trace.Contains(next))
    {
      currentPos = next;
      trace.Add(next);
      break;
    }
  }
}

int minJump = 100;
int count = 0;
for (int i = 0; i < trace.Count; i++)
{
  var startJump = trace.Where(p => Distance(p, trace[i]) <= 20).ToArray();
  foreach(var sj in startJump)
  {
    var d = Distance(sj, trace[i]);
    int jumpSize = trace.IndexOf(sj) - i - d;
    if (jumpSize >= minJump)
    {
      if (maxJumps.ContainsKey(jumpSize))
        maxJumps[jumpSize]++;
      else
        maxJumps.Add(jumpSize, 1);

      count++;
    }
  }
}

Console.WriteLine(count);


bool IsOnMap(Point p)
{
  return p.X >= 0 && p.Y >= 0 && p.X < input[0].Length && p.Y < input.Length;
}

int Distance(Point p1, Point p2)
{
  var res = Math.Abs(p2.X - p1.X) + Math.Abs(p2.Y - p1.Y);
  return res;
}