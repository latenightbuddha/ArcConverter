namespace ArcConverter
{
    public struct Converters
    {
        public static object Read(BinaryReader reader, bool readShort = false)
        {
            switch (readShort)
            {
                case false:
                    byte[] _byteInt = reader.ReadBytes(4);
                    Array.Reverse(_byteInt);
                    return BitConverter.ToInt32(_byteInt, 0);
                case true:
                    byte[] _byteShort = reader.ReadBytes(2);
                    Array.Reverse(_byteShort);
                    return BitConverter.ToInt16(_byteShort, 0);
            }
        }

        public static void Write(BinaryWriter writer, int value, short int16 = short.MinValue)
        {
            byte[] _byte;
            if (int16 != short.MinValue && value == int16)
            {
                _byte = BitConverter.GetBytes((short)int16);
            }
            else
            {
                _byte = BitConverter.GetBytes(value);
            }
            Array.Reverse(_byte);
            writer.Write(_byte);
        }

        public static void Write(BinaryWriter writer, short value)
        {
            Write(writer, value, value);
        }
    }
}