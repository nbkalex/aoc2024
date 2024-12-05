using System.Collections.Generic;

var lines = File.ReadLines("in.txt").ToList();
int splitIndex = lines.FindIndex(l => !l.Any());
var ordering = lines.Take(splitIndex).Select(o => o.Split("|").Select(t => int.Parse(t)).ToArray());

Dictionary<int, HashSet<int>> ordering1 = new Dictionary<int, HashSet<int>>();
foreach (var o in ordering)
{
  if (ordering1.ContainsKey(o[0]))
    ordering1[o[0]].Add(o[1]);
  else
    ordering1.Add(o[0], new HashSet<int>() { o[1] });
}

var updates = lines.Skip(splitIndex + 1).Select(o => o.Split(",").Select(t => int.Parse(t)).ToArray());

List<IEnumerable<int>> correctUpdates = new List<IEnumerable<int>>();
List<List<int>> incorrectUpdates = new List<List<int>>();

foreach (var update in updates)
{
  bool correct = true;
  for (int i = 0; i < update.Length; i++)
  {
    int current = update[i];
    if (ordering1.ContainsKey(current))
    {
      if (!update.Take(i).All(p => !ordering1[current].Contains(p)))
      {
        correct = false;
        break;
      }
    }
  }

  if (correct)
    correctUpdates.Add(update);
  else
  {
    var list = update.ToList();
    list.Sort((n1, n2) => !ordering1.ContainsKey(n1) || !ordering1[n1].Contains(n2) ? -1 : 1);
    incorrectUpdates.Add(list);
  }
}

Console.WriteLine(correctUpdates.Select(c => c.ElementAt(c.Count() / 2)).Sum());
Console.WriteLine(incorrectUpdates.Select(c => c.ElementAt(c.Count() / 2)).Sum());
