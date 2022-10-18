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
    public void Part_2Solve()
    {
        int[] positions = GetInput(false);
        int min_pos = positions.Min();
        int max_pos = positions.Max();

        int min_fuel_cost = int.MaxValue;
        for (int pos = min_pos; pos <= max_pos; pos++)
        {
            int cur_fuel_cost = CalcFuelCost_NonLinear(positions, pos);
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

    private int CalcFuelCost_NonLinear(int[] positions, int current_position)
    {
        Dictionary<int, int> memory = new Dictionary<int, int>();

        int fuel_cost_sum = 0;
        foreach (int pos in positions)
        {
            int distance = Math.Abs(pos - current_position);
            fuel_cost_sum += Sum(distance, memory);
        }

        return fuel_cost_sum; 
    }

    private int Sum(int i, Dictionary<int, int> memory)
    {
        if (i == 1 || i == 0)
        {
            memory[i] = i;
            return i;
        }

        int sum;
        if (memory.TryGetValue(i, out sum))
        {
            return sum;
        }
        else
        {
            sum = i + Sum(i - 1, memory);
            memory[i] = sum;
        }

        return sum;
    }
}