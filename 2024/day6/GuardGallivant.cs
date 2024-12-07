using Ind = (int i, int j);

namespace CY2024;

public partial class Solutions
{
    private const char Guard = '^';
    private const char Obstacle = '#';

    public static void Day6_Part1()
    {
        var input = File.ReadAllLines("2024/day6/input.txt");
        //var input = File.ReadAllLines("2024/day6/sample.txt");
        bool[][] map = input.Select(line => line.Select(c => c.Equals(Obstacle)).ToArray()).ToArray();
        Ind? startPos = null;
        int i = 0;
        foreach (var line in input)
        {
            int j = 0;
            foreach (char c in line)
            {
                if (c.Equals(Guard))
                {
                    startPos = (i, j);
                    break;
                }
                j++;
            }

            if (startPos != null)
                break;

            i++;
        }

        if (startPos == null)
            throw new Exception();

        int count = Move(map, startPos.Value.i, startPos.Value.j);

        Console.WriteLine($"Count: {count}");
    }

    public static void Day6_Part2()
    {
        var input = File.ReadAllLines("2024/day6/input.txt");
        //var input = File.ReadAllLines("2024/day6/sample.txt");
        bool[][] map = input.Select(line => line.Select(c => c.Equals(Obstacle)).ToArray()).ToArray();
        Ind? startPos = null;
        int i = 0;
        foreach (var line in input)
        {
            int j = 0;
            foreach (char c in line)
            {
                if (c.Equals(Guard))
                {
                    startPos = (i, j);
                    break;
                }
                j++;
            }

            if (startPos != null)
                break;

            i++;
        }

        if (startPos == null)
            throw new Exception();

        // Wondering if the traverse logic is messed up? Is there an edge case tripping us up?
        // Didn't account for obstacles that form corners or u-turns

        int count = 0;
        for (i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[0].Length; j++)
            {
                // Ignore guard start position
                if (i == startPos.Value.i && j == startPos.Value.j)
                    continue;

                if (!map[i][j])
                {
                    // Try adding an obstacle and run the sim
                    map[i][j] = true;
                    bool stuck = MoveWithLoopDetection(map, startPos.Value.i, startPos.Value.j);
                    if (stuck)
                    {
                        // Console.WriteLine($"Success. Obstacle added at [{i},{j}]");
                    }

                    count += stuck ? 1 : 0;

                    // remove the added obstacle.
                    map[i][j] = false;
                }
            }
        }

        Console.WriteLine($"Count: {count}");
    }

    private static bool MoveWithLoopDetection(bool[][] map, int start_i, int start_j)
    {
        // memo will represent if the guard has been in a position in a direction
        // 0 = not been there
        // 1 = been there in the up dir
        // 2 = right
        // 4 = down
        // 8 = left
        int[][] memo = map.Select(x => x.Select(_ => 0).ToArray()).ToArray();
        memo[start_i][start_j] = 1;
        int rows = map.Length;
        int cols = map[0].Length;

        int i = start_i;
        int j = start_j;

        Ind up = (-1, 0);
        Ind down = (1, 0);
        Ind left = (0, -1);
        Ind right = (0, 1);

        Ind[] dirs = [up, right, down, left];

        int turnCount = 0;
        int currentDirection = 1; // default to guard faceing up.
        var moveDir = up;
        while (true)
        {
            // Advance if no obstacles, otherwise, turn, and advance.
            Ind forward = (i + moveDir.i, j + moveDir.j);
            if (forward.i < 0 || forward.i >= cols || forward.j < 0 || forward.j >= rows)
                return false;

            // Keep turning until an obstacle is not found
            while (map[forward.i][forward.j])
            {
                // Obstruction found. Turn 90 degrees.
                turnCount++;
                moveDir = dirs[turnCount % 4];
                currentDirection = 1 << (turnCount % 4);

                // Record the about-face, before moving.
                memo[i][j] |= currentDirection;

                forward = (i + moveDir.i, j + moveDir.j);
            }

            // Move forward
            i += moveDir.i;
            j += moveDir.j;

            // Check if guard has been in this spot, in this direction before.
            if ((memo[i][j] & currentDirection) != 0)
            {
                return true;
            }

            // Record direction after turning and moving
            memo[i][j] |= currentDirection;
        }
    }

    private static int Move(bool[][] map, int start_i, int start_j)
    {
        bool[][] memo = map.Select(x => x.Select(_ => false).ToArray()).ToArray();
        memo[start_i][start_j] = true;
        int rows = map.Length;
        int cols = map[0].Length;

        int i = start_i;
        int j = start_j;

        Ind up = (-1, 0);
        Ind down = (1, 0);
        Ind left = (0, -1);
        Ind right = (0, 1);

        Ind[] dirs = [up, right, down, left];

        int turnCount = 0;
        var curDir = dirs[turnCount % 4];
        while (true)
        {
            // Advance if no obstacles, otherwise, turn, and advance.

            Ind forward = (i + curDir.i, j + curDir.j);
            if (forward.i < 0 || forward.i >= cols || forward.j < 0 || forward.j >= rows)
                break;

            if (map[forward.i][forward.j])
                curDir = dirs[++turnCount % 4];

            i += curDir.i;
            j += curDir.j;
            memo[i][j] = true;
        }

        return memo.SelectMany(x => x).Aggregate(0, (agg, val) => val ? agg + 1 : agg);
    }
}