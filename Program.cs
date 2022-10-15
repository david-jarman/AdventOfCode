/*
// Day 3, part 1
// https://adventofcode.com/2021/day/3#part1
var input = File.ReadAllLines("input.txt")
    .Where(s => !string.IsNullOrWhiteSpace(s))
    .ToArray();

int bitSize = 12;

int inputSize = input.Length;

int[] oneCounts = new int[bitSize];

foreach (string byte_s in input)
{
    for (int i = 0; i < bitSize; i++)
    {
        if (byte_s[i] == '1')
        {
            oneCounts[i]++;
        }
    }
}

int gamma_rate = 0;
int epsilon_rate = 0;
int inputSizeMajority = inputSize / 2;
for (int i = 0; i < bitSize; i++)
{
    int shift_left = bitSize - i - 1;
    if (oneCounts[i] > inputSizeMajority)
    {
        gamma_rate = gamma_rate | (1 << shift_left);
    }
    else
    {
        epsilon_rate = epsilon_rate | (1 << shift_left);
    }
}

Console.WriteLine($"Gamma rate: {gamma_rate}");
Console.WriteLine($"Epsilon rate: {epsilon_rate}");
Console.WriteLine($"Power rate: {epsilon_rate * gamma_rate}");

// 101110100101 = 2981
// 010001011010 = 1114
*/


// Day 3, part 2
// https://adventofcode.com/2021/day/3#part2
var input = File.ReadAllLines("input.txt")
    .Where(s => !string.IsNullOrWhiteSpace(s))
    .ToArray();

var oxyInput = input.ToList();
var co2Input = input.ToList();
int bitSize = 12;
//int bitSize = 5;
int inputSize = input.Length;

int oxygen = 0;
int co2_scrub = 0;

for (int i = 0; i < bitSize; i++)
{
    if (oxyInput.Count == 1)
    {
        oxygen = parseBin(oxyInput[0]);
        break;
    }

    int one_count = 0;
    int majority = (int)Math.Ceiling(oxyInput.Count / 2.0);
    List<string> ones = new List<string>();
    List<string> zeros = new List<string>();
    for (int j = 0; j < oxyInput.Count; j++)
    {
        if (oxyInput[j][i] == '1')
        {
            one_count++;
            ones.Add(oxyInput[j]);
        }
        else
        {
            zeros.Add(oxyInput[j]);
        }
    }

    if (one_count >= majority)
    {
        oxyInput = ones;
        oxygen |= (1 << (bitSize - i - 1));
    }
    else
    {
        oxyInput = zeros;
    }
}

// co2
for (int i = 0; i < bitSize; i++)
{
    if (co2Input.Count == 1)
    {
        co2_scrub = parseBin(co2Input[0]);
        break;
    }

    int one_count = 0;
    int majority = (int)Math.Ceiling(co2Input.Count / 2.0);
    List<string> ones = new List<string>();
    List<string> zeros = new List<string>();
    for (int j = 0; j < co2Input.Count; j++)
    {
        if (co2Input[j][i] == '1')
        {
            one_count++;
            ones.Add(co2Input[j]);
        }
        else
        {
            zeros.Add(co2Input[j]);
        }
    }

    if (one_count < majority)
    {
        co2Input = ones;
        co2_scrub |= (1 << (bitSize - i - 1));
    }
    else
    {
        co2Input = zeros;
    }
}

Console.WriteLine($"oxygen: {oxygen}");
Console.WriteLine($"co2: {co2_scrub}");
Console.WriteLine($"life support rating: {co2_scrub * oxygen}");

static int parseBin(string s)
{
    int val = 0;
    int size = s.Length - 1;
    int iter = 0;
    foreach (char c in s)
    {
        int shift = size - iter;
        if (c == '1')
        {
            val |= (1 << shift);
        }
        iter++;
    }

    return val;
}