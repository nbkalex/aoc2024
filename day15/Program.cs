using System.Drawing;

var input = File.ReadAllLines("in.txt").ToList();

int emptyLineIndex = input.IndexOf("");
var mapInput = input.Take(emptyLineIndex).ToArray();

Point robot = Point.Empty;
Dictionary<Point, char> map = new Dictionary<Point, char>();
for (int i = 0; i < mapInput.Length; i++)
  for (int j = 0; j < mapInput[0].Length; j++)
  {
    //if (mapInput[i][j] == '@')
    //  robot = new Point(j, i);

    //map.Add(new Point(j, i), mapInput[i][j]);
    Point left = new Point(2 * j, i);
    Point right = new Point(2 * j + 1, i);

    char leftSymbol = ' ';
    char rightSymbol = ' ';

    if (mapInput[i][j] == '@')
    {
      leftSymbol = '@';
      rightSymbol = '.';
      robot = left;
    }
    else if (mapInput[i][j] == 'O')
    {
      leftSymbol = '[';
      rightSymbol = ']';
    }
    else
    {
      leftSymbol = mapInput[i][j];
      rightSymbol = mapInput[i][j];
    }

    map.Add(left, leftSymbol);
    map.Add(right, rightSymbol);
  }

var directions = new Dictionary<char, Point>()
{
  {'<', new Point(-1, 0) },
  {'>', new Point(1, 0) },
  {'^', new Point(0, -1) },
  {'v', new Point(0, 1) },
};


//Console.Clear();
//foreach (var p in map)
//{
//  Console.SetCursorPosition(p.Key.X, p.Key.Y);
//  Console.Write(p.Value);
//}

//Console.ReadKey();


var movements = input.Skip(emptyLineIndex + 1).ToArray();
foreach (var m in movements)
{
  foreach (char d in m)
  {
    var dir = directions[d];
    List<(Point, char)> points = new List<(Point, char)>();

    Queue<(Point, char)> queue = new Queue<(Point, char)>();
    queue.Enqueue((robot, '.'));

    bool canMove = true;
    while (queue.Count > 0)
    {
      var current = queue.Dequeue();
      points.Add((current.Item1, current.Item2));

      if (map[current.Item1] == '.' && current.Item1 != robot)
        continue;

      Point next = AddPoints(current.Item1, dir);

      if (map[next] == '#')
      {
        canMove = false;
        break;
      }

      if (map[next] != '.')
      {
        if (dir.Y == 0)
          queue.Enqueue((next, map[current.Item1]));
        else
        {
          Point left = Point.Empty;
          Point right = Point.Empty;

          char leftC = ' ';
          char rightC = ' ';

          if (map[next] == '[')
          {
            var r = new Point(current.Item1.X + 1, current.Item1.Y);

            bool containsR = points.Any(p => p.Item1 == r);

            leftC = map[current.Item1];
            rightC = current.Item1 == robot || !containsR ? '.' : map[r];
            left = next;
            right = new Point(next.X + 1, next.Y);
          }
          else
          {
            var l = new Point(current.Item1.X - 1, current.Item1.Y);

            bool containsL = points.Any(p => p.Item1 == l);

            leftC = current.Item1 == robot | !containsL ? '.' : map[l];
            rightC = map[current.Item1];

            left = new Point(next.X - 1, next.Y);
            right = next;
          }

          queue.Enqueue((left, leftC));
          queue.Enqueue((right, rightC));
        }
      }
      else
        queue.Enqueue((next, map[current.Item1]));
    }

    if (canMove)
    { 
      foreach (var p in points)
        map[p.Item1] = p.Item2;

      robot = AddPoints(robot, dir);
    }

    //Console.ReadKey();
    //Console.Clear();
    //foreach (var p in map)
    //{
    //  Console.SetCursorPosition(p.Key.X, p.Key.Y);
    //  Console.Write(p.Value);
    //}
  }
}

var boxes = map.Where(kvp => kvp.Value == '[' ).ToArray();
Console.WriteLine(boxes.Sum(p => p.Key.X + (p.Key.Y * 100)));

//Console.WriteLine();

Point AddPoints(Point p1, Point p2)
{
  return new Point(p1.X + p2.X, p1.Y + p2.Y);
}
