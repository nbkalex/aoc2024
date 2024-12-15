using System.Drawing;

var input = File.ReadAllLines("in.txt").ToList();

int emptyLineIndex = input.IndexOf("");
var mapInput = input.Take(emptyLineIndex).ToArray();
Dictionary<PointF, char> map = new Dictionary<PointF, char>();
for (int i = 0; i < mapInput.Length; i++)
  for (int j = 0; j < mapInput[0].Length; j++)
    map.Add(new PointF(j, i), mapInput[i][j]);

var directions = new Dictionary<char, Point>()
{
  {'<', new Point(-1, 0) },
  {'>', new Point(1, 0) },
  {'^', new Point(0, -1) },
  {'v', new Point(0, 1) },
};

PointF robot = map.First(kvp => kvp.Value == '@').Key;

var movements = input.Skip(emptyLineIndex + 1).ToArray();
foreach (var m in movements)
{
  foreach(char d in m)
  {
    var dir = directions[d];
    List<PointF> points = new List<PointF>();
    PointF current = robot;
    while(map[current] != '.' && map[current] != '#')
    {
      points.Add(current);
      current = new PointF(current.X + dir.X, current.Y + dir.Y);
    }

    if(map[current] == '.')
    { 
      map[robot] ='.';
      if(points.Count == 1)
        robot = current;
      else
      { 
        robot = points[1];
        map[current] = 'O';
      }

      map[robot] = '@';
    }

    //Console.Clear();
    //foreach(var p in map)
    //{
    //  Console.SetCursorPosition(p.Key.X, p.Key.Y);
    //  Console.Write(p.Value);
    //}  

    //Console.ReadKey();
  }
}

var boxes = map.Where(kvp => kvp.Value == 'O').ToArray();
Console.WriteLine(boxes.Sum(p => p.Key.X + (p.Key.Y*100)));

Console.WriteLine();
