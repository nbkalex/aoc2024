static bool IsSafe(IEnumerable<int> values)
{
  return values.Zip(values.Skip(1)).All(n => n.First - n.Second > 0 && n.First - n.Second < 4)
      || values.Zip(values.Skip(1)).All(n => n.First - n.Second < 0 && n.First - n.Second > -4);
}

var input = File.ReadAllLines("in.txt").Select(l => l.Split(" ").Select(c => int.Parse(c)));
int count = 0;
foreach(var line in input)
{
  if (IsSafe(line))
  {
    Console.WriteLine(string.Join(' ', line));
    count++;
    continue;
  }

  for(int i = 0; i < line.Count(); i++)
  {
    var newLine = new List<int>(line);
    newLine.RemoveAt(i);

    if(IsSafe(newLine))
    {
      Console.WriteLine(string.Join(' ', newLine));

      count++;
      break;
    }
  }
}

Console.WriteLine(count);
