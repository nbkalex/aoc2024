using System.Drawing;

var input = File.ReadAllLines("in.txt");

Point start = new Point();
HashSet<Point> obstructions = new HashSet<Point>();

for(int i = 0; i < input.Length; i++)
{
  for(int j = 0; j < input[0].Length; j++)
  {
    if(input[i][j] == '#')
      obstructions.Add(new Point(j,i));
    if(input[i][j] == '^')
      start = new Point(j,i);
  }
}

Point[] directions = new Point[]
{
  new Point(0, -1),
  new Point(1, 0),
  new Point(0, 1),
  new Point(-1, 0)
};

int currentDir = 0;
HashSet<Tuple<Point,Point>> path = new HashSet<Tuple<Point, Point>>();

Point currentPos = new Point(start.X, start.Y);

while(currentPos.X >= 0 && currentPos.X < input[0].Length && currentPos.Y >= 0 && currentPos.Y < input.Length)
{
  Point currendDirPoint = directions[currentDir % directions.Length];
  path.Add(new Tuple<Point, Point>(currentPos, currendDirPoint));

  Point nextPos = new Point(currentPos.X + currendDirPoint.X, currentPos.Y + currendDirPoint.Y);

  if(obstructions.Contains(nextPos))
    currentDir++;
  else
    currentPos = nextPos;
}

var pathPoints = path.Select(p => p.Item1).ToHashSet();
Console.WriteLine(pathPoints.Count);

int count = 0;

foreach (Point p in pathPoints)
{
  currentPos = new Point(start.X, start.Y);
  currentDir = 0;
  HashSet<Tuple<Point, Point>> newPath = new HashSet<Tuple<Point, Point>>();
  obstructions.Add(p);

  bool hasLoop = false;

  while (currentPos.X >= 0 && currentPos.X < input[0].Length && currentPos.Y >= 0 && currentPos.Y < input.Length)
  {
    Point currendDirPoint = directions[currentDir % directions.Length];
    if(!newPath.Add(new Tuple<Point, Point>(currentPos, currendDirPoint)))
    {
      hasLoop = true;
      break;
    }

    Point nextPos = new Point(currentPos.X + currendDirPoint.X, currentPos.Y + currendDirPoint.Y);

    if (obstructions.Contains(nextPos))
      currentDir++;
    else
      currentPos = nextPos;
  }

  if (hasLoop)
    count++;  

  obstructions.Remove(p);
}

Console.WriteLine(count);