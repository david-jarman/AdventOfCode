namespace problem_solving;

public class Lines
{
    public void Solve()
    {
        string[] lines = File.ReadAllLines("day5/input.txt");
        //string[] lines = File.ReadAllLines("day5/test_input.txt");

        const int gridSize = 1000;
        //const int gridSize = 10;
        int[][] grid = new int[gridSize][];
        for (int i = 0; i < gridSize; i++)
        {
            grid[i] = new int[gridSize];
            for (int j = 0; j < gridSize; j++)
            {
                grid[i][j] = 0;
            }
        }

        foreach (string line in lines)
        {
            // sample line: 348,742 -> 620,742
            var coordArray = line.Split(" -> ");

            (int x_1, int y_1) = ConvertStringToPoint(coordArray[0]);
            (int x_2, int y_2) = ConvertStringToPoint(coordArray[1]);

            if (x_1 != x_2 && y_1 != y_2)
            {
                // not a horizontal or vertical line, discard
                continue;
            }

            // Fill in points between coords
            // distance can be positive or negative
            int delta_x = x_2-x_1;
            int delta_y = y_2-y_1;
            int distance = (delta_x) + (delta_y);
            int abs_distance = Math.Abs(distance);

            // Either 1 or -1, depending on going forwards or backwards along line
            int incrementer = abs_distance / distance;

            for (int iter = 0; iter <= abs_distance; iter += 1)
            {
                // make the iterator positive or negative, based on direction
                int i = iter * incrementer;

                int incr_x = 0;
                if (delta_x != 0)
                {
                    incr_x = i;
                }

                int incr_y = 0;
                if (delta_y != 0)
                {
                    incr_y = i;
                }

                int new_x = x_1 + incr_x;
                int new_y = y_1 + incr_y;

                grid[new_x][new_y] += 1;
            }
        }

        // look at grid

        int overlappingPointCount = 0;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (grid[i][j] >= 2)
                {
                    overlappingPointCount++;
                }
            }
        }
        
        Console.WriteLine($"overlappingPointCount: {overlappingPointCount}");
    }

    private (int x, int y) ConvertStringToPoint(string s)
    {
        int[] x_y = s.Split(',').Select(s => int.Parse(s)).ToArray();
        int x = x_y[0];
        int y = x_y[1];

        return (x, y);
    }
}