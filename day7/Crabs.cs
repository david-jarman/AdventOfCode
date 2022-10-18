namespace problem_solving;

public class Crabs
{
    public void Part_1Solve()
    {
        // Naive solution is to check the cost of fuel for each unique position

        int[] positions = GetInput(false);
        int[] distinct_pos = positions.Distinct().ToArray();

        int min_fuel_cost = int.MaxValue;
        foreach (int pos in distinct_pos)
        {
            int cur_fuel_cost = CalcFuelCost(positions, pos);
            min_fuel_cost = Math.Min(min_fuel_cost, cur_fuel_cost);
        }

        Console.WriteLine($"Min fuel cost: {min_fuel_cost}");
    }

    private int[] GetInput(bool useTestInput)
    {
        string input_file_name = useTestInput ? "day7/test_input.txt" : "day7/input.txt";

        return File.ReadAllText(input_file_name)
            .Split(',')
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => int.Parse(s))
            .ToArray();
    }

    private int CalcFuelCost(int[] positions, int current_position)
    {
        int sum = 0;
        foreach (int pos in positions)
        {
            sum += Math.Abs(pos - current_position);
        }

        return sum;
    }
}