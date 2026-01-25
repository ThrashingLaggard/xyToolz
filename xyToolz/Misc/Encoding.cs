namespace xyToolz.Misc
{
    /// <summary>
    /// Facade for encoding and byte/string conversions.
    /// Contains no logic – delegates to xyEncoder.
    /// </summary>
    public static class Encodings
    {
        public static byte[] StringToBytes(string input)
            => xyEncoder.StringToBytes(input);

        public static byte[] BaseToBytes(string base64)
            => xyEncoder.BaseToBytes(base64);

        public static string BytesToString(byte[] bytes)
            => xyEncoder.BytesToString(bytes);

        public static string BytesToBase(byte[] bytes)
            => xyEncoder.BytesToBase(bytes);
    }
}
