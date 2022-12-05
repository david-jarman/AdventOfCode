namespace problem_solving;

public class BeaconScanner
{
    public void Solve_Part1()
    {
        var scanners = ParseInput("day19/test_input.txt");

        var relatives = scanners[0].Beacons[0].GetRelativePositions();
    }

    private List<Scanner> ParseInput(string fileName)
    {
        var lines = File.ReadAllLines(fileName);
        List<Scanner> scanners = new();

        Scanner? scanner = null;
        foreach (string line in lines)
        {
            if (line.StartsWith("---"))
            {
                scanner = new();
                scanners.Add(scanner);
                continue;
            }
            else if (scanner is null)
            {
                throw new Exception();
            }

            int[] coords = line.Split(',').Select(s => int.Parse(s)).ToArray();
            scanner.Beacons.Add(new Position(coords));
        }

        return scanners;
    }
}

public class Scanner
{
    public List<Position> Beacons { get; set; } = new();
}

// Compare two scanners beacon locations
// Calculate the 24 position orientations
// Scanner 0 and scanner 1
// Take one scanner from 1 and normalize it against one from scanner 0
// Test positions and see if 12 match
// Do this for each beacon in 0 and 1
// # Beacons for scanner 1 * # Beacons for scanner 2 * 24 * 24
public class Position
{
    public Position(int[] coords)
    {
        if (coords.Length != 3)
        {
            throw new Exception();
        }
        Coords = coords;
    }

    public int X => this.Coords[0];
    public int Y => this.Coords[1];
    public int Z => this.Coords[2];

    public int[] Coords { get; }

    public override string ToString()
    {
        return $"{X},{Y},{Z}";
    }

    public List<Position> GetRelativePositions()
    {
        List<Position> positions = new(24);

        // face one direction, keeping it the same
        // rotate the other directions according to a shift in perspective on what is up

        // Facing positive X
        positions.Add(new Position(new int[] { X,  Y,  Z }));
        positions.Add(new Position(new int[] { X, -Z,  Y }));
        positions.Add(new Position(new int[] { X, -Y, -Z }));
        positions.Add(new Position(new int[] { X,  Z, -Y }));

        // Facing negative X
        positions.Add(new Position(new int[] { -X, -Y,  Z }));
        positions.Add(new Position(new int[] { -X, -Z, -Y }));
        positions.Add(new Position(new int[] { -X,  Y, -Z }));
        positions.Add(new Position(new int[] { -X,  Z,  Y }));

        // Facing positive Y
        positions.Add(new Position(new int[] {  X, Y,  Z }));
        positions.Add(new Position(new int[] {  X, Y,  Z }));
        positions.Add(new Position(new int[] {  X, Y,  Z }));
        positions.Add(new Position(new int[] {  X, Y,  Z }));

        // Facing negative Y
        positions.Add(new Position(new int[] { -Y, -X, Z }));
        positions.Add(new Position(new int[] { -Y, Z, -X }));
        positions.Add(new Position(new int[] { -Y, X, Z }));
        positions.Add(new Position(new int[] { -Y, -Z, -X }));

        // Facing positive Z
        positions.Add(new Position(new int[] { Z, X, Y }));
        positions.Add(new Position(new int[] { Z, Y, X }));
        positions.Add(new Position(new int[] { Z, -X, Y }));
        positions.Add(new Position(new int[] { Z, -Y, X }));

        // Facing negative Z
        positions.Add(new Position(new int[] { -Z, -X, Y }));
        positions.Add(new Position(new int[] { -Z, Y, -X }));
        positions.Add(new Position(new int[] { -Z, X, Y }));
        positions.Add(new Position(new int[] { -Z, -Y, -X }));

        return positions;
    }
}