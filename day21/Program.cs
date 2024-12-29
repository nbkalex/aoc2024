using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

var directions = new Dictionary<char, Point>()
{
  {'>',new Point(1, 0)},
  {'<',new Point(-1, 0)},
  {'v',new Point(0, 1)},
  {'^',new Point(0, -1) }
};

//+---+---+---+
//| 7 | 8 | 9 |
//+---+---+---+
//| 4 | 5 | 6 |
//+---+---+---+
//| 1 | 2 | 3 |
//+---+---+---+
//    | 0 | A |
//    +---+---+
var numericKeypad = new Dictionary<Point, char>()
{
  {new Point(0, 0),'7'},
  {new Point(1, 0),'8'},
  {new Point(2, 0),'9'},
  {new Point(0, 1),'4'},
  {new Point(1, 1),'5'},
  {new Point(2, 1),'6'},
  {new Point(0, 2),'1'},
  {new Point(1, 2),'2'},
  {new Point(2, 2),'3'},
  {new Point(1, 3),'0'},
  {new Point(2, 3),'A'}
};

//    +---+---+
//    | ^ | A |
//+---+---+---+
//| < | v | > |
//+---+---+---+
var directionalKeypad = new Dictionary<Point, char>()
{
  {new Point(1, 0), '^'},
  {new Point(2, 0), 'A'},
  {new Point(0, 1), '<'},
  {new Point(1, 1), 'v'},
  {new Point(2, 1), '>'},
};

var numericPaths = new Dictionary<(char, char), List<string>>();
foreach (var nk in numericKeypad)
{
  Queue<(Point, string, HashSet<Point>)> queue = new Queue<(Point, string, HashSet<Point>)>();
  queue.Enqueue((nk.Key, "", new HashSet<Point>()));
  
  while (queue.Any())
  {
    var current = queue.Dequeue();

    foreach (var dir in directions)
    {
      var next = AddPoints(current.Item1, dir.Value);
      if(!numericKeypad.ContainsKey(next) || current.Item3.Contains(next))
        continue;

      var newPath = current.Item2 + dir.Key;
      var nextPoints = new HashSet<Point>(current.Item3);
      nextPoints.Add(next);
      queue.Enqueue((next, newPath, nextPoints));

      var hash = (nk.Value, numericKeypad[next]);
      if(numericPaths.ContainsKey(hash))
      {
        if (newPath.Length < numericPaths[hash][0].Length)
        { 
          numericPaths[hash].Clear();
          numericPaths[hash].Add(newPath);
        }
        else if (newPath.Length <= numericPaths[hash][0].Length)
          numericPaths[hash].Add(newPath);
      }
      else
        numericPaths.Add(hash, new List<string>() { newPath });
    }
  }
}

var arrowPaths = new Dictionary<(char, char), List<string>>();

foreach (var dk in directionalKeypad)
{
  Queue<(Point, string, HashSet<Point>)> queue = new Queue<(Point, string, HashSet<Point>)>();
  queue.Enqueue((dk.Key, "", new HashSet<Point>()));

  while (queue.Any())
  {
    var current = queue.Dequeue();

    foreach (var dir in directions)
    {
      var next = AddPoints(current.Item1, dir.Value);
      if (!directionalKeypad.ContainsKey(next) || current.Item3.Contains(next))
        continue;

      var newPath = current.Item2 + dir.Key;
      var nextPoints = new HashSet<Point>(current.Item3);
      nextPoints.Add(next);
      queue.Enqueue((next, newPath, nextPoints));

      var hash = (dk.Value, directionalKeypad[next]);
      if(hash.Item2 == hash.Item1)
      {
        arrowPaths[hash] = new List<string>(){ "A" };
        continue;
      }

      if (arrowPaths.ContainsKey(hash))
      {
        if (newPath.Length < arrowPaths[hash][0].Length)
        {
          arrowPaths[hash].Clear();
          arrowPaths[hash].Add(newPath);
        }
        else if (newPath.Length <= arrowPaths[hash][0].Length)
          arrowPaths[hash].Add(newPath);
      }
      else
        arrowPaths.Add(hash, new List<string>() { newPath });
    }
  }
}


string[] input = 
  {
  "964A",
  "140A",
  "413A",
  "670A",
  "593A"
  };
int sum = 0;
foreach (var n in input)
{
  var step1 = ComputeAllPaths(n, numericPaths);
  HashSet<string> step2 = new HashSet<string>();
  int minS2 = int.MaxValue;
  foreach (var path in step1)
  {
    foreach(var pathStep2 in ComputeAllPaths(path, arrowPaths))
    {
      if (minS2 > pathStep2.Length)
        minS2 = pathStep2.Length;
      step2.Add(pathStep2);
    }
  }

  step2 = step2.Where(s => s.Length == minS2).ToHashSet();

  int minS3 = int.MaxValue;
  HashSet<string> step3 = new HashSet<string>();
  foreach (var path in step2)
  {
    foreach (var pathStep3 in ComputeAllPaths(path, arrowPaths))
    {
      if(minS3 > pathStep3.Length)
        minS3 = pathStep3.Length;
      step3.Add(pathStep3);
    }
  }

  step3 = step3.Where(s => s.Length == minS3).ToHashSet();

  sum += minS3 * int.Parse(n.Substring(0, n.Length-1));
}

Console.Write(sum);


HashSet<string> ComputeAllPaths(string input, Dictionary<(char, char), List<string>> paths)
{
  var minLength = int.MaxValue;
  HashSet<string> res = new HashSet<string>();

  Stack<(int, string)> stack = new Stack<(int, string)>();
  stack.Push((0, ""));
  while (stack.Any())
  {
    var current = stack.Pop();
    int currentIndex = current.Item1;
    if (currentIndex == input.Length)
    {
      if(minLength < current.Item2.Length)
        continue;

      minLength = current.Item2.Length;      

      res.Add(current.Item2);
      continue;
    }

    char currentChar = input[currentIndex];
    char prevChar = currentIndex == 0 ? 'A' : input[currentIndex - 1];

    foreach (var next in paths[(prevChar, currentChar)])
    {
      if(prevChar == currentChar)
        stack.Push((currentIndex + 1, current.Item2 + 'A'));
      else
        stack.Push((currentIndex + 1, current.Item2 + next + 'A'));
    }
  }

  return res;
}

int Distance(Point p1, Point p2)
{
  return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
}

Point AddPoints(Point p1, Point p2)
{
  return new Point(p1.X + p2.X, p1.Y + p2.Y);
}