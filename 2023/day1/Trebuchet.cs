using System.Text.RegularExpressions;

namespace CY2023;

public partial class Solutions
{
    public static void Day1_Part1()
    {
        var input = File.ReadAllLines("2023/day1/input.txt");

        int sum = 0;
        foreach (var line in input)
        {
            int firstNum = -1, secondNum = -1;

            foreach ((char c, int i) in line.Select((it, id) => (it, id)))
            {
                if (char.IsAsciiDigit(c))
                {
                    if (firstNum == -1)
                    {
                        firstNum = secondNum = c;
                    }
                    else
                    {
                        secondNum = c;
                    }
                }

                if (i == line.Length - 1)
                    sum += (10 * (firstNum - '0')) + (secondNum - '0');
            }
        }

        Console.WriteLine($"Answer: {sum}");
    }

    public static void Day1_Part2()
    {
        var input = File.ReadAllLines("2023/day1/input.txt");
        //var input = File.ReadAllLines("2023/day1/sample.txt");

        Regex regex = new Regex("([1-9]|one|two|three|four|five|six|seven|eight|nine)", RegexOptions.Compiled);

        uint sum = 0;
        foreach (var line in input)
        {
            var matches = regex.Matches(line);
            uint num1 = ConvertToInt(matches.First().Captures.Single().Value);
            uint num2 = ConvertToInt(matches.Last().Captures.Single().Value);

            sum += (num1 * 10) + num2;
            Console.WriteLine($"{line} = {num1} {num2}. Current sum: {sum}");
        }

        Console.WriteLine($"Answer: {sum}");
    }

    private static uint ConvertToInt(string s)
    {
        if (uint.TryParse(s, out uint i))
        {
            if (s.Length > 1) throw new Exception();
            return i;
        }

        return s switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => throw new Exception()
        };
    }
}