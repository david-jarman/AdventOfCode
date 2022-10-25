using System.Text;

namespace problem_solving;

public class Polymerization
{
    public void Solve_Part1()
    {
        string fileName = "day14/input.txt";
        string[] contents = File.ReadAllLines(fileName);

        string template = contents[0];
        string[] rules = contents[2..^0];

        var pairLookup = new Dictionary<string, string>();
        foreach (string rule in rules)
        {
            var split = rule.Split(" -> ");
            pairLookup[split[0]] = split[1];
        }

        const int steps = 10;
        for (int i = 0; i < steps; i++)
        {
            var sb = new StringBuilder();
            int pairCount = template.Length - 1;
            for (int j = 0; j < pairCount; j++)
            {
                Range pairRange = j..(j+2);
                string pair = template[pairRange];

                string insert = pairLookup[pair];

                sb.Append($"{pair[0]}{insert}");
            }

            // add final char
            sb.Append(template[^1]);

            template = sb.ToString();
        }

        Dictionary<char, int> elements = new Dictionary<char, int>();
        foreach (char c in template)
        {
            if (!elements.ContainsKey(c))
            {
                elements[c] = 1;
            }
            else
            {
                elements[c]++;
            }
        }

        int max = elements.Max(kvp => kvp.Value);
        int min = elements.Min(kvp => kvp.Value);

        Console.WriteLine($"answer: {max-min}");

    }

    public void Solve_Part2()
    {
        string fileName = "day14/input.txt";
        string[] contents = File.ReadAllLines(fileName);

        string template = contents[0];
        string[] rules = contents[2..^0];

        var pairLookup = new Dictionary<string, string>();
        Dictionary<string, long> pairCounts = new Dictionary<string, long>();
        Dictionary<char, long> letterCount = new Dictionary<char, long>();
        foreach (string rule in rules)
        {
            var split = rule.Split(" -> ");
            pairLookup[split[0]] = split[1];
            pairCounts[split[0]] = 0;
        }

        const int steps = 40;

        // set up pair counts with initial template
        int pairCount = template.Length - 1;
        for (int j = 0; j < pairCount; j++)
        {
            Range pairRange = j..(j+2);
            string pair = template[pairRange];

            pairCounts[pair]++;
        }

        // start counting individual letters
        foreach (char c in template)
        {
            if (!letterCount.ContainsKey(c))
            {
                letterCount[c] = 1;
            }
            else
            {
                letterCount[c]++;
            }
        }

        for (int i = 0; i < steps; i++)
        {
            var pairLoop = pairCounts.Where(p => p.Value > 0).ToList();
            var pairCountsCopy = DeepCopyCounts(pairCounts);

            foreach (var pair in pairLoop)
            {
                string insert = pairLookup[pair.Key];
                string newPair1 = $"{pair.Key[0]}{insert}";
                string newPair2 = $"{insert}{pair.Key[1]}";
                long currentPairCount = pairCounts[pair.Key];

                char c1 = insert[0];
                if (!letterCount.ContainsKey(c1))
                {
                    letterCount[c1] = currentPairCount;
                }
                else
                {
                    // increase letter count by number of pairs it applies to
                    letterCount[c1] += currentPairCount;
                }

                // Original pair is broken up, so decrease
                pairCountsCopy[pair.Key] -= currentPairCount;

                pairCountsCopy[newPair1] += currentPairCount;
                pairCountsCopy[newPair2] += currentPairCount;
            }

            pairCounts = pairCountsCopy;
        }

        long min = letterCount.Min(kvp => kvp.Value);
        long max = letterCount.Max(kvp => kvp.Value);

        Console.WriteLine($"answer: {max-min}");
    }

    private Dictionary<string, long> DeepCopyCounts(Dictionary<string, long> original)
    {
        var copy = new Dictionary<string, long>(original.Count);

        foreach (var kvp in original)
        {
            copy[kvp.Key] = kvp.Value;
        }

        return copy;
    }
}