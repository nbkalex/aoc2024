using System.Collections.Frozen;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
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

var allPaths = new Dictionary<(char, char), HashSet<string>>();

ComputePaths(numericKeypad);
ComputePaths(directionalKeypad);

string[] input = 
  {
  "964A",
  "140A",
  "413A",
  "670A",
  "593A"
  };


long sum = 0;
foreach (var n in input)
{
  HashSet<string> nextStep = new HashSet<string>(){n };
  long min = 0;
  for(int i = 0; i < 3; i++)
  {
    nextStep = ComputeStep(nextStep);
    min = nextStep.First().Length;
  }

  sum += min * int.Parse(n.Substring(0, n.Length-1));
}

Console.Write(sum);

void ComputePaths(Dictionary<Point, char> keypad)
{
  foreach (var nk in keypad)
  {
    Queue<(Point, string, HashSet<Point>)> queue = new Queue<(Point, string, HashSet<Point>)>();
    queue.Enqueue((nk.Key, "", new HashSet<Point>()));

    while (queue.Any())
    {
      var current = queue.Dequeue();

      foreach (var dir in directions)
      {
        var next = AddPoints(current.Item1, dir.Value);
        if (!keypad.ContainsKey(next) || current.Item3.Contains(next))
          continue;

        var newPath = current.Item2 + dir.Key;
        var nextPoints = new HashSet<Point>(current.Item3);
        nextPoints.Add(next);
        queue.Enqueue((next, newPath, nextPoints));

        var hash = (nk.Value, keypad[next]);
        if (allPaths.ContainsKey(hash))
        {
          if (newPath.Length < allPaths[hash].First().Length)
          {
            allPaths[hash].Clear();
            allPaths[hash].Add(newPath);
          }
          else if (newPath.Length <= allPaths[hash].First().Length)
            allPaths[hash].Add(newPath);
        }
        else
          allPaths.Add(hash, new HashSet<string>() { newPath });
      }
    }
  }
}

HashSet<string> ComputeStep(HashSet<string> prevStep)
{
  long minS3 = long.MaxValue;
  HashSet<string> step3 = new HashSet<string>();
  foreach (var path in prevStep)
  {
    foreach (var pathStep3 in ComputeAllPaths2(path))
    {
      if (minS3 > pathStep3.Length)
      {
        minS3 = pathStep3.Length;
        step3.Clear();
      }
      if (minS3 == pathStep3.Length)
        step3.Add(pathStep3);
    }
  }

  return step3;
}


HashSet<string> ComputeAllPaths(string input)
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

    foreach (var next in allPaths[(prevChar, currentChar)])
    {
      if(prevChar == currentChar)
        stack.Push((currentIndex + 1, current.Item2 + 'A'));
      else
        stack.Push((currentIndex + 1, current.Item2 + next + 'A'));
    }
  }

  return res;
}

HashSet<string> ComputeAllPaths2(string input)
{
  Dictionary<(int,int),int> mins = new Dictionary<(int, int), int>();

  Stack<(string,int, int)> stack = new Stack<(string, int, int)>();
  stack.Push((input, 0, 0));

  while(stack.Any())
  {
    var current = stack.Pop();
    string seq = current.Item1;
    int lvl = current.Item2;
    int count = current.Item3;

    foreach(var pair in seq.Zip(seq.Skip(1)))
    {
      int minPath = 0;

      char from = pair.First;
      char to = pair.Second;
      foreach(var path in allPaths[(from, to)])
      {
        int newCount = count + path.Length;
        stack.Push((path, newCount, lvl+1));
      }
    }
  }

  return null;
}

int Distance(Point p1, Point p2)
{
  return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
}

Point AddPoints(Point p1, Point p2)
{
  return new Point(p1.X + p2.X, p1.Y + p2.Y);
}