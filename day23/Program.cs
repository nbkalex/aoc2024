using System.Collections.Generic;

var connections = File.ReadAllLines("in.txt").Select(l => l.Split("-")).ToArray();

Dictionary<string, HashSet<string>> pcsCon = new Dictionary<string, HashSet<string>>();



for (int i = 0; i < connections.Length; i++)
{
  string pc1 = connections[i][0];
  string pc2 = connections[i][1];

  if (!pcsCon.ContainsKey(pc1))
    pcsCon.Add(pc1, new HashSet<string>());
  if (!pcsCon.ContainsKey(pc2))
    pcsCon.Add(pc2, new HashSet<string>());

  pcsCon[pc1].Add(pc2);
  pcsCon[pc2].Add(pc1);
}


HashSet<string> parties = new HashSet<string>();
foreach (var c in pcsCon)
{
  List<(string, string)> pairs = new List<(string, string)>();
  for (int i = 0; i < c.Value.Count; i++)
    for (int j = i + 1; j < c.Value.Count; j++)
    {
      if (pcsCon[c.Value.ElementAt(i)].Contains(c.Value.ElementAt(j)))
      {
        List<string> party = new List<string>()
        {
          c.Key, c.Value.ElementAt(i), c.Value.ElementAt(j)
        };
        party.Sort();
        parties.Add(string.Join("", party));
      }
    }
}

Console.WriteLine(parties.Count(p => p[0] == 't' || p[2] == 't' || p[4] == 't'));

List<string> largestParty = new List<string>();

var maxCons = pcsCon.Max(pc => pc.Value.Count);

foreach (var c in pcsCon)
{
  foreach (var pc in c.Value)
  {
    List<string> tocheck = new List<string>(c.Value);
    tocheck.Remove(pc);

    bool all = true;

    foreach (var pc2 in tocheck)
    {
      foreach (var pc3 in tocheck)
      { 
        if (pc2 == pc3)
          continue;

        if (!pcsCon[pc2].Contains(pc3))
        {
          all = false;
          break;
        }
      }

      if(!all)
        break;
    }

    if(all)
    {
      tocheck.Add(c.Key);
      largestParty = tocheck;
    }
  }
}

largestParty.Sort();

Console.WriteLine(string.Join(",", largestParty));