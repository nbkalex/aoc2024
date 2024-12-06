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
HashSet<Point> path = new HashSet<Point>();

Point currentPos = new Point(start.X, start.Y);

while(currentPos.X >= 0 && currentPos.X < input[0].Length && currentPos.Y >= 0 && currentPos.Y < input.Length)
{
  path.Add(currentPos);

  Point currendDirPoint = directions[currentDir % directions.Length];
  Point nextPos = new Point(currentPos.X + currendDirPoint.X, currentPos.Y + currendDirPoint.Y);

  if(obstructions.Contains(nextPos))
    currentDir++;
  else
    currentPos = nextPos;
}

Console.WriteLine(path.Count);

foreach (Point p in path)
{
  

}