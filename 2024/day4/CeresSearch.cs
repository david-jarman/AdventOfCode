using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CY2024;

public partial class Solutions
{
    public static void Day4_Part1()
    {
        var input = File.ReadAllLines("2024/day4/input.txt");

        var grid = input.Select(s => s.ToCharArray()).ToArray();
        int rows = grid.Length;
        int cols = grid[0].Length;
        int xmasCount = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                xmasCount += CountXmasFrom(grid, i, j);
            }
        }

        Console.WriteLine($"Count: {xmasCount}");
    }

    private static int CountXmasFrom(char[][] grid, int i, int j)
    {
        int[][] offsets = [[0,1],[1,1],[1,0],[1,-1],[0,-1],[-1,-1],[-1,0],[-1,1],];

        int count = 0;
        foreach (var offset in offsets)
        {
            count += SpellsXmas(grid, i, j, offset[0], offset[1]) ? 1 : 0;
        }

        return count;
    }

    private static bool SpellsXmas(char[][] grid, int i, int j, int i_off, int j_off)
    {
        const string xmas = "XMAS";

        // bounds check
        // check top, bottom, left, right
        int rows = grid.Length;
        int cols = grid[0].Length;

        int i_end = i + (3 * i_off);
        int j_end = j + (3 * j_off);
        if (i_end < 0 || i_end >= rows)
            return false;
        if (j_end < 0 || j_end >= cols)
            return false;

        StringBuilder sb = new();
        for (int iter = 0; iter < 4; iter++)
        {
            sb.Append(grid[i + (iter * i_off)][j + (iter * j_off)]);
        }

        return sb.ToString().Equals(xmas, StringComparison.OrdinalIgnoreCase);
    }
}