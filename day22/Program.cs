using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;

var input = File.ReadAllLines("in.txt").Select(l => long.Parse(l)).ToList();

List<long> secrets = new List<long>();
List<List<long>> allSeq = new List<List<long>>();
List<List<long>> allPrices = new List<List<long>>();

foreach (var n in input)
{
  long current = n;
  var prices = new List<long>();
  var sequences = new List<long>();

  for (int i = 0; i < 2000; i++)
  {
    prices.Add(current % 10);

    long current2 = current;
    current *= 64;
    current ^= current2;
    current %= 16777216;

    current2 = current;
    current /= 32;
    current ^= current2;
    current %= 16777216;

    current2 = current;
    current *= 2048;
    current ^= current2;
    current %= 16777216;

    sequences.Add(current % 10 - prices.Last());
  }

  allSeq.Add(sequences);
  allPrices.Add(prices);
  secrets.Add(current);
}

List<string> allSeqStr = new List<string>();
List<Dictionary<string, long>> vals = new List<Dictionary<string, long>>();
for (int iSeq = 0; iSeq < allSeq.Count; iSeq++)
{
  Dictionary<string, long> seqVals = new Dictionary<string, long>();

  var seqs = allSeq[iSeq];  
  for (int i = 0; i < 1996; i++)
  {
    string toCheck = GetSequence(seqs, i);
    if(!seqVals.ContainsKey(toCheck))
      seqVals[toCheck] = allPrices[iSeq][i + 4];
  }

  vals.Add(seqVals);
}

long max = 0;

HashSet<string> checkedSeqs = new HashSet<string>();

// collect all sequences
HashSet<string> sequences4 = new HashSet<string>();
for (int iSeq = 0; iSeq < allSeq.Count; iSeq++)
{
  var seqs = allSeq[iSeq];

  for (int i = 0; i < 1997; i++)
  {
    string toCheck = "," + seqs[i].ToString() + "," + seqs[i + 1] + "," + seqs[i + 2] + "," + seqs[i + 3];
    sequences4.Add(toCheck);
  }
}

int count = 0;
foreach (string seq4 in sequences4)
{
  count++;
  long sum = 0;
  for (int iSeq = 0; iSeq < allSeq.Count; iSeq++)
  {
    var seqs = allSeq[iSeq];
    
    if (vals[iSeq].ContainsKey(seq4))
      sum += vals[iSeq][seq4];
  }

  if (sum > max)
    max = sum;
}


Console.WriteLine(secrets.Sum());
Console.WriteLine(max);

string GetSequence(List<long> seqs, int i)
{
  return "," + seqs[i].ToString() + "," + seqs[i + 1] + "," + seqs[i + 2] + "," + seqs[i + 3];
}