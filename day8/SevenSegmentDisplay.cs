namespace problem_solving;

public class SevenSegmentDisplay
{
    public void Solve()
    {
        var inputs = ParseInput(false);

        int countOfUniqueDigits = 0;
        foreach (string[] input in inputs.digitOutputs)
        {
            foreach (string digit in input)
            {
                if (digit.Length == 2 || digit.Length == 4 || digit.Length == 3 || digit.Length == 7)
                {
                    countOfUniqueDigits++;
                }
            }
        }

        Console.WriteLine($"Count: {countOfUniqueDigits}");
    }

    private (string[][] signalPatterns, string[][] digitOutputs) ParseInput(bool useTestInput)
    {
        string fileName = useTestInput ? "day8/test_input.txt" : "day8/input.txt";
        string[] lines = File.ReadAllLines(fileName)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

        string[][] signalPatterns = new string[lines.Length][];
        string[][] digitOutputs = new string[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split('|');
            signalPatterns[i] = line[0].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            digitOutputs[i] = line[1].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        }

        return (signalPatterns, digitOutputs);
    }
}
