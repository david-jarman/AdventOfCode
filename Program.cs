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