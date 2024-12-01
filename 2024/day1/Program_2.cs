var input = File.ReadAllLines("input.txt");

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