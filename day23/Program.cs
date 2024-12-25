var connections = File.ReadAllLines("in.txt").Select(l => l.Split("-")).ToArray();

Dictionary<string, HashSet<string>> pcsCon = new Dictionary<string, HashSet<string>>();



for (int i = 0; i < connections.Length; i++)
{
  string pc1 = connections[i][0];
  string pc2 = connections[i][1];

  if(!pcsCon.ContainsKey(pc1))
    pcsCon.Add(pc1, new HashSet<string>());
  if (!pcsCon.ContainsKey(pc2))
    pcsCon.Add(pc2, new HashSet<string>());

  pcsCon[pc1].Add(pc2);
  pcsCon[pc2].Add(pc1);
}


HashSet<string> parties = new HashSet<string>();
foreach(var c in pcsCon)
{
  List<(string, string)> pairs = new List<(string, string)>();
  for(int i = 0; i < c.Value.Count; i++)
    for (int j = i+1; j < c.Value.Count; j++)
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

HashSet<string> largestParty = new HashSet<string>();

foreach (var c in pcsCon)
{
  Queue<HashSet<string>> queue = new Queue<HashSet<string>>();
  queue.Enqueue((new HashSet<string>() { c.Key }));
  while(queue.Count > 0)
  {
    var current = queue.Dequeue();

    foreach (var pc in c.Value.Except(current))
    {
      if (current.Any(c => !pcsCon[c].Contains(pc)))
        continue;

      HashSet<string> newList = new HashSet<string>(current);
      newList.Add(pc);

      if (largestParty.Count < newList.Count)
        largestParty = newList;
      queue.Enqueue(newList);
    }
  }
}

Console.WriteLine();