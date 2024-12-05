using System.Collections;

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

    public static void Day5_Part2()
    {
        var input = File.ReadAllLines("2024/day5/input.txt");
        //var input = File.ReadAllLines("2024/day5/sample.txt");

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

            if (!isValid)
            {
                // Time to order the pages according to the rules.
                // Use a linked list to structure the pages
                // Loop through the linked list from front to back.
                // For each page, look backwards through the list
                // and determine if any pages behind you are violating a rule.
                // If so, move the current page directly behind the first violating page.
                // Repeat until all pages are in order.

                var orderedPages = new LinkedList<int>(pageNumbers.Select(x => x.n));
                var iNode = orderedPages.First;
                while (iNode != null)
                {
                    if (!orderingRules.ContainsKey(iNode.Value))
                    {
                        iNode = iNode.Next;
                        continue;
                    }

                    var rules = orderingRules[iNode.Value].ToHashSet();
                    var jNode = iNode.Previous;
                    LinkedListNode<int>? firstViolatingRuleNode = null;
                    while (jNode != null)
                    {
                        if (rules.Contains(jNode.Value))
                            firstViolatingRuleNode = jNode;

                        jNode = jNode.Previous;
                    }

                    var nodeToRemove = iNode;
                    iNode = iNode.Next;

                    if (firstViolatingRuleNode != null)
                    {
                        orderedPages.Remove(nodeToRemove);
                        orderedPages.AddBefore(firstViolatingRuleNode, nodeToRemove.Value);
                    }
                }

                var arr = orderedPages.ToArray();
                sum += arr[arr.Length / 2];
            }
        }

        Console.WriteLine($"Sum: {sum}");
    }
}