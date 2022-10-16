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

        List<int> spawnedFish = new List<int>();

        //const int days = 18;
        const int days = 80;
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
        }

        Console.WriteLine($"School size: {lanternFish.Count}");
    }
}