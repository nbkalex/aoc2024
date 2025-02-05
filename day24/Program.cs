var input = File.ReadAllLines("in.txt").ToList();

int separatorIndex = input.IndexOf("");

var wires = input.Take(separatorIndex).Select(w => w.Split(": ")).ToDictionary(k => k[0], v => int.Parse(v[1]));

var gates = input.Skip(separatorIndex + 1).Select(g => g.Split(" -> ")).ToDictionary(k => k[1], v => v[0].Split(" "));

HashSet<string> allwires = new HashSet<string>();
foreach (var g in gates)
{
  allwires.Add(g.Value[0]);
  allwires.Add(g.Value[2]);
  allwires.Add(g.Key);
}

long x = GetNumber("x", wires);
long y = GetNumber("y", wires);
string sumBin = Convert.ToString(x + y, 2);

var firstRun = RunProgram(gates, "z11", "z11");

HashSet<string> validOutputs = new HashSet<string>();
foreach (var output in gates.Keys)
{
  foreach (var output2 in gates.Keys)
  {
    if (output == output2 || !gates.ContainsKey(output) || !gates.ContainsKey(output2) || output.StartsWith("z") || output2.StartsWith("z"))
      continue;

    var testGates = new Dictionary<string, string[]>(gates);
    var run = RunProgram(testGates, output, output2);
    if(run == null) continue;

    var vals0 = run.Keys.Where(v => run[v] == '0');
    var vals1 = run.Keys.Where(v => run[v] == '1');
    if (run.Count == 6 && vals0.Count() == 3)
    {
      var zip = vals0.Zip(vals1);
      foreach (var pair in zip)
        SwapWires(testGates, GetZName(pair.First), GetZName(pair.Second));

      var test = RunProgram(testGates, output, output2);
      if (test.Count != 0)
        continue;

      List<string> outputs = new List<string>(){ output, output2 };
      foreach(var o in run.Keys)
        outputs.Add(GetZName(o));
      outputs.Sort();

      validOutputs.Add(string.Join(',', outputs));
    }
  }
}

foreach(var valid in validOutputs)
  Console.WriteLine(valid);


Console.ReadKey();


string GetZName(int zIndex)
{
  return "z" + (zIndex < 10 ? "0" : "") + zIndex.ToString();
}

Dictionary<int, char> RunProgram(Dictionary<string, string[]> aGates, string swap1 = "", string swap2 = "")
{
  Dictionary<string, string[]> gates2 = new Dictionary<string, string[]>(aGates);
  var wires2 = new Dictionary<string, int>(wires);

  Dictionary<string, Func<string, string, int>> gatesDef = new Dictionary<string, Func<string, string, int>>()
{
  {"AND", (a,b) => wires2[a] & wires2[b] },
  {"OR", (a,b) => wires2[a] | wires2[b] },
  {"XOR", (a,b) => wires2[a] ^ wires2[b] }
};

  if(swap1 != "")
    SwapWires(gates2, swap1, swap2);

  while (wires2.Count != allwires.Count)
  {
    int revCount = wires2.Count;
    foreach (var g in gates2)
    {
      if (wires2.ContainsKey(g.Key) || !wires2.ContainsKey(g.Value[0]) || !wires2.ContainsKey(g.Value[2]))
        continue;

      wires2[g.Key] = gatesDef[g.Value[1]](g.Value[0], g.Value[2]);
    }

    if (wires2.Count == revCount)
      break;
  }

  Dictionary<int, char> result = new Dictionary<int, char>();

  long z = GetNumber("z", wires2);
  string zBin = Convert.ToString(z, 2);
  if (zBin.Length != sumBin.Length)
    return null;
  else
    for (int i = 0; i < sumBin.Length; i++)
      if (sumBin[i] != zBin[i])
      {
        result.Add(sumBin.Length - i - 1, sumBin[i]);
      }

  return result;
}


long GetNumber(string wireName, Dictionary<string, int> wires)
{
  var ordered = GetWires(wireName, wires);
  long n = 0;
  for (int i = 0; i < ordered.Length; i++)
    n += (long)Math.Pow(2, i) * ordered[i].Value;

  return n;
}

KeyValuePair<string, int>[] GetWires(string name, Dictionary<string, int> wires)
{
  return wires.Where(kvp => kvp.Key.StartsWith(name)).OrderBy(kvp => kvp.Key).ToArray();
}

void SwapWires(Dictionary<string, string[]> aGates, string wire1, string wire2)
{
  var temp = aGates[wire1];
  aGates[wire1] = aGates[wire2];
  aGates[wire2] = temp;
}
