using System.Drawing;

var lines = File.ReadAllLines("in.txt");

var columns = Enumerable.Repeat("", lines[0].Length).ToList();
for (int i = 0; i < lines.Length; i++)
  for (int j = 0; j < lines[i].Length; j++)
    columns[j] += lines[i][j];

List<string> allLines = new List<string>(columns);
allLines.AddRange(lines);

int size = lines[0].Length;
for (int g = 0; g < size; g++)
{
  string diagBot = "";
  string diagTop = "";

  string diagBot2 = "";
  string diagTop2 = "";

  for (int i = 0; i + g < lines.Length; i++)
  {
    diagBot+=lines[i + g][i];
    diagTop += lines[i][i + g];

    diagBot2 += lines[i][size - i - g - 1];
    diagTop2 += lines[g + i][size - i - 1];
  }

  allLines.Add(diagBot);
  allLines.Add(diagBot2);
  
  if (g > 0)
  {
    allLines.Add(diagTop);
    allLines.Add(diagTop2);
  }
}

// PART 1
string toFind = "XMAS";
string toFindR = "SAMX";
Console.WriteLine(allLines.Select(l => l.Split(toFind).Length - 1).Sum()
                + allLines.Select(l => l.Split(toFindR).Length - 1).Sum());

// PART 2
HashSet<string> accepted = new HashSet<string>() { "MAS", "SAM"};
int count = 0;
for (int i = 0; i < lines.Length-2; i++)
{ 
  for (int j = 0; j < lines[i].Length-2; j++)
  {
    string diag1 = "";
    diag1 += lines[i][j].ToString() + lines[i+1][j+1].ToString() + lines[i+2][j+2].ToString();

    string diag2 = "";
    diag2 += lines[i][j+2].ToString() + lines[i + 1][j + 1].ToString() + lines[i + 2][j].ToString();

    if(accepted.Contains(diag1) && accepted.Contains(diag2))
      count++;
  }
}

//1825 too low
Console.WriteLine(count);
