namespace problem_solving;

public class Heightmap
{
    public void Solve_Part1()
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
    public void Solve_Part2()
    {
        int[][] heightMap = GetHeightmap(false);

        List<int> basinSizes = new List<int>();

        for (int i = 0; i < heightMap.Length; i++)
        {
            int[] row = heightMap[i];
            for (int j = 0; j < row.Length; j++)
            {
                // Determine if position is in low-spot
                // If so, traverse graph looking for adjacent
                // nodes that have a height larger than current
                // and is not 9
                if (IsLowSpot(i, j, heightMap))
                {
                    int basinSize = GetBasinSize(i, j, heightMap);
                    basinSizes.Add(basinSize);
                }
            }
        }

        basinSizes.Sort();
        int[] largestBasins = basinSizes.TakeLast(3).ToArray();

        int answer = 1;
        foreach (var basinSize in largestBasins)
        {
            answer *= basinSize;
        }

        Console.WriteLine($"Answer: {answer}");
    }

    private bool[][] InitBoolArray(int height, int width)
    {
        bool[][] arr = new bool[height][];

        // init memory
        for (int i = 0; i < height; i++)
        {
            arr[i] = new bool[width];
            for (int j = 0; j < width; j++)
            {
                arr[i][j] = false;
            }
        }

        return arr;
    }

    private int GetBasinSize(int root_i, int root_j, int[][] heightMap)
    {
        bool[][] memory = InitBoolArray(heightMap.Length, heightMap[0].Length);

        Queue<(int i, int j)> traverse = new Queue<(int i, int j)>();

        int basinSize = 0;
        traverse.Enqueue((root_i, root_j));
        while (traverse.Count != 0)
        {
            // Get location
            var location = traverse.Dequeue();

            // Increase basin size
            basinSize++;

            // Get height for current location
            int currentHeight = heightMap[location.i][location.j];

            var adjacents = GetAdjacents(location.i, location.j, heightMap);
            foreach (var adjacent in adjacents)
            {
                // Have not been here before
                if (!memory[adjacent.i][adjacent.j])
                {
                    var adjacent_height = heightMap[adjacent.i][adjacent.j];
                    if (adjacent_height != 9 && adjacent_height > currentHeight)
                    {
                        // Traverse to the adjacent point if it meets the basin criteria
                        traverse.Enqueue(adjacent);

                        // Mark adjacent location as traversed, to avoid adding it multiple times in future
                        memory[adjacent.i][adjacent.j] = true;
                    }
                }
            }
        }

        return basinSize;
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