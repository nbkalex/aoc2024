using System;
using System.Diagnostics;

var input = File.ReadAllLines("in.txt");

var patterns = input[0].Split(", ").ToArray();
var patternsR = patterns.Select(p => string.Join("", p.Reverse())).ToArray();

var desired = input.Skip(2).ToList();

Dictionary<string, long> matchable = new Dictionary<string, long>();

foreach (var design in desired)
{
  var toCheck = new Stack<(string, List<string>)>();
  toCheck.Push((design, new List<string>()));
  while (toCheck.Any())
  {
    var current = toCheck.Pop();
    string currentDesign = current.Item1;

    if (currentDesign == "")
    {
      foreach(var d in current.Item2)
        matchable[d]++;

      continue;
    }

    if(matchable.ContainsKey(currentDesign))
    {
      foreach(var d in current.Item2)
        matchable[d] += matchable[currentDesign];

      continue;
    }

    if(!matchable.ContainsKey(currentDesign))
      matchable.Add(currentDesign, 0);

    foreach (var pattern in patterns)
    {
      if (currentDesign.StartsWith(pattern))
      {
        var newDesigns = new List<string>(current.Item2);
        newDesigns.Add(currentDesign);
        toCheck.Push((currentDesign.Substring(pattern.Length), newDesigns));
      }
    }
  }

  Console.WriteLine(design + " : " + matchable[design]);
}

Console.WriteLine(desired.Select(d => matchable[d]).Where(c => c > 0).Count());
Console.WriteLine(desired.Select(d => matchable[d]).Sum());

