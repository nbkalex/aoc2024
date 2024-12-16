using System.Drawing;

var input = File.ReadAllLines("in.txt").ToList();

int emptyLineIndex = input.IndexOf("");
var mapInput = input.Take(emptyLineIndex).ToArray();

Point robot = Point.Empty;
Dictionary<Point, char> map = new Dictionary<Point, char>();
for (int i = 0; i < mapInput.Length; i++)
  for (int j = 0; j < mapInput[0].Length; j++)
  {
    Point left = new Point(2 * j, i);
    Point right = new Point(2 * j + 1, i);

    char leftSymbol = ' ';
    char rightSymbol = ' ';

    if(mapInput[i][j] == '@')
    {
      leftSymbol = '@';
      rightSymbol = '.';
      robot = new Point(j, i);
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
  foreach(char d in m)
  {
    var dir = directions[d];
    List<(Point,char)> points = new List<(Point, char)>();
    Point current = robot;
    
    while(map[current] != '.' && map[current] != '#')
      points.Add((current, map[current]));

    if(points.Count > 2)
    {
      for(int i = 0; i < points.Count; i++)
      {

      }

    }


    if(points.Count > 1)
    {
      map[robot] = '.';
      robot = points[1].Item1;
      map[robot] = '@';
    }    

    //Console.Clear();
    //foreach (var p in map)
    //{
    //  Console.SetCursorPosition(p.Key.X, p.Key.Y);
    //  Console.Write(p.Value);
    //}

    //Console.ReadKey();
  }
}

//var boxes = map.Where(kvp => kvp.Value == 'O').ToArray();
//Console.WriteLine(boxes.Sum(p => p.Key.X + (p.Key.Y*100)));

//Console.WriteLine();
