namespace TWN4Api
{
    public static class ByteExtensions
    {
        public static byte[] Add(this byte[] source, byte[] add)
        {
            if (source == null)
            {
                source = add;
                return source;
            }
            byte[] buffer = new byte[source.Length + add.Length];
            
            for (int i = 0; i < source.Length; i++)
                buffer[i] = source[i];
            
            for (int i = source.Length; i < buffer.Length; i++)
                buffer[i] = add[i - source.Length];
            
            return buffer;
        }

        public static byte[] AddByte(this byte[] source, byte add)
        {
            if (source == null) return new byte[] { add };
            byte[] buffer = new byte[source.Length + 1];
            for (int i = 0; i < source.Length; i++)
                buffer[i] = source[i];
            
            buffer[source.Length] = add;
            return buffer;
        }

        public static byte[] Segment(this byte[] source, int index, int count)
        {
            byte[] buffer = new byte[count];
            for (int i = index; i < (index + count); i++)
                buffer[i - index] = source[i];
            return buffer;
        }

        public static bool CompareSegments(this byte[] first, int index1, byte[] second, int index2, int count)
        {
            if (((index1 + count) > first.Length) || ((index2 + count) > second.Length))
                return false;
            
            for (int i = 0; i < count; i++)
                if (first[i + index1] != second[i + index2])
                    return false;
            
            return true;
        }
    }
}