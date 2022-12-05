namespace problem_solving;

public class SevenSegmentDisplay
{
    public void Part1_Solve()
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

    public void Solve()
    {
        var inputs = ParseInput(false);

        int answer = 0;
        for (int i = 0; i < inputs.signalPatterns.Length; i++)
        {
            var deduction = new DigitDeduction();
            var signalPatterns = inputs.signalPatterns[i];
            var digitOutput = inputs.digitOutputs[i];

            foreach (string signalPattern in signalPatterns)
            {
                switch (signalPattern.Length)
                {
                    case 2:
                        // One Digit
                        deduction.Deduce_1(signalPattern);
                        break;

                    case 4:
                        // Four Digit
                        deduction.Deduce_4(signalPattern);
                        break;

                    case 3:
                        // Seven Digit
                        deduction.Deduce_7(signalPattern);
                        break;

                    case 5:
                        // 5 Digits
                        deduction.Deduce_5_Segment_Signals(signalPattern);
                        break;

                    case 6:
                        // 6 Digits
                        deduction.Deduce_6_Segment_Signals(signalPattern);
                        break;
                }
            }

            // Infer from whats leftover
            deduction.InferRemainingSignals();

            if (deduction.UninferredSignals().Count() != 0)
            {
                throw new Exception("unable to deduce");
            }

            // Convert those signals
            int fullNumber = 0;
            int scale = 1000;
            for (int j = 0; j < digitOutput.Length; j++)
            {
                string digitSignal = digitOutput[j];
                int number = deduction.GetDigitFromBadSignal(digitSignal);

                fullNumber += number * scale;
                scale = scale / 10;
            }

            answer += fullNumber;
        }

        Console.WriteLine($"answer: {answer}");
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

public class DigitDeduction
{
    public HashSet<char> A_Possibilities { get; set; } = new HashSet<char>("abcdefg");
    public HashSet<char> B_Possibilities { get; set; } = new HashSet<char>("abcdefg");
    public HashSet<char> C_Possibilities { get; set; } = new HashSet<char>("abcdefg");
    public HashSet<char> D_Possibilities { get; set; } = new HashSet<char>("abcdefg");
    public HashSet<char> E_Possibilities { get; set; } = new HashSet<char>("abcdefg");
    public HashSet<char> F_Possibilities { get; set; } = new HashSet<char>("abcdefg");
    public HashSet<char> G_Possibilities { get; set; } = new HashSet<char>("abcdefg");

    public int GetDigitFromBadSignal(string signalString)
    {
        string newSignal = "";
        foreach (char signal in signalString)
        {
            var correctedSignal = GetCorrectedSignal(signal);
            newSignal += correctedSignal;
        }

        return GetDigit(newSignal);
    }

    private HashSet<char> Zero = new HashSet<char>("abcefg");
    private HashSet<char> One = new HashSet<char>("cf");
    private HashSet<char> Two = new HashSet<char>("acdeg");
    private HashSet<char> Three = new HashSet<char>("acdfg");
    private HashSet<char> Four = new HashSet<char>("bcdf");
    private HashSet<char> Five = new HashSet<char>("abdfg");
    private HashSet<char> Six = new HashSet<char>("abdefg");
    private HashSet<char> Seven = new HashSet<char>("acf");
    private HashSet<char> Eight = new HashSet<char>("abcdefg");
    private HashSet<char> Nine = new HashSet<char>("abcdfg");

    private int GetDigit(string correctedSignal)
    {
        HashSet<char> signalSet = new HashSet<char>(correctedSignal);

        if (signalSet.SetEquals(Zero))
        {
            return 0;
        }

        if (signalSet.SetEquals(One))
        {
            return 1;
        }

        if (signalSet.SetEquals(Two))
        {
            return 2;
        }

        if (signalSet.SetEquals(Three))
        {
            return 3;
        }

        if (signalSet.SetEquals(Four))
        {
            return 4;
        }

        if (signalSet.SetEquals(Five))
        {
            return 5;
        }

        if (signalSet.SetEquals(Six))
        {
            return 6;
        }

        if (signalSet.SetEquals(Seven))
        {
            return 7;
        }

        if (signalSet.SetEquals(Eight))
        {
            return 8;
        }

        if (signalSet.SetEquals(Nine))
        {
            return 9;
        }

        return -999;
    }

    public char GetCorrectedSignal(char oldSignal)
    {
        if (A_Possibilities.Contains(oldSignal))
        {
            return 'a';
        }
        if (B_Possibilities.Contains(oldSignal))
        {
            return 'b';
        }
        if (C_Possibilities.Contains(oldSignal))
        {
            return 'c';
        }
        if (D_Possibilities.Contains(oldSignal))
        {
            return 'd';
        }
        if (E_Possibilities.Contains(oldSignal))
        {
            return 'e';
        }
        if (F_Possibilities.Contains(oldSignal))
        {
            return 'f';
        }
        if (G_Possibilities.Contains(oldSignal))
        {
            return 'g';
        }

        throw new Exception();
    }

    public IEnumerable<HashSet<char>> Signals()
    {
        yield return A_Possibilities;
        yield return B_Possibilities;
        yield return C_Possibilities;
        yield return D_Possibilities;
        yield return E_Possibilities;
        yield return F_Possibilities;
        yield return G_Possibilities;
    }

    public IEnumerable<HashSet<char>> UninferredSignals()
    {
        return Signals().Where(s => s.Count != 1).ToList();
    }

    public IEnumerable<HashSet<char>> KnownSignals()
    {
        return Signals().Where(s => s.Count == 1).ToList();
    }

    public void InferRemainingSignals()
    {
        var knownSignals = KnownSignals();
        var unkownSignals = UninferredSignals();

        foreach (var unkownSignal in unkownSignals)
        {
            foreach (var knownSignal in knownSignals)
            {
                if (unkownSignal.Count == 1)
                {
                    // finish, it has been inferred
                    break;
                }

                unkownSignal.ExceptWith(knownSignal);
            }
        }
    }

    public void Deduce_1(string signal)
    {
        if (signal.Length != 2)
        {
            throw new Exception();
        }

        C_Possibilities.IntersectWith(signal);
        F_Possibilities.IntersectWith(signal);

        A_Possibilities.ExceptWith(signal);
        B_Possibilities.ExceptWith(signal);
        D_Possibilities.ExceptWith(signal);
        E_Possibilities.ExceptWith(signal);
        G_Possibilities.ExceptWith(signal);
    }

    public void Deduce_4(string signal)
    {
        if (signal.Length != 4)
        {
            throw new Exception();
        }

        B_Possibilities.IntersectWith(signal);
        C_Possibilities.IntersectWith(signal);
        D_Possibilities.IntersectWith(signal);
        F_Possibilities.IntersectWith(signal);

        A_Possibilities.ExceptWith(signal);
        E_Possibilities.ExceptWith(signal);
        G_Possibilities.ExceptWith(signal);
    }

    public void Deduce_7(string signal)
    {
        if (signal.Length != 3)
        {
            throw new Exception();
        }

        A_Possibilities.IntersectWith(signal);
        C_Possibilities.IntersectWith(signal);
        F_Possibilities.IntersectWith(signal);

        B_Possibilities.ExceptWith(signal);
        D_Possibilities.ExceptWith(signal);
        E_Possibilities.ExceptWith(signal);
        G_Possibilities.ExceptWith(signal);
    }

    public void Deduce_5_Segment_Signals(string signal)
    {
        if (signal.Length != 5)
        {
            throw new Exception();
        }

        A_Possibilities.IntersectWith(signal);
        D_Possibilities.IntersectWith(signal);
        G_Possibilities.IntersectWith(signal);
    }

    internal void Deduce_6_Segment_Signals(string signal)
    {
        if (signal.Length != 6)
        {
            throw new Exception();
        }

        A_Possibilities.IntersectWith(signal);
        B_Possibilities.IntersectWith(signal);
        F_Possibilities.IntersectWith(signal);
        G_Possibilities.IntersectWith(signal);
    }
}
