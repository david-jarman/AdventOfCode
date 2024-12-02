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
}