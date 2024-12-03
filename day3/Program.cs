using System.ComponentModel;

Tuple<int, int> GetMulInstr(string instr, ref int offset, ref bool stop)
{
  int nextMulIndex = instr.IndexOf("mul", offset);
  if (nextMulIndex == -1)
  {
    stop = true;
    return null;
  }

  offset = nextMulIndex + 1;

  try
  {
    int i = nextMulIndex + 3;

    if (instr[i] != '(')
      return null;

    int firstOpStart = ++i;
    int firstOpEnd = instr.IndexOf(',', i);
    if (firstOpEnd == -1)
      return null;

    int firstOp = 0;
    if (!int.TryParse(instr.Substring(firstOpStart, firstOpEnd - firstOpStart), out firstOp))
      return null;

    int secondOpStart = firstOpEnd + 1;
    int secondOpEnd = instr.IndexOf(')', secondOpStart + 1);
    if (secondOpEnd == -1)
      return null;

    int secondOp = 0;
    if (!int.TryParse(instr.Substring(secondOpStart, secondOpEnd - secondOpStart), out secondOp))
      return null;

    offset = secondOpEnd + 1;

    return new Tuple<int, int>(firstOp, secondOp);
  }
  catch
  {
    return null;
  }
}

var input = File.ReadAllText("in.txt");

List<Tuple<int, int>> list = new List<Tuple<int, int>>();
bool stop = false;
int offset = 0;
bool doInstr = true;
while (!stop)
{
  int switchIndex = input.IndexOf(doInstr ? "don't()" : "do()", offset);

  var mul = GetMulInstr(input, ref offset, ref stop);
  if(switchIndex != -1 && switchIndex < offset)
    doInstr = !doInstr;

  if (mul != null && doInstr)
    list.Add(mul);
}

Console.WriteLine(list.Sum(m => m.Item1 * m.Item2));

