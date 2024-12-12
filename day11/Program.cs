using System.Text;

List<long> stones = File.ReadAllText("in.txt").Split(" ").Select(s => long.Parse(s)).ToList();

List<long> stones25 = GetStones(25, stones);
Console.WriteLine(stones25.Count);


List<long> stones38 = GetStones(38, stones);

HashSet<long> distinctStones38 = new HashSet<long>(stones38);

Dictionary<long, int> distinctStones38Count = new Dictionary<long, int>();

int i = 0;
foreach (long stone in distinctStones38)
{
  i++;
  distinctStones38Count.Add(stone, GetStones(37, new List<long>() { stone }).Count);
}


Console.WriteLine(stones38.Sum(s => (long)distinctStones38Count[s]));

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

List<long> GetStones(int steps, List<long> astones)
{
  for (int i = 0; i < steps; i++)
  {
    List<long> blinkStones = new List<long>();
    for (int j = 0; j < astones.Count; j++)
    {
      long stone = astones[j];
      int length = GetLength(stone);
      if (stone == 0)
        blinkStones.Add(1);
      else if (length % 2 == 0)
      {
        long left = stone / (long)(Math.Pow(10, (length / 2)));
        long right = stone % (long)(Math.Pow(10, (length / 2)));
        blinkStones.Add(left);
        blinkStones.Add(right);
      }
      else
      {
        blinkStones.Add(stone * 2024);
      }
    }

    astones = blinkStones;
  }

  return astones;
}