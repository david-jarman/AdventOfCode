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

    private int FindHighestShot((int min, int max) y_range)
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

        return (int)Math.Ceiling(init_y_vel * (init_y_vel + 1) / 2.0);
    }
}