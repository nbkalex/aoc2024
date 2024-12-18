using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

var bytes = File.ReadAllLines("in.txt").Select(l => l.Split(",").Select(c => int.Parse(c)).ToArray()).Select(l => new Point(l[0], l[1])).ToArray();

int width = 70;
int height = 70;

Point start = new Point(0, 0);
Point end = new Point(width, height);

Point[] direction =
{
  new Point(0,1),
  new Point(0,-1),
  new Point(1,0),
  new Point(-1,0),
};

for (int i = 0; i < bytes.Count(); i++)
{
  var walls = bytes.Take(i).ToHashSet();

  var minValue = new Dictionary<Point, int>
  {
    { start, 0 }
  };

  var toVisit = new Queue<Point>();
  toVisit.Enqueue(start);
  while (toVisit.Any())
  {
    var current = toVisit.Dequeue();

    foreach (var dir in direction)
    {
      Point next = new Point(current.X + dir.X, current.Y + dir.Y);
      if (next.X < 0 || next.Y < 0 || next.X > width || next.Y > height ||
         walls.Contains(next) || (minValue.ContainsKey(next) && minValue[next] <= minValue[current] + 1))
        continue;

      minValue[next] = minValue[current] + 1;
      toVisit.Enqueue(next);
    }
  }

  if(!minValue.ContainsKey(end))
  { 
    Console.WriteLine(bytes[i-1].X + "," + bytes[i-1].Y);
    break;
  }
}

  
