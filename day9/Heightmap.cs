namespace problem_solving;

public class Heightmap
{
    public void Solve_Day1()
    {
        int[][] heightMap = GetHeightmap(false);

        int riskLevelSum = 0;

        for (int i = 0; i < heightMap.Length; i++)
        {
            int[] row = heightMap[i];
            for (int j = 0; j < row.Length; j++)
            {
                // Determine if position is in low-spot
                // if true, add position + 1 to risk level sum
                if (IsLowSpot(i, j, heightMap))
                {
                    riskLevelSum += heightMap[i][j] + 1;
                }
            }
        }

        Console.WriteLine($"Risk Level: {riskLevelSum}");
    }

    private bool IsLowSpot(int i, int j, int[][] heightMap)
    {
        int height = heightMap[i][j];
        bool isLow = true;
        foreach (var adjacents in GetAdjacents(i, j, heightMap))
        {
            if (height >= heightMap[adjacents.i][adjacents.j])
            {
                isLow = false;
                break;
            }
        }

        return isLow;
    }

    private (int i, int j)[] GetAdjacents(int i, int j, int[][] heightMap)
    {
        List<(int i, int j)> adjacents = new List<(int i, int j)>();

        int left = j - 1;
        if (left >= 0)
        {
            adjacents.Add((i, left));
        }

        int right = j + 1;
        if (right < heightMap[i].Length)
        {
            adjacents.Add((i, right));
        }

        int down = i + 1;
        if (down < heightMap.Length)
        {
            adjacents.Add((down, j));
        }

        int up = i - 1;
        if (up >= 0)
        {
            adjacents.Add((up, j));
        }

        return adjacents.ToArray();
    }

    private int[][] GetHeightmap(bool useTestInput)
    {
        string fileName = useTestInput ? "day9/test_input.txt" : "day9/input.txt";

        string[] lines = File.ReadAllLines(fileName)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

        int array_size = lines.Length;

        int[][] heightMap = new int[array_size][];
        for (int i = 0; i < array_size; i++)
        {
            heightMap[i] = lines[i].ToCharArray().Select(c => int.Parse($"{c}")).ToArray();
        }

        return heightMap;
    }
}