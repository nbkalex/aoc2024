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

string[] allSeqStr = allSeq.Select(s => string.Join("", s)).ToArray();

long max = 0;

HashSet<string> checkedSeqs = new HashSet<string>();

for (int iSeq = 0; iSeq < allSeq.Count; iSeq++)
{
  var seqs = allSeq[iSeq];

  for (int i = 0; i < 1997; i++)
  {
    long sum = 0;
    //string toCheck = "-2,1,-1,3";
    string toCheck = seqs[i].ToString() + seqs[i + 1] + seqs[i + 2] + seqs[i + 3];
    if(!checkedSeqs.Add(toCheck))
      continue;

    for (int iSeq2 = 0; iSeq2 < allSeq.Count; iSeq2++)
    {
      var seqs2 = allSeq[iSeq2];
      for (int i2 = 0; i2 < 1997; i2++)
      {
        if (seqs[i] == seqs2[i2] &&
            seqs[i+1] == seqs2[i2+1] &&
            seqs[i+2] == seqs2[i2+2] &&
            seqs[i+3] == seqs2[i2+3] &&
            i2 + 4 < seqs2.Count)
        {
          sum += allPrices[iSeq2][i2+4];
          break;
        }
      }
    }

    if (sum > max)
      max = sum;
  }
}


Console.WriteLine(secrets.Sum());
Console.WriteLine(max);

// 2657 too high