namespace problem_solving;

public class Bingo
{
    public void Solve()
    {
        string fullInput = File.ReadAllText("day4/input.txt");

        string[] stringChunks = fullInput.Split("\n\n");

        int[] randomNumbers = stringChunks[0]
            .Split(',')
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => int.Parse(s))
            .ToArray();

        List<BingoBoard> boards = new List<BingoBoard>();

        for (int i = 1; i < stringChunks.Length; i++)
        {
            var rows = stringChunks[i].Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            var bingoBoard = new BingoBoard(rows);
            boards.Add(bingoBoard);
        }

        int lastBoardScore = 0;
        foreach (int calledNumber in randomNumbers)
        {
            List<int> boardIndexesToRemove = new List<int>();
            int iter = 0;
            foreach(var board in boards)
            {
                if (board.Blot(calledNumber))
                {
                    // winner
                    lastBoardScore = board.Score() * calledNumber;

                    // need to remove the board at this point.
                    boardIndexesToRemove.Add(iter);

                    // Console.WriteLine($"We have a winner. Score: {score}");
                    // Console.WriteLine($"The answer to the puzzle is {score * calledNumber}");
                    // return;
                }
                iter++;
            }

            // remove boards from last to start, to avoid messing with iterators as boards are removed :)
            boardIndexesToRemove.Sort();
            boardIndexesToRemove.Reverse();

            foreach (int i in boardIndexesToRemove)
            {
                boards.RemoveAt(i);
            }
        }

        Console.WriteLine($"The last board to score had a score of: {lastBoardScore}");
    }
}

public class BingoBoard
{
    private int[][] _board;

    private bool[][] _markedBoard;

    private Dictionary<int, (int i, int j)> _numberToPosMap;

    public BingoBoard(string[] rows)
    {
        int iter = 0;
        _board = new int[5][];
        _markedBoard = new bool[5][];
        _numberToPosMap = new Dictionary<int, (int i, int j)>();

        foreach (string row in rows)
        {
            _markedBoard[iter] = new bool[5];
            _board[iter] =  row.Split(' ')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(num => int.Parse(num))
                .ToArray();

            for (int j = 0; j < _board[iter].Length; j++)
            {
                int num = _board[iter][j];
                _numberToPosMap[num] = (iter, j);
                _markedBoard[iter][j] = false;
            }

            iter++;
        }
    }

    public bool Blot(int number)
    {
        if (!_numberToPosMap.TryGetValue(number, out var position))
        {
            return false;
        }

        _markedBoard[position.i][position.j] = true;

        return IsWinner(position);
    }

    public int Score()
    {
        int score = 0;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (!_markedBoard[i][j])
                {
                    score += _board[i][j];
                }
            }
        }

        return score;
    }

    private bool IsWinner((int i, int j) position)
    {
        // check row
        bool rowMarked = true;
        for (int i = 0; i < 5; i++)
        {
            rowMarked = _markedBoard[i][position.j];
            if (!rowMarked)
            {
                break;
            }
        }

        if (rowMarked)
        {
            return true;
        }
        
        // check col
        bool colMarked = true;
        for (int j = 0; j < 5; j++)
        {
            colMarked = _markedBoard[position.i][j];
            if (!colMarked)
            {
                break;
            }
        }

        return colMarked;
    }
}