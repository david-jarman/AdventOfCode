namespace CY2023;

public partial class Solutions
{
    public static void Day3_Part1()
    {
        var input = File.ReadAllLines("2023/day3/input.txt");

        bool[][] symbolMap = input.Select(line => line.Select(c => !char.IsDigit(c) && c != '.').ToArray()).ToArray();
        List<(int num, int i, int j, int len)> partNumbers = new();

        foreach (var row in input.Select((line, i) => (line, i)))
        {
            int i_start = -1;
            int j_start = -1;
            var curNum = new Stack<int>();
            foreach (var col in row.line.Select((c, j) => (c, j)))
            {
                int i = row.i;
                int j = col.j;
                char c = col.c;

                if (char.IsDigit(c))
                {
                    if (i_start == -1)
                    {
                        // start new number
                        i_start = i;
                        j_start = j;
                    }

                    curNum.Push(c - '0');
                }
                else if (curNum.Count > 0)
                {
                    // end current number and reset context
                    int num = 0;
                    int mul = 1;
                    int numLength = curNum.Count;
                    while (curNum.TryPop(out int digit))
                    {
                        num += mul * digit;
                        mul *= 10;
                    }

                    partNumbers.Add((num, i_start, j_start, numLength));
                    i_start = -1;
                    j_start = -1;
                }
            }
        }

        int partNumSum = 0;
        foreach (var partNum in partNumbers)
        {
            // Determine if part number is adjacent to a symbol in the map
            partNumSum += IsAdjacent(partNum.i, partNum.j, partNum.len, symbolMap) ? partNum.num : 0;
        }

        Console.WriteLine($"Sum: {partNumSum}");
    }

    private static bool IsAdjacent(int i, int j, int len, bool[][] symbolMap)
    {
        var adjacentIndicies = GetAdjacentIndices(i, j, len, symbolMap.Length, symbolMap[0].Length);
        foreach (var adj in adjacentIndicies)
        {
            if (symbolMap[adj.i][adj.j])
                return true;
        }

        return false;
    }

    private static List<(int i, int j)> GetAdjacentIndices(int i, int j, int len, int rows, int cols)
    {
        List<(int i, int j)> indices = new();

        int start = Math.Max(j - 1, 0);
        int end = Math.Min(j + len, cols - 1);
        for (int iter = start; iter <= end; iter++)
        {
            // top
            int i_top = i - 1;
            if (i_top > 0)
                indices.Add((i_top, iter));

            // middle
            if (iter < j || iter > j + len - 1)
                indices.Add((i, iter));

            // bottom
            int i_bottom = i + 1;
            if (i_bottom < rows)
                indices.Add((i_bottom, iter));
        }

        return indices;
    }
}