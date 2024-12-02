namespace CY2024;

public partial class Solutions
{
    public static void Day1_Part1()
    {
        var input = File.ReadAllLines("2024/day1/input.txt");

        List<int> list1 = new();
        Dictionary<int, int> list2 = new();

        foreach (var line in input)
        {
            var num_s = line.Split("   ");

            list1.Add(int.Parse(num_s[0]));
            var val2 = int.Parse(num_s[1]);
            if (list2.TryGetValue(val2, out int count))
                list2[val2] = count + 1;
            else
                list2[val2] = 1;
        }

        int sim_score = 0;

        foreach (var val in list1)
        {
            if (!list2.TryGetValue(val, out var count))
                count = 0;
            sim_score += val * count;
        }

        Console.WriteLine($"Answer2: {sim_score}");
    }

    public static void Day1_Part2()
    {
        var input = File.ReadAllLines("2024/day1/input.txt");

        List<int> list1 = new();
        List<int> list2 = new();

        foreach (var line in input)
        {
            var num_s = line.Split("   ");

            list1.Add(int.Parse(num_s[0]));
            list2.Add(int.Parse(num_s[1]));
        }

        list1.Sort();
        list2.Sort();

        int error_sum = 0;

        for (int i = 0; i < list1.Count; i++)
        {
            error_sum += Math.Abs(list1[i] - list2[i]);
        }

        Console.WriteLine($"Answer: {error_sum}");
    }
}