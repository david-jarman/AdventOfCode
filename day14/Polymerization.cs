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
}