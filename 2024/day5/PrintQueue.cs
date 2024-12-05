namespace CY2024;

public partial class Solutions
{
    public static void Day5_Part1()
    {
        var input = File.ReadAllLines("2024/day5/input.txt");
        // var input = File.ReadAllLines("2024/day5/sample.txt");

        var orderingInput = new List<string>();
        var pageNumberInput = new List<string>();

        bool beforeBreak = true;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                beforeBreak = false;
                continue;
            }

            if (beforeBreak)
                orderingInput.Add(line);
            else
                pageNumberInput.Add(line);
        }

        var orderingRules = new Dictionary<int, List<int>>();
        foreach (var line in orderingInput)
        {
            var rule = line.Split('|').Select(s => int.Parse(s)).ToArray();
            if (!orderingRules.TryGetValue(rule[0], out var list))
            {
                orderingRules[rule[0]] = [rule[1]];
            }
            else
            {
                orderingRules[rule[0]].Add(rule[1]);
            }
        }

        int sum = 0;
        foreach (var line in pageNumberInput)
        {
            var pageNumbers = line.Split(',').Select(s => int.Parse(s)).Select((n, i) => (n, i)).ToArray();
            var pageIndexLookup = pageNumbers.ToDictionary(p => p.n, p => p.i);

            bool isValid = true;
            foreach (var p in pageNumbers)
            {
                int page = p.n;
                int i = p.i;

                if (orderingRules.TryGetValue(page, out var rules))
                {
                    foreach (var pageNumAfter in rules)
                    {
                        // Validate that all rules are adhered to
                        if (pageIndexLookup.TryGetValue(pageNumAfter, out int pageAfterIndex))
                        {
                            // A page matches this rule. Compare it's index to ours.
                            if (pageAfterIndex < i)
                            {
                                // Rule violated
                                isValid = false;
                                break;
                            }
                        }
                    }
                }

                if (!isValid)
                    break;
            }

            if (isValid)
            {
                sum += pageNumbers[pageNumbers.Length / 2].n;
            }
        }

        Console.WriteLine($"Sum: {sum}");
    }
}