var input = File.ReadAllLines("in.txt").ToList();

int separatorIndex = input.IndexOf("");

var wires = input.Take(separatorIndex).Select(w => w.Split(": ")).ToDictionary(k => k[0], v => int.Parse(v[1]));

var gates = input.Skip(separatorIndex + 1).Select(g => g.Split(" -> ")).ToDictionary(k => k[1], v => v[0].Split(" "));

var gatesDef = new Dictionary<string, Func<int, int, int>>()
  {
    {"AND", (a,b) => a & b },
    {"OR", (a,b) => a | b },
    {"XOR", (a,b) => a ^ b }
  };

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

var res = RunProgram(gates);

var zOutputs = gates.Keys.Where(g => g.StartsWith("z")).OrderBy(g => g).ToArray();
Dictionary<string, int> unfolded = new Dictionary<string, int>(wires);
List<string> unfoldedStr = new List<string>();
foreach (var z in zOutputs)
{
  Dictionary<string, HashSet<string>> operands = new Dictionary<string, HashSet<string>>();
  string val = z;

  Stack<string> stack = new Stack<string>();
  stack.Push(z);
  while (stack.Any())
  {
    var current = stack.Pop();

    string op1 = gates[current][0];
    string op2 = gates[current][2];
    string instr = gates[current][1];

    if (!unfolded.ContainsKey(op1) || !unfolded.ContainsKey(op2))
    { 
      stack.Push(current);
      if (!unfolded.ContainsKey(op1))
        stack.Push(op1);
      if (!unfolded.ContainsKey(op2))
        stack.Push(op2);
    }
    else
      unfolded[current] = gatesDef[instr](unfolded[op1], unfolded[op2]);
  }
}

List<string> swaps = new List<string>()
{
  "vhm", "z14",
  "cnk", "qwf",
  "mps", "z27",
  "msq", "z39",
};
swaps.Sort();

Console.WriteLine(string.Join(',', swaps));

SwapWires(gates, "vhm", "z14");
SwapWires(gates, "cnk", "qwf");
SwapWires(gates, "mps", "z27");
SwapWires(gates, "msq", "z39");

var run = RunProgram(gates);


Console.ReadKey();


string GetZName(int zIndex)
{
  return "z" + (zIndex < 10 ? "0" : "") + zIndex.ToString();
}

Dictionary<int, char> RunProgram(Dictionary<string, string[]> aGates)
{
  Dictionary<string, string[]> gates2 = new Dictionary<string, string[]>(aGates);
  var wires2 = new Dictionary<string, int>(wires);

  Dictionary<string, Func<string, string, int>> gatesDef = new Dictionary<string, Func<string, string, int>>()
  {
    {"AND", (a,b) => wires2[a] & wires2[b] },
    {"OR", (a,b) => wires2[a] | wires2[b] },
    {"XOR", (a,b) => wires2[a] ^ wires2[b] }
  };


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
