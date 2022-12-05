/*
// Problem 1, part 1
// https://adventofcode.com/2021/day/1#part1

var input = File.ReadAllLines("input.txt");

int prev = -1;
int numTimesIncreased = 0;

foreach (string depth in input)
{
    if (string.IsNullOrWhiteSpace(depth))
    {
        Console.WriteLine("Discarding empty line");
        continue;
    }

    int depth_i = int.Parse(depth);

    if (prev != -1 && depth_i > prev)
    {
        Console.WriteLine($"Cur: {depth_i}, prev: {prev}");
        numTimesIncreased++;
    }

    prev = depth_i;
}

Console.WriteLine($"Anser: {numTimesIncreased}");
*/

/*
// Problem 1, part 2
// https://adventofcode.com/2021/day/1#part2
var input = File.ReadAllLines("input.txt")
    .Where(s => !string.IsNullOrWhiteSpace(s))
    .Select(s => int.Parse(s))
    .ToArray();

int iter = 1;
int end = input.Length - 2;

int count = 0;

while (iter < end)
{
    int A = input[iter - 1];
    int B = input[iter + 2];

    if (B > A) count++;

    iter++;
}

Console.WriteLine($"count: {count}");
*/
