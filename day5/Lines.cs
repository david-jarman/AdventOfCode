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

            int delta_x = x_2-x_1;
            int delta_y = y_2-y_1;

            int incr_x = delta_x == 0 ? 0 : Math.Abs(delta_x) / delta_x;
            int incr_y = delta_y == 0 ? 0 : Math.Abs(delta_y) / delta_y;

            int new_x = 0;
            int new_y = 0;

            int iter = 0;
            do
            {
                new_x = x_1 + (incr_x * iter);
                new_y = y_1 + (incr_y * iter);

                grid[new_x][new_y] += 1;

                iter++;
            }
            // loop until we've reached the destination point
            while (!(new_x == x_2 && new_y == y_2));
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