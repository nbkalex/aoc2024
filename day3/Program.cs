using System.Text.RegularExpressions;
string input = File.ReadAllText("in.txt");
var matches = Regex.Matches(input, "mul\\(\\d+,\\d+\\)").Select(m => (Match)m).OrderBy(m => m.Index);
var mul_ops = matches.Select(m => m.Value.Substring(4, m.Length - 5).Split(",").Select(n => int.Parse(n)));

int sum = 0;
foreach(var match in matches)
{	
	string current = input.Substring(0, match.Index);

	int indexDo = current.LastIndexOf("do()");
	int indexDont = current.LastIndexOf("don't()");

	if(indexDo >= indexDont)
		sum += match.Value.Substring(4, match.Length - 5).Split(",").Select(n => int.Parse(n)).Aggregate((a, b) => a * b);
}

Console.WriteLine(mul_ops.Sum(ops => ops.Aggregate((a, b) => a * b)));
Console.WriteLine(sum);
