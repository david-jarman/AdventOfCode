namespace problem_solving;

public class Folding
{
    public void Solve_Part1()
    {
        var input = ParseInput(false);
        bool[,] matrix = input.matrix;
        (int x, int y)[] foldInstruction = input.foldInstruction;

        foreach (var instr in foldInstruction)
        {
            int x_size, y_size = 0;
            if (instr.x > 0)
            {
                x_size = matrix.GetLength(0) / 2;
                y_size = matrix.GetLength(1);
            }
            else
            {
                x_size = matrix.GetLength(0);
                y_size = matrix.GetLength(1) / 2;
            }

            bool[,] next = new bool[x_size, y_size];
            for (int y = 0; y < y_size; y++)
            {
                for (int x = 0; x < x_size; x++)
                {
                    int matrix_x_fold = instr.x > 0 ? matrix.GetLength(0) - x - 1 : x;
                    int matrix_y_fold = instr.y > 0 ? matrix.GetLength(1) - y - 1 : y;
                    next[x, y] = matrix[x, y] || matrix[matrix_x_fold, matrix_y_fold];
                }
            }

            matrix = next;
        }

        Console.WriteLine("Printing matrix");
        for (int y = 0; y < matrix.GetLength(1); y++)
        {
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                string c = matrix[x,y] ? "#" : ".";
                Console.Write(c);
            }
            Console.WriteLine();
        }
    }

    private (bool[,] matrix, (int x, int y)[] foldInstruction) ParseInput(bool useTestInput)
    {
        string fileName = useTestInput ? "day13/test_input.txt" : "day13/input.txt";

        string[] fileContents = File.ReadAllLines(fileName);

        var dots_s = fileContents.TakeWhile(s => !string.IsNullOrWhiteSpace(s));
        var folds_s = fileContents.TakeLast(fileContents.Length - dots_s.Count() - 1);
        
        (int x, int y)[] dots = dots_s.Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Split(',').Select(x => int.Parse(x)).ToArray())
            .Select(i => (i[0], i[1]))
            .ToArray();
        int max_x = dots.Max(k => k.x) + 1;
        int max_y = dots.Max(k => k.y) + 2;

        bool[,] matrix = new bool[max_x, max_y];

        foreach (var dot in dots)
        {
            matrix[dot.x, dot.y] = true;
        }

        var folds = new (int x, int y)[folds_s.Count()];
        int i = 0;
        foreach (var fold in folds_s)
        {
            var fold_clean = fold.Remove(0, 11);
            var fold_i = fold_clean.Split('=');
            switch (fold_i[0])
            {
                case "x":
                    folds[i] = (int.Parse(fold_i[1]), 0);
                    break;
                case "y":
                    folds[i] = (0, int.Parse(fold_i[1]));
                    break;
                default:
                    throw new Exception();
            }

            i++;
        }
        

        return (matrix, folds);
    }
}