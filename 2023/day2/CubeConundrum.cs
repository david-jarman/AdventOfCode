using System.Text.RegularExpressions;

namespace CY2023;

public partial class Solutions
{
    public static void Day2_Part1()
    {
        var input = File.ReadAllLines("2023/day2/input.txt");
        int sum = 0;

        var gameIdRegex = new Regex("^Game ([0-9]+): (.*)$");
        var gameRegex = new Regex(@"([0-9]+) (\w+)");

        foreach (var line in input)
        {
            var gameIdMatch = gameIdRegex.Match(line);
            var gameId = int.Parse(gameIdMatch.Groups[1].Captures[0].Value);
            var gameSequence = gameIdMatch.Groups[2].Value;

            var games = gameSequence.Split(';').Select(s => s.Trim());
            bool isValid = true;
            foreach (var game in games)
            {
                var gameMatches = gameRegex.Matches(game);

                foreach (Match cubeInfo in gameMatches)
                {
                    int cubeCount = int.Parse(cubeInfo.Groups[1].Value);
                    string cubeColor = cubeInfo.Groups[2].Value;

                    int validCount = CountPerColor(cubeColor);

                    if (cubeCount > validCount)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (!isValid)
                    break;
            }

            sum += isValid ? gameId : 0;
        }

        Console.WriteLine($"Answer: {sum}");
    }

    private static int CountPerColor(string color)
    {
        return color switch
        {
            "red" => 12,
            "green" => 13,
            "blue" => 14,
            _ => throw new Exception($"Invalid color: {color}")
        };
    }
}