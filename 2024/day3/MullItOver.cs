using System.Text.RegularExpressions;

namespace CY2024;

public partial class Solutions
{
    // https://adventofcode.com/2024/day/3
    public static void Day3_Part1()
    {
        var input = File.ReadAllText("2024/day3/input.txt");

        Regex regex = new Regex(@"mul\(([0-9]{1,3}),([0-9]{1,3})\)", RegexOptions.Compiled);

        MatchCollection matches = regex.Matches(input);

        int sum = 0;
        foreach (Match match in matches)
        {
            int val1 = int.Parse(match.Groups[1].Value);
            int val2 = int.Parse(match.Groups[2].Value);

            sum += val1 * val2;
        }

        Console.WriteLine($"Answer: {sum}");
    }
}