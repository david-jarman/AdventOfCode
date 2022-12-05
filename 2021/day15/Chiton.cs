namespace problem_solving;

public class Chiton
{
    public void Solve_Part1()
    {
        string fileName = "day15/input.txt";

        var lines = File.ReadAllLines(fileName)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

        int size = lines.Length;
        int[,] map = new int[size,size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                map[j, i] = int.Parse($"{lines[i][j]}");
            }
        }

        Queue<(int x, int y, int accRisk)> q = new Queue<(int x, int y, int accRisk)>();
        
        int[,] lowestRisk = new int[size, size];
        for (int i = 0; i < lowestRisk.GetLength(1); i++)
        {
            for (int j = 0; j < lowestRisk.GetLength(0); j++)
            {
                lowestRisk[j, i] = int.MaxValue;
            }
        }

        lowestRisk[0,0] = 0;

        q.Enqueue((0, 0, 0));

        while (q.Count > 0)
        {
            var loc = q.Dequeue();

            foreach (var adj in GetAdjacents(loc.x, loc.y, size))
            {
                int newPossibleRisk = loc.accRisk + map[adj.x, adj.y];
                if (newPossibleRisk < lowestRisk[adj.x, adj.y])
                {
                    lowestRisk[adj.x, adj.y] = newPossibleRisk;
                    q.Enqueue((adj.x, adj.y, newPossibleRisk));
                }
            }
        }

        int lowestRiskPath = lowestRisk[size-1, size-1];

        Console.WriteLine($"answer: {lowestRiskPath}");
    }

    public void Solve_Part2()
    {
        string fileName = "day15/input.txt";

        var lines = File.ReadAllLines(fileName)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

        int smallSize = lines.Length;
        int fullSize = smallSize * 5;
        int[,] map = new int[fullSize,fullSize];

        for (int i = 0; i < fullSize; i++)
        {
            for (int j = 0; j < fullSize; j++)
            {
                int originalValue = int.Parse($"{lines[i%smallSize][j%smallSize]}");
                int scale = (i / smallSize) + (j / smallSize);
                int actualValue = ((originalValue + scale - 1) % 9) + 1;
                map[j, i] = actualValue;
            }
        }

        Queue<(int x, int y, int accRisk)> q = new Queue<(int x, int y, int accRisk)>();
        
        int[,] lowestRisk = new int[fullSize, fullSize];
        for (int i = 0; i < lowestRisk.GetLength(1); i++)
        {
            for (int j = 0; j < lowestRisk.GetLength(0); j++)
            {
                lowestRisk[j, i] = int.MaxValue;
            }
        }

        lowestRisk[0,0] = 0;

        q.Enqueue((0, 0, 0));

        while (q.Count > 0)
        {
            var loc = q.Dequeue();

            foreach (var adj in GetAdjacents(loc.x, loc.y, fullSize))
            {
                int newPossibleRisk = loc.accRisk + map[adj.x, adj.y];
                if (newPossibleRisk < lowestRisk[adj.x, adj.y])
                {
                    lowestRisk[adj.x, adj.y] = newPossibleRisk;
                    q.Enqueue((adj.x, adj.y, newPossibleRisk));
                }
            }
        }

        int lowestRiskPath = lowestRisk[fullSize-1, fullSize-1];

        Console.WriteLine($"answer: {lowestRiskPath}");
    }

    private List<(int x_off, int y_off)> Offsets = new List<(int i_off, int j_off)>
    {
        (-1, 0),
        (0, -1),
        (0, 1),
        (1, 0),
    };

    private IEnumerable<(int x, int y)> GetAdjacents(int x, int y, int size)
    {
        var list = new List<(int i, int j)>();

        foreach (var adjacentOffset in Offsets)
        {
            int x_prime = x + adjacentOffset.x_off;
            int y_prime = y + adjacentOffset.y_off;

            if (x_prime < 0 || x_prime >= size || y_prime < 0 || y_prime >= size)
            {
                continue;
            }

            list.Add((x_prime, y_prime));
        }

        return list;
    }
}