using System.Diagnostics.CodeAnalysis;
using System.Text;

var input = File.ReadAllText("in.txt");

string layout = "";

List<string> layoutList = new List<string>();

char id = '0';
for (int i = 0; i < input.Length; i++)
{
  char toAdd = i % 2 == 0 ? id : '.';
  string chunk = string.Join("", Enumerable.Repeat(toAdd, int.Parse(input[i].ToString())));
  layout += chunk;

  if (i % 2 == 0)
    id++;

  layoutList.Add(chunk);
}

// Part 1
int notSpacesCount = layout.Count(c => c != '.');
string part1Layout = layout;
while(part1Layout.IndexOf('.') < notSpacesCount)
{
  StringBuilder sb = new StringBuilder(part1Layout);

  char toMove = part1Layout.Last(c => c != '.');
  int indexToMove = part1Layout.LastIndexOf(toMove);
  int indexToPlace = part1Layout.IndexOf('.');
  sb[indexToPlace] = toMove;
  sb[indexToMove] = '.';

  part1Layout = sb.ToString();
}
Console.WriteLine(ComputeSum(part1Layout));

List<string> newLayoutList = new List<string>(layoutList);

layoutList.Reverse();
foreach(var chunck in layoutList)
{
  if (chunck.Length == 0 || chunck[0] == '.')
    continue;

  int indexToMove = newLayoutList.IndexOf(chunck);
  newLayoutList[indexToMove] = newLayoutList[indexToMove].Replace(newLayoutList[indexToMove][0], '.');
  string? spaceToFit = newLayoutList.FirstOrDefault(c => c.Length >0 && c[0] == '.' && c.Length >= chunck.Length);
  if(spaceToFit == null)
    continue;

  int indexToFit = newLayoutList.IndexOf(spaceToFit);
  newLayoutList.Insert(indexToFit, chunck);
  newLayoutList[indexToFit +1] = newLayoutList[indexToFit + 1].Remove(0, chunck.Length);
}

Console.WriteLine(ComputeSum(string.Join("", newLayoutList)));

long ComputeSum(string aLayout)
{
  long sum = 0;
  for (int i = 0; i < aLayout.Length; i++)
  {
    if (aLayout[i] != '.')
      sum += i * int.Parse((aLayout[i] - '0').ToString());
  }

  return sum;
}