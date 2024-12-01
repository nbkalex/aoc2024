var input = File.ReadAllLines("in.txt").Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries));
var list1 = input.Select(i => int.Parse(i[0])).OrderBy(n => n);
var list2 = input.Select(i => int.Parse(i[1])).OrderBy(n => n);

// Part 1
Console.WriteLine(list1.Zip(list2).Sum(z => Math.Abs(z.First - z.Second)));

// Part 2
var countMap = list2.GroupBy(n => n).ToDictionary(n => n.Key, n => n.Count());
Console.WriteLine(list1.Sum(n => n * (countMap.ContainsKey(n) ? countMap[n] : 0)));
