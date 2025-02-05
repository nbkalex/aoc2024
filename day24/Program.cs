var input = File.ReadAllLines("in.txt").ToList();

int separatorIndex = input.IndexOf("");

var wires = input.Take(separatorIndex).Select(w => w.Split(": ")).ToDictionary(k => k[0], v => int.Parse(v[1]));

var gates = input.Skip(separatorIndex+1).Select(g =>  g.Split(" -> ")).ToDictionary(k => k[1], v => v[0].Split(" "));

HashSet<string> allwires = new HashSet<string>();
foreach (var g in gates)
{
  allwires.Add(g.Value[0]);
  allwires.Add(g.Value[2]);
  allwires.Add(g.Key);
}

Dictionary<string, Func<string, string, int>> gatesDef = new Dictionary<string, Func<string, string, int>>()
{
  {"AND", (a,b) => wires[a] & wires[b] },
  {"OR", (a,b) => wires[a] | wires[b] },
  {"XOR", (a,b) => wires[a] ^ wires[b] }
};

while(wires.Count != allwires.Count)
{
  foreach(var g in gates)
  {
    if(wires.ContainsKey(g.Key) || !wires.ContainsKey(g.Value[0]) || !wires.ContainsKey(g.Value[2]))
      continue;

    wires[g.Key] = gatesDef[g.Value[1]](g.Value[0], g.Value[2]);
  }
}

var xWires = GetWires("x");
var yWires = GetWires("y");
var zWires = GetWires("z");

List<string> wrongWires = new List<string>();
for(int i = 0; i< xWires.Length-1; i++)
{
  if ((xWires[i].Value & yWires[i].Value) != zWires[i].Value)
  {
    //wrongWires.Add(xWires[i].Key);
    //wrongWires.Add(yWires[i].Key);
    wrongWires.Add(zWires[i].Key);
  }
}


Console.WriteLine(GetNumber("x"));
Console.WriteLine(GetNumber("y"));
Console.WriteLine(GetNumber("z"));



long GetNumber(string wireName)
{
  var ordered = GetWires(wireName);
  long n = 0;
  for (int i = 0; i < ordered.Length; i++)
    n += (long)Math.Pow(2, i) * ordered[i].Value;

  return n;
}

KeyValuePair<string, int>[] GetWires(string name)
{
  return wires.Where(kvp => kvp.Key.StartsWith(name)).OrderBy(kvp => kvp.Key).ToArray();
}

