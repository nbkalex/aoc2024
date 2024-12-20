using System.Drawing;

var input = File.ReadAllLines("in.txt").ToList();

int emptyLineIndex = input.IndexOf("");
var mapInput = input.Take(emptyLineIndex).ToArray();

Point robot = Point.Empty;
Dictionary<Point, char> map = new Dictionary<Point, char>();
for (int i = 0; i < mapInput.Length; i++)
  for (int j = 0; j < mapInput[0].Length; j++)
  {
    if (mapInput[i][j] == '@')
      robot = new Point(j, i);

    map.Add(new Point(j, i), mapInput[i][j]);
    //Point left = new Point(2 * j, i);
    //Point right = new Point(2 * j + 1, i);

    //char leftSymbol = ' ';
    //char rightSymbol = ' ';

    //if (mapInput[i][j] == '@')
    //{
    //  leftSymbol = '@';
    //  rightSymbol = '.';
    //  robot = new Point(j, i);
    //}
    //else if (mapInput[i][j] == 'O')
    //{
    //  leftSymbol = '[';
    //  rightSymbol = ']';
    //}
    //else
    //{
    //  leftSymbol = mapInput[i][j];
    //  rightSymbol = mapInput[i][j];
    //}

    //map.Add(left, leftSymbol);
    //map.Add(right, rightSymbol);
  }

var directions = new Dictionary<char, Point>()
{
  {'<', new Point(-1, 0) },
  {'>', new Point(1, 0) },
  {'^', new Point(0, -1) },
  {'v', new Point(0, 1) },
};


Console.Clear();
foreach (var p in map)
{
  Console.SetCursorPosition(p.Key.X, p.Key.Y);
  Console.Write(p.Value);
}

Console.ReadKey();


var movements = input.Skip(emptyLineIndex + 1).ToArray();
foreach (var m in movements)
{
  foreach (char d in m)
  {
    var dir = directions[d];
    Point current = robot;
    List<(Point, char)> points = new List<(Point, char)>()
    {
      (robot, map[robot])
    };

    //if (dir.Y != 0)
    //{
      while (map[current] != '.' && map[current] != '#')
      {
        current = AddPoints(current, dir);
        points.Add((current, map[current]));
      }

      if (map[current] == '.')
      {
        for (var i = 1; i < points.Count; i++)
          map[points[i].Item1] = points[i - 1].Item2;

        map[robot] = '.';
        robot = AddPoints(robot, dir);
      }
    //}

    //Console.Clear();
    //foreach (var p in map)
    //{
    //  Console.SetCursorPosition(p.Key.X, p.Key.Y);
    //  Console.Write(p.Value);
    //}

    //Console.ReadKey();
  }
}

var boxes = map.Where(kvp => kvp.Value == 'O').ToArray();
Console.WriteLine(boxes.Sum(p => p.Key.X + (p.Key.Y * 100)));

//Console.WriteLine();

Point AddPoints(Point p1, Point p2)
{
  return new Point(p1.X + p2.X, p1.Y + p2.Y);
}
