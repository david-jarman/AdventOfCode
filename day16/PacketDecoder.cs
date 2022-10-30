namespace problem_solving;

public class PacketDecoder
{
    public void Solve_Part1()
    {
        string contents = File.ReadAllText("day16/input.txt");
        // string contents = "D2FE28";
        //string contents = "38006F45291200";
        // string contents = "A0016C880162017C3686B18A3D4780";

        byte[] bytes = Convert.FromHexString(contents);

        var bin = new Binary(bytes);

        var pf = new PacketFactory();

        var packet = pf.GetPacket(bin, 0, out int endIndex);

        Console.WriteLine($"Sum of versions: {packet.SummedVersion}");
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
    public Packet GetPacket(Binary bin, int startIndex, out int endIndex)
    {
        int index = startIndex;
        int version = bin.GetNumber(index, 3);
        index += 3;
        int typeId = bin.GetNumber(index, 3);
        index += 3;

        Packet packet;

        if (typeId == 4)
        {
            packet = new LiteralValuePacket(version, typeId, bin, index, out endIndex);
        }
        else
        {
            packet = new OperatorPacket(version, typeId, bin, index, out endIndex);
        }

        return packet;
    }
}

public abstract class Packet
{
    public int Version { get; }

    public int TypeId { get; }

    public virtual int SummedVersion => Version;

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

public class OperatorPacket : Packet
{
    public int LengthTypeId { get; }

    public List<Packet> SubPackets { get; }

    public override int SummedVersion
    {
        get
        {
            int sum = this.Version;
            foreach (var p in SubPackets)
            {
                sum += p.SummedVersion;
            }

            return sum;
        }
    }

    public OperatorPacket(int version, int typeId, Binary bin, int startIndex, out int endIndex)
        : base(version, typeId)
    {
        int index = startIndex;
        LengthTypeId = bin.GetNumber(index++, 1);
        SubPackets = new List<Packet>();

        if (LengthTypeId == 0)
        {
            int subPacketNumBits = bin.GetNumber(index, 15);
            index += 15;
            endIndex = index + subPacketNumBits;

            while (index < endIndex)
            {
                var p = new PacketFactory().GetPacket(bin, index, out index);
                this.SubPackets.Add(p);
            }
        }
        else if (LengthTypeId == 1)
        {
            int numSubPackets = bin.GetNumber(index, 11);
            index += 11;

            int packetsAdded = 0;
            while (packetsAdded < numSubPackets)
            {
                var p = new PacketFactory().GetPacket(bin, index, out index);
                this.SubPackets.Add(p);
                packetsAdded++;
            }
        }

        endIndex = index;
    }
}