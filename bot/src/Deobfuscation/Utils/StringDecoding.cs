using System.Text;

namespace MoonsecDeobfuscator.Deobfuscation.Utils;

public static class StringDecoding
{
    public static byte[] DecodeEscape(string data)
    {
        var parts = data.Split('\\').Skip(1).ToArray();
        var bytes = new byte[parts.Length];

        for (var i = 0; i < bytes.Length; i++)
            bytes[i] = byte.Parse(parts[i]);

        return bytes;
    }

    public static byte[] Decode(string data, int key)
    {
        using var decoded = new MemoryStream((data.Length - 16) / 2);
        var chars = new Dictionary<char, int>();

        for (var i = 0; i < 16; i++)
            chars[data[i]] = i;

        var len = data.Length;
        var rkey = key;

        for (var i = 16; i < len; i += 2)
        {
            var c1 = data[i];
            var c2 = i + 1 < len ? data[i + 1] : '\0';

            var i1 = chars.ContainsKey(c1) ? chars[c1] : 0;
            var i2 = chars.ContainsKey(c2) ? chars[c2] : 0;

            decoded.WriteByte((byte) ((i1 * 16 + i2 + rkey) % 256));
            rkey += key;
        }

        return decoded.ToArray();
    }

    public static Dictionary<string, string[]> DecodeConstants(byte[] data)
    {
        var constants = new Dictionary<string, string[]>();

        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        while (true)
        {
            var control = reader.ReadByte();

            if (control == 5)
                break;

            if (control == 1)
                control++;

            var size = reader.ReadByte();
            var first = Encoding.UTF8.GetString(reader.ReadBytes(size));

            string[]? value = null;

            switch (control)
            {
                case 0:
                    var strSize = reader.ReadByte();
                    var second = Encoding.UTF8.GetString(reader.ReadBytes(strSize));
                    value = [first, second];
                    break;
                case 2:
                case 4:
                case 6:
                    value = [first];
                    break;
            }

            var key = Encoding.UTF8.GetString(reader.ReadBytes(8));
            constants[key] = value!;
        }

        return constants;
    }

    public static string DecodeConstant(int key, byte[] bytes)
    {
        if (!(bytes.Length > 1 && bytes[0] > 0x7F))
            return Encoding.UTF8.GetString(bytes);

        var newBytes = new byte[bytes.Length - 1];

        for (var i = 1; i < bytes.Length; i++)
            newBytes[i - 1] = (byte) ((bytes[i] + key) % 256);

        return Encoding.UTF8.GetString(newBytes);
    }
}