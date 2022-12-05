public class CalorieCounting
{
    public void Solve_Part1()
    {
        var lines = File.ReadAllLines("day1/input.txt");

        int maxCalories = 0;
        int caloriesAcc = 0;
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                caloriesAcc = 0;
            }
            else
            {
                caloriesAcc += int.Parse(line);
                if (caloriesAcc > maxCalories)
                {
                    maxCalories = caloriesAcc;
                }
            }
        }

        Console.WriteLine($"Max: {maxCalories}");
    }
    public void Solve_Part2()
    {
        SortedList<int, int> calorieAccs = new(Comparer<int>.Default);
        var lines = File.ReadAllLines("day1/input.txt");

        int caloriesAcc = 0;
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                calorieAccs.Add(caloriesAcc, caloriesAcc);
                caloriesAcc = 0;
            }
            else
            {
                caloriesAcc += int.Parse(line);
            }
        }

        int top3Sum = calorieAccs.TakeLast(3).Select(kvp => kvp.Value).Sum();

        Console.WriteLine($"Max: {top3Sum}");
    }
}