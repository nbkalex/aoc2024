using System.Drawing;
using System.Linq;
using System.Numerics;

var input = File.ReadAllLines("in.txt");

List<Machine> machines = new List<Machine>();
for(int i = 0; i < input.Length; i+=4)
{
  int[] aCoords =   input[i].Split(":")[1].Split(",").Select(a => int.Parse(a.Substring(2).Trim(' '))).ToArray();
  int[] bCoords = input[i+1].Split(":")[1].Split(",").Select(b => int.Parse(b.Substring(2).Trim(' '))).ToArray();
  int[] pCoords = input[i+2].Split(":")[1].Split(",").Select(b => int.Parse(b.Substring(3).Trim(' '))).ToArray();

  Machine machine = new Machine()
  {
    A = new Point(aCoords[0], aCoords[1]),
    B = new Point(bCoords[0], bCoords[1]),
    P = new BigPoint(10000000000000 + pCoords[0], 10000000000000 + pCoords[1])
  };

  machines.Add(machine);
}

var credits = machines.Select(m => GetCredits2(m)).ToArray();

BigInteger sum = 0;
foreach(var credit in credits)
  sum += credit;

Console.WriteLine(sum);

//int GetCredits(Machine m)
//{
//  int maxA = Math.Min(m.P.X / m.A.X, m.P.Y / m.A.Y);
//  int min = int.MaxValue;

//  for (int i = 0; i <= maxA; i++)
//  {
//    int currentX = i * m.A.X;
//    int currentY = i * m.A.Y;

//    int restX = m.P.X - currentX;
//    int restY = m.P.Y - currentY;

//    int bPressed = restX / m.B.X;

//    if (restX % m.B.X != 0 || restY % m.B.Y != 0 || bPressed != restY / m.B.Y)
//      continue;

//    min = Math.Min(min, 3 * i + bPressed);
//  }

//  return min != int.MaxValue ? min : 0;
//}

BigInteger GetCredits2(Machine m)
{
  BigInteger CbNum = (m.A.Y * m.P.X) - (m.P.Y * m.A.X);
  BigInteger CbDen = (m.A.Y * m.B.X) - (m.A.X * m.B.Y);
  if(CbNum % CbDen != 0)
    return 0;

  BigInteger Cb = CbNum / CbDen;

  BigInteger CaNum = m.P.X - (m.B.X * Cb);
  BigInteger CaDen =m.A.X;
  if(CaNum % CaDen != 0)
    return 0;

  BigInteger Ca = CaNum / CaDen;

  return Ca * 3 + Cb;
}


struct Machine
{
  public Point A, B;
  public BigPoint P;
}

struct BigPoint
{
  public BigInteger X, Y;
  public BigPoint(BigInteger x,  BigInteger y)
  {
    X = x;
    Y = y;
  }
}

