namespace problem_solving;

public class OctupusFlashes
{
    public void Solve_Part1()
    {
        int[][] energy = ParseInput(false);

        const int steps = 100;
        int flashes = 0;
        int height = energy.Length;
        int width = energy[0].Length;

        for (int step = 0; step < steps; step++)
        {
            Queue<(int i, int j)> traverse = new Queue<(int i, int j)>();
            bool[][] flashed = new bool[energy.Length][];
            for (int i = 0; i < flashed.Length; i++)
            {
                flashed[i] = new bool[energy[i].Length];
                Array.Fill(flashed[i], false);
            }

            // increase energy levels by 1. Add any flashers to queue
            for (int i = 0; i < energy.Length; i++)
            {
                for (int j = 0; j < energy[i].Length; j++)
                {
                    if (++energy[i][j] > 9)
                    {
                        traverse.Enqueue((i, j));
                        flashes++;
                        flashed[i][j] = true;
                    }
                }
            }

            // Cascade flashes to adjacents
            while (traverse.Count > 0)
            {
                (int i, int j) = traverse.Dequeue();

                List<(int i, int j)> adjacents = GetAdjacents(i, j, width, height);
                foreach (var adjacent in adjacents)
                {
                    energy[adjacent.i][adjacent.j]++;
                    if (energy[adjacent.i][adjacent.j] > 9 && !flashed[adjacent.i][adjacent.j])
                    {
                        flashed[adjacent.i][adjacent.j] = true;
                        flashes++;
                        traverse.Enqueue((adjacent.i, adjacent.j));
                    }
                }
            }

            // Reset energy back to 0
            for (int i = 0; i < energy.Length; i++)
            {
                for (int j = 0; j < energy[i].Length; j++)
                {
                    if (energy[i][j] > 9)
                    {
                        energy[i][j] = 0;
                    }
                }
            }
        }

        Console.WriteLine($"Flashes: {flashes}");
    }

    private List<(int i_off, int j_off)> adjacent_offsets = new List<(int i_off, int j_off)>
    {
        (-1, -1),
        (-1, 0),
        (-1, 1),
        (0, -1),
        (0, 0),
        (0, 1),
        (1, -1),
        (1, 0),
        (1, 1)
    };

    private List<(int i, int j)> GetAdjacents(int i, int j, int width, int height)
    {
        var list = new List<(int i, int j)>();

        foreach (var adjacentOffset in adjacent_offsets)
        {
            int i_prime = i + adjacentOffset.i_off;
            int j_prime = j + adjacentOffset.j_off;

            if (i_prime < 0 || i_prime >= height || j_prime < 0 || j_prime >= width)
            {
                continue;
            }

            list.Add((i_prime, j_prime));
        }

        return list;
    }

    private int[][] ParseInput(bool useTestData)
    {
        string fileName = useTestData ? "day11/test_input.txt" : "day11/input.txt";
        string[] lines = File.ReadAllLines(fileName)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

        int[][] input = new int[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            input[i] = lines[i]
                .ToCharArray()
                .Select(c => int.Parse($"{c}"))
                .ToArray();
        }

        return input;
    }
}