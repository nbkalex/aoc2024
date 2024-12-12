using System.Runtime.InteropServices;
using System.Text;

var stones = File.ReadAllText("in.txt").Split(" ").Select(s => long.Parse(s)).ToList().ToDictionary(s => s, s => (long)1);

Console.WriteLine(GetStones2(25, stones).Sum(s => s.Value));
Console.WriteLine(GetStones2(75, stones).Sum(s => s.Value));

int GetLength(long stone)
{
  int length = 0;
  while (stone != 0)
  {
    length++;
    stone /= 10;
  }
  return length;
}

Dictionary<long, long> GetStones2(int steps, Dictionary<long,long> astones)
{
  for (int i = 0; i < steps; i++)
  {
    Dictionary<long, long> blinkStones = new Dictionary<long, long>();
    foreach(var stoneCount in astones)
    {
      long stone = stoneCount.Key;
      int length = GetLength(stone);
      List<long> newStones = new List<long>();
      if (stone == 0)
        newStones.Add(1);
      else if (length % 2 == 0)
      {
        long left = stone / (long)(Math.Pow(10, (length / 2)));
        long right = stone % (long)(Math.Pow(10, (length / 2)));
        newStones.Add(left);
        newStones.Add(right);
      }
      else
      {
        newStones.Add(stone * 2024);
      }

      foreach(long newStone in newStones)
      {
        if(blinkStones.ContainsKey(newStone))
          blinkStones[newStone] += stoneCount.Value;
        else
          blinkStones.Add(newStone, stoneCount.Value);
      }
    }

    astones = blinkStones;
  }

  return astones;
}