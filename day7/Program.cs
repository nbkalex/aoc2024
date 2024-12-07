using System.Numerics;

var input = File.ReadAllLines("in.txt").Select(l => l.Split(":")).Select(l => (l[0], l[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => long.Parse(n)).ToArray())); ;

BigInteger sum = 0;
foreach(var line in input)
{
  long value = long.Parse(line.Item1);
  long[] operands = line.Item2;

  Queue<(long,int)> toCheck = new Queue<(long,int)> ();
  toCheck.Enqueue((operands[0],0));
  while(toCheck.Any())
  {
    var current = toCheck.Dequeue();

    if (current.Item1 == value && current.Item2 == operands.Length - 1)
    {
      sum += value;
      break;
    }

    if (current.Item2 == operands.Length-1 || current.Item2 > value)
      continue;

    int nextOpIndex = current.Item2 + 1;
    long nextOp = operands[nextOpIndex];

    toCheck.Enqueue((current.Item1 + nextOp, nextOpIndex));
    toCheck.Enqueue((current.Item1 * nextOp, nextOpIndex));
    toCheck.Enqueue((current.Item1 * (long)Math.Pow(10, nextOp.ToString().Length) + nextOp, nextOpIndex));
  }

}

Console.WriteLine(sum);
