namespace problem_solving;

public class PacketDecoder
{
    public void Solve_Part1()
    {
        //string contents = File.ReadAllText("day16/input.txt");
        string contents = "D2FE28";

        byte[] bytes = Convert.FromHexString(contents);

        var bin = new Binary(bytes);

        var pf = new PacketFactory();

        var packet = pf.GetPacket(bin, 0, out int endIndex);

        Console.WriteLine(packet);
    }
}

public class Binary
{
    private byte[] _bits;

    public Binary(byte[] bytes)
    {
        _bits = new byte[bytes.Length * 8];
        Length = _bits.Length;

        for (int i = 0; i < bytes.Length; i++)
        {
            byte b = bytes[i];

            for (byte j = 0; j < 8; j++)
            {
                int shiftAmount = 7 - j;
                int mask = 1 << shiftAmount;
                int bit = (b & mask) >> shiftAmount;
                int index = i * 8 + j;
                _bits[index] = (byte)bit;
            }
        }
    }

    public int Length { get; }

    public int GetNumber(int startIndex, int length)
    {
        int value = 0;

        if (length > 32)
        {
            throw new Exception("Can't create int32 with more than 32 bits");
        }

        for (int i = 0; i < length; i++)
        {
            int index = i + startIndex;

            value = (value << 1) | _bits[index];
        }

        return value;
    }
}

public class PacketFactory
{
    public Packet? GetPacket(Binary bin, int startIndex, out int endIndex)
    {
        int version = bin.GetNumber(startIndex, 3);
        int typeId = bin.GetNumber(startIndex + 3, 3);

        Packet packet = null;
        endIndex = -1;

        if (typeId == 4)
        {
            packet = new LiteralValuePacket(version, typeId, bin, startIndex + 6, out endIndex);
        }

        return packet;
    }
}

public abstract class Packet
{
    public int Version { get; }

    public int TypeId { get; }

    public Packet(int version, int typeId)
    {
        Version = version;
        TypeId = typeId;
    }

    public override string ToString()
    {
        return $"Version: {Version}, TypeId: {TypeId}";
    }
}

public class LiteralValuePacket : Packet
{
    public LiteralValuePacket(int version, int typeId, Binary bin, int startIndex, out int endIndex)
        : base(version, typeId)
    {
        int index = startIndex;
        bool keepParsing = true;

        int value = 0;
        do
        {
            keepParsing = bin.GetNumber(index, 1) == 1;
            index++;

            value = (value << 4) | bin.GetNumber(index, 4);
            index += 4;
        }
        while (keepParsing);

        endIndex = index;

        Value = value;
    }

    public int Value { get; }
}