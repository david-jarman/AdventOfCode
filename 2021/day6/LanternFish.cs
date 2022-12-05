namespace problem_solving;

public class LanternFish
{
    public void Solve()
    {
        //string input = File.ReadAllText("day6/test_input.txt");
        string input = File.ReadAllText("day6/input.txt");

        List<int> lanternFish = input
            .Split(',')
            .Select(s => int.Parse(s))
            .ToList();

        //const int days = 80;
        const int days = 256;

        // Array representing the count of fish in each "age", 0 -> 8
        long[] populationAges = new long[9];

        // Initialize based on input
        foreach (int fishAge in lanternFish)
        {
            populationAges[fishAge]++;
        }

        // Simulate each day
        for (int i = 0; i < days; i++)
        {
            // The age 0 fish are the repoducers
            long reproducers = populationAges[0];

            // Age all fish by 1 day by shifting them down one index, and add reproduced fish to the end
            populationAges = populationAges.Skip(1).Take(8).Append(reproducers).ToArray();

            // Reset the age of the reproducers back to 6
            populationAges[6] += reproducers;
        }

        Console.WriteLine($"Total count: {populationAges.Sum()}");
    }

    public void Simulate(List<int> lanternFish, int days)
    {
        List<int> spawnedFish = new List<int>();

        for (int day = 0; day < days; day++)
        {
            // simulate
            for (int i = 0; i < lanternFish.Count; i++)
            {
                int fish = lanternFish[i];
                if (fish == 0)
                {
                    lanternFish[i] = 6;
                    spawnedFish.Add(8);
                }
                else
                {
                    lanternFish[i] -= 1;
                }
            }

            // Add spawned fish
            lanternFish.AddRange(spawnedFish);
            spawnedFish = new List<int>();

            Console.WriteLine($"Day: {day+1}, Count: {lanternFish.Count}, School: {string.Join(',', lanternFish)}");
        }

        Console.WriteLine($"School size: {lanternFish.Count}");
    }
}