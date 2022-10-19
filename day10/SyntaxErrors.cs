namespace problem_solving;

public class SyntaxErrors
{
    public void Solve_Day1()
    {
        var lines = GetInput(false);

        int syntaxErrorScore = 0;

        foreach (string line in lines)
        {
            Stack<char> chunkVerifier = new Stack<char>();
            foreach (char chonk in line)
            {
                if (openers.Contains(chonk))
                {
                    chunkVerifier.Push(chonk);
                }
                else if (chunkVerifier.Count > 0)
                {
                    char opener = chunkVerifier.Pop();
                    if (opener != matchingChunkers[chonk])
                    {
                        Console.WriteLine($"Expected {matchingChunkers[opener]} but got {chonk} instead");

                        syntaxErrorScore += ScoreBadSyntax(chonk);
                    }
                }
                else
                {
                    // incomplete chonk
                    Console.WriteLine($"Skipping incomplete line: {line}");
                    break;
                }
            }

            //Console.WriteLine($"Queue size at end: {chunkVerifier.Count}");
        }

        Console.WriteLine($"Score: {syntaxErrorScore}");
    }

    public void Solve_Day2()
    {
        var lines = GetInput(false);

        List<long> scores = new List<long>();

        foreach (string line in lines)
        {
            bool corruptedLine = false;
            Stack<char> chunkVerifier = new Stack<char>();
            foreach (char chonk in line)
            {
                if (openers.Contains(chonk))
                {
                    chunkVerifier.Push(chonk);
                }
                else if (chunkVerifier.Count > 0)
                {
                    char opener = chunkVerifier.Pop();
                    if (opener != matchingChunkers[chonk])
                    {
                        Console.WriteLine($"Corrupted line. Skipping...");
                        corruptedLine = true;
                        break;
                    }
                }
            }

            if (!corruptedLine)
            {
                long missingCharScore = 0;
                while (chunkVerifier.Count > 0)
                {
                    char orphanedOpener = chunkVerifier.Pop();
                    char longLostCloser = matchingChunkers[orphanedOpener];

                    missingCharScore *= 5;
                    missingCharScore += ScoreMissingSyntax(longLostCloser);
                }

                scores.Add(missingCharScore);
            }
        }

        scores.Sort();
        long middleScore = scores[scores.Count / 2];

        Console.WriteLine($"Score: {middleScore}");
    }

    private string[] GetInput(bool useTestInput)
    {
        string fileName = useTestInput ? "day10/test_input.txt" : "day10/input.txt";

        string[] lines = File.ReadAllLines(fileName)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

        return lines;
    }

    private int ScoreBadSyntax(char illegalChar)
    {
        switch (illegalChar)
        {
            case ')': return 3;
            case ']': return 57;
            case '}': return 1197;
            case '>': return 25137;
            default: throw new Exception($"Not a closing chunk character: {illegalChar}");
        }
    }

    private long ScoreMissingSyntax(char missingChar)
    {
        switch (missingChar)
        {
            case ')': return 1;
            case ']': return 2;
            case '}': return 3;
            case '>': return 4;
            default: throw new Exception($"Not a closing chunk character: {missingChar}");
        }
    }

    private HashSet<char> openers = new HashSet<char>
    {
        '(', '{', '[', '<'
    };

    private Dictionary<char, char> matchingChunkers = new Dictionary<char, char>
    {
        { '(', ')' },
        { ')', '(' },
        { '[', ']' },
        { ']', '[' },
        { '{', '}' },
        { '}', '{' },
        { '<', '>' },
        { '>', '<' },
    };
}