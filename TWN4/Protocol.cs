using System.Globalization;

namespace TWN4Api
{
    public class Protocol
    {
        public static byte[] FromPrs(string prs)
        {
            if (string.IsNullOrEmpty(prs)) return null;
            byte[] buffer = new byte[prs.Length / 2];
            for (var i = 0; i < (buffer.Length); i++)
                buffer[i] = byte.Parse(prs.Substring((i * 2), 2), NumberStyles.HexNumber);
            return buffer;
        }
        public static string ToPrs(byte[] prsStream)
        {
            if (prsStream.Length < 1) return null;
            string buffer = null;
            foreach (var t in prsStream)
                buffer += t.ToString("X2");

            return buffer;
        }
    }
}