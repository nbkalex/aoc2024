var input = File.ReadAllText("in.txt");

var keysAndLocks = input.Split("\r\n\r\n");

List<List<int>> keys = new List<List<int>>();
List<List<int>> locks = new List<List<int>>();

foreach (var kl in keysAndLocks)
{
  List<int> ints = new List<int>();
  var lines = kl.Split("\r\n");
  for(int i = 0;  i < lines[0].Length; i++)
    ints.Add(lines.Count(l => l[i] == '#')-1);

  bool isKey = lines[0].All(c => c == '#');
  if (isKey)
    keys.Add(ints);
  else
    locks.Add(ints);
}

Console.WriteLine(keys.Sum(k => locks.Count(l => Match(k,l))));

bool Match(List<int> kl1, List<int> kl2)
{
  return kl1.Zip(kl2).All(z => z.First + z.Second < 6);
}


Console.WriteLine();