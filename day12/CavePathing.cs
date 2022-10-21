namespace problem_solving;

public class CavePathing
{
    public void Solve_Part1()
    {
        Cave startCave = BuildCaveSystemFromInput();

        int uniquePaths = 0;

        Stack<(int level, Cave cave)> caveStack = new Stack<(int level, Cave cave)>();
        Stack<Cave> currentPath = new Stack<Cave>();
        caveStack.Push((0, startCave));

        while (caveStack.Count != 0)
        {
            (int level, Cave cave) = caveStack.Pop();

            while (level != currentPath.Count)
            {
                // remove caves off the path to match the level of the newly visited cave
                currentPath.Pop();
            }

            currentPath.Push(cave);

            if (cave.IsEndCave)
            {
                uniquePaths++;
                continue;
            }

            // remove any caves that are small and in the current path
            List<Cave> nextCaves = cave.ConnectedCaves.Where(c => c.IsBig || !currentPath.Contains(c)).ToList();

            foreach (var next in nextCaves)
            {
                caveStack.Push((level + 1, next));
            }
        }

        Console.WriteLine($"unique paths: {uniquePaths}");
    }

    private Cave BuildCaveSystemFromInput()
    {
        string fileName = "day12/input.txt";

        var lines = File.ReadAllLines(fileName)
            .Where(s => !string.IsNullOrWhiteSpace(s));

        Dictionary<string, Cave> caves = new Dictionary<string, Cave>();

        foreach (string line in lines)
        {
            var caveNames = line.Split('-');
            string cave1Name = caveNames[0];
            string cave2Name = caveNames[1];

            if (!caves.TryGetValue(cave1Name, out Cave cave1))
            {
                caves[cave1Name] = new Cave(cave1Name);
                cave1 = caves[cave1Name];
            }

            if (!caves.TryGetValue(cave2Name, out Cave cave2))
            {
                caves[cave2Name] = new Cave(cave2Name);
                cave2 = caves[cave2Name];
            }

            cave1.ConnectedCaves.Add(cave2);
            cave2.ConnectedCaves.Add(cave1);
        }

        return caves["start"];
    }
}

public class Cave
{
    public Cave(string name)
    {
        IsBig = char.IsUpper(name[0]);
        IsEndCave = name.Equals("end", StringComparison.OrdinalIgnoreCase);
        Name = name;
    }

    public List<Cave> ConnectedCaves { get; } = new List<Cave>();

    public bool IsBig { get; }

    public string Name { get; }

    public bool IsEndCave { get; }
}