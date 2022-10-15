/*
// Day 2, part 1
// https://adventofcode.com/2021/day/2#part1
var input = File.ReadAllLines("input.txt")
    .Where(s => !string.IsNullOrWhiteSpace(s))
    .ToArray();

int horizontal = 0, depth = 0;

foreach (string command in input)
{
    var commandPair = command.Split(' ');
    if (commandPair.Length != 2)
    {
        Console.WriteLine($"Error, malformed command: {command}");
        return -1;
    }

    string operation = commandPair[0];
    int distance = int.Parse(commandPair[1]);

    switch (operation)
    {
        case "forward":
            horizontal += distance;
            break;
        case "down":
            depth += distance;
            break;
        case "up":
            depth -= distance;
            break;

        default:
            Console.WriteLine($"Error, invalid command: {command}");
            return -1;
    }
}

Console.WriteLine($"Horizontal: {horizontal}");
Console.WriteLine($"Depth: {depth}");

Console.WriteLine($"Multiple: {depth * horizontal}");

return 0;

*/

// Day 2, part 2
// https://adventofcode.com/2021/day/2#part2
var input = File.ReadAllLines("input.txt")
    .Where(s => !string.IsNullOrWhiteSpace(s))
    .ToArray();

int horizontal = 0, depth = 0, aim = 0;

foreach (string command in input)
{
    var commandPair = command.Split(' ');
    if (commandPair.Length != 2)
    {
        Console.WriteLine($"Error, malformed command: {command}");
        return -1;
    }

    string operation = commandPair[0];
    int distance = int.Parse(commandPair[1]);

    switch (operation)
    {
        case "forward":
            horizontal += distance;
            depth += (aim * distance);
            break;
        case "down":
            aim += distance;
            break;
        case "up":
            aim -= distance;
            break;

        default:
            Console.WriteLine($"Error, invalid command: {command}");
            return -1;
    }
}

Console.WriteLine($"Horizontal: {horizontal}");
Console.WriteLine($"Depth: {depth}");

Console.WriteLine($"Multiple: {depth * horizontal}");

return 0;