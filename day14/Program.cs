using System.Drawing;

var input = File.ReadAllLines("in.txt");

List<(Point, Point)> robots = new List<(Point, Point)>();
foreach (var line in input)
{
  string[] robotTokens = line.Split(' ');
  var posCoords = robotTokens[0].Substring(2).Split(',').Select(coord => int.Parse(coord)).ToArray();
  var velocityVals = robotTokens[1].Substring(2).Split(',').Select(val => int.Parse(val)).ToArray();
  robots.Add((new Point(posCoords[0], posCoords[1]), new Point(velocityVals[0], velocityVals[1])));
}

const int width = 101;
const int height = 103;


for (long seconds = 8006; seconds < 1000000; seconds++)
{
  HashSet<Point> futureRobots = new HashSet<Point>();
  foreach (var robot in robots)
  {
    int x = (int)((robot.Item1.X + (robot.Item2.X * seconds)) % width);
    int y = (int)((robot.Item1.Y + (robot.Item2.Y * seconds)) % height);

    if (x < 0)
      x = width + x;
    if (y < 0)
      y = height + y;

    futureRobots.Add(new Point(x, y));
  }

  for (int h = 0; h < height; h++)
  {
    int count = 0;
    for (int i = 0; i < width; i++)
    {
      if (futureRobots.Contains(new Point(i, h)))
        count++;
    }

    if (count > width / 4)
    {
      foreach (var robot in futureRobots)
      {
        Console.SetCursorPosition(robot.X, robot.Y);
        Console.Write("#");
      }

      Console.ReadKey();
      Console.Clear();
      Console.WriteLine(seconds);
    }
  }
}


//var futureRobotsQ1 = futureRobots.Where(robot => robot.Item1 < width / 2 && robot.Item2 < height / 2);
//var futureRobotsQ2 = futureRobots.Where(robot => robot.Item1 > width / 2 && robot.Item2 < height / 2);
//var futureRobotsQ3 = futureRobots.Where(robot => robot.Item1 < width / 2 && robot.Item2 > height / 2);
//var futureRobotsQ4 = futureRobots.Where(robot => robot.Item1 > width / 2 && robot.Item2 > height / 2);

//Console.WriteLine(futureRobotsQ1.Count() * futureRobotsQ2.Count() * futureRobotsQ3.Count() * futureRobotsQ4.Count());