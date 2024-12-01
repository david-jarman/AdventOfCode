internal class Program_1
{
    private static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");

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