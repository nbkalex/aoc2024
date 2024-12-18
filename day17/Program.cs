using System.Diagnostics;

var input = File.ReadAllLines("in.txt");

long ra = long.Parse(input[0].Split(": ")[1]);
long rb = long.Parse(input[1].Split(": ")[1]);
long rc = long.Parse(input[2].Split(": ")[1]);

long ip = 0;

var program = input[4].Split(": ")[1].Split(",").Select(long.Parse).ToList();

var combo = new Dictionary<long, Func<long>>()
{
  {0, () => 0 },
  {1, () => 1 },
  {2, () => 2 },
  {3, () => 3 },
  {4, () => ra },
  {5, () => rb },
  {6, () => rc },
  {7, () => throw new Exception() }
};

List<long> output = new List<long>();

var instructions = new Dictionary<long, Instruction>()
{
  {0, (op) => ra /= (long)Math.Pow(2,combo[op]()) },
  {1, (op) => rb ^= op },
  {2, (op) => rb = combo[op]() % 8 },
  {3, (op) => { if(ra != 0) ip = op; } },
  {4, (op) => rb ^= rc },
  {5, (op) => output.Add(combo[op]()%8) },
  {6, (op) => rb = ra / (long)Math.Pow(2,combo[op]()) },
  {7, (op) => rc = ra / (long)Math.Pow(2,combo[op]()) },
};

long maxCount = 0;
long step = 0;
for (long i = 1; i < long.MaxValue; i += (long)Math.Pow(8,step))
{
  output.Clear();
  ra = i;
  rb = 0;
  rc = 0;
  ip = 0;

  bool stop = false;

  while (ra != 0)
  {
    output.Add(((ra % 8) ^ 1 ^ (long)(ra / Math.Pow(2, (ra % 8) ^ 1)) ^ 4) % 8);
    if (output[output.Count - 1] != program[output.Count - 1])
    {
      stop = true;
      break;
    }

    ra /= 8;

    if (step < output.Count - 4)
      step = output.Count - 4;

    if (maxCount < output.Count)
    {
      Console.WriteLine(i + " " + string.Join(',', output));
      maxCount = output.Count;
    }
  }

  if (stop)
    continue;

  if (output.SequenceEqual(program))
  {
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine(i.ToString() + " - " + string.Join(',', output));
    break;
  }
}

delegate void Instruction(long aOperand);