namespace problem_solving;

public class TrickShot
{
    public void Solve_Part1()
    {
        //string input = "target area: x=128..160, y=-142..-88";
        (int min, int max) x_range = (128, 160);
        (int min, int max) y_range = (-142, -88);

        Console.WriteLine($"max y: {FindHighestShot(y_range)}");
    }

    public void Solve_Part2()
    {
        //string input = "target area: x=128..160, y=-142..-88";
        (int min, int max) x_range = (128, 160);
        (int min, int max) y_range = (-142, -88);
        // (int min, int max) x_range = (20, 30);
        // (int min, int max) y_range = (-10,-5);

        Console.WriteLine($"distinc velocities: {GetDistinctVelocities(x_range, y_range)}");
    }

    private int GetDistinctVelocities((int min, int max) x_range, (int min, int max) y_range)
    {
        int max_y_vel = GetMaxYVel(y_range);
        int count = 0;

        for (int i = y_range.min; i <= max_y_vel; i++)
        {
            for (int j = 0; j <= x_range.max; j++)
            {
                if (Fire(j, i, x_range, y_range))
                {
                    count++;
                }
            }
        }

        return count;
    }

    private bool Fire(int init_x_vel, int init_y_vel, (int min, int max) x_range, (int min, int max) y_range)
    {
        int x_pos = 0;
        int y_pos = 0;

        int x_vel = init_x_vel;
        int y_vel = init_y_vel;

        while (y_pos >= y_range.min && x_pos <= x_range.max && !IsHit(x_pos, y_pos, x_range, y_range))
        {
            x_pos += x_vel;
            y_pos += y_vel;

            x_vel = Math.Max(0, x_vel - 1);
            y_vel--;
        }

        return IsHit(x_pos, y_pos, x_range, y_range);
    }

    private bool IsHit(int x_pos, int y_pos, (int min, int max) x_range, (int min, int max) y_range)
    {
        return x_pos >= x_range.min && x_pos <= x_range.max && y_pos >= y_range.min && y_pos <= y_range.max;
    }

    private int FindHighestShot((int min, int max) y_range)
    {
        int init_y_vel = GetMaxYVel(y_range);

        return (int)Math.Ceiling(init_y_vel * (init_y_vel + 1) / 2.0);
    }

    private int GetMaxYVel((int min, int max) y_range)
    {
        int init_y_vel = 0;
        if (y_range.min < 0)
        {
            init_y_vel = Math.Abs(y_range.min + 1);
        }
        else
        {
            init_y_vel = y_range.min;
        }

        if (y_range.max < 0)
        {
            init_y_vel = Math.Max(init_y_vel, Math.Abs(y_range.max + 1));
        }
        else
        {
            init_y_vel = Math.Max(init_y_vel, y_range.max);
        }

        return init_y_vel;
    }
}