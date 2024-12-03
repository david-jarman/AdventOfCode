namespace CY2024;

public partial class Solutions
{
    public static void Day2_Part1()
    {
        var input = File.ReadAllLines("2024/day2/input.txt");

        int sum = 0;
        foreach (var line in input)
        {
            int[] nums = line.Split(' ').Select(s => int.Parse(s)).ToArray();

            sum += IsValid(nums) ? 1 : 0;
        }

        Console.WriteLine($"Answer: {sum}");
    }

    private static bool IsValid(int[] vals)
    {
        // Rules:
        // The levels are either all increasing or all decreasing.
        // Any two adjacent levels differ by at least one and at most three.

        // 1 2 3 4 5

        int prev = vals[0];
        bool increasing = vals[1] - vals[0] > 0;
        for (int i = 1; i < vals.Length; i++)
        {
            int cur = vals[i];
            int diff = cur - prev;

            if (diff < 0 && increasing)
                return false;

            if (diff > 0 && !increasing)
                return false;

            int abs_diff = Math.Abs(diff);

            if (abs_diff < 1 || abs_diff > 3)
                return false;

            prev = cur;
        }

        return true;
    }
}