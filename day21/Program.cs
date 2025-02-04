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

foreach(var key in allPaths.Keys)
{ 
  if(key.Item1 == key.Item2)
    allPaths[key] = new HashSet<string>(){ "A" };    
}

string[] input =
  {
  "964A",
  "140A",
  "413A",
  "670A",
  "593A",
  };


long sum = 0;
foreach (var n in input)
{
  sum += int.Parse(n.Substring(0, n.Length-1)) * ComputeAllPaths2("A" + n, 25);
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
        newPath = newPath + "A";
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

long ComputeAllPaths2(string input, int steps)
{
  Dictionary<((char, char), int), long> mins = new Dictionary<((char, char), int), long>();
  foreach(var path in allPaths)
    mins[(path.Key, steps)] = path.Value.First().Length;


  Stack<((char, char), int)> stack = new Stack<((char, char), int)>();
  var inputPairs = input.Zip(input.Skip(1));
  foreach (var pair in inputPairs)
    stack.Push((pair, 0));

  while (stack.Any())
  {
    var current = stack.Pop();

    var currentPair = current.Item1;
    int currentStep = current.Item2;

    foreach (var p in allPaths[current.Item1])
    {
      string newPath = "A" + p;
      int nextStep = currentStep + 1;
      
      var zip = newPath.Zip(newPath.Skip(1)).ToArray();
      var missingPairs = zip.Where(z => !mins.ContainsKey((z, nextStep))).ToArray();
      if(missingPairs.Count() == 0)
      {
        long newCost = zip.Sum(z => mins[(z, nextStep)]);
        if (!mins.ContainsKey(current))
          mins[current] = newCost;
        if (mins[current] > newCost)
          mins[current] = newCost;
      }
      else
      {
        stack.Push(current);
        foreach (var pair in missingPairs)
          stack.Push((pair, nextStep));
      }
    }
  }
  long res = inputPairs.Sum(p => mins[(p, 0)]);
  Console.WriteLine(res);

  return res;
}

Point AddPoints(Point p1, Point p2)
{
  return new Point(p1.X + p2.X, p1.Y + p2.Y);
}