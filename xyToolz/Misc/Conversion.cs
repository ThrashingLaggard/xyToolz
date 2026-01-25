namespace xyToolz.Misc
{
    /// <summary>
    /// Public facade for number system conversions.
    /// No logic – delegates to xyConversion.
    /// </summary>
    public static class Conversion
    {
        public static string X_to_X(int fromBase, string value, int toBase)
              => xyConversion.X_to_X(fromBase, value, toBase);

        public static string DEC_to_X(int number, int targetBase)
            => xyConversion.DEC_to_X(number, targetBase);

        public static string HEX_to_DEC(string number)
            => xyConversion.HEX_to_DEC(number);

        public static string HEX_to_Oct(string number)
            => xyConversion.HEX_to_Oct(number);

        public static string HEX_to_Bin(string number)
            => xyConversion.HEX_to_Bin(number);

        public static string Bin_to_Dec(string number)
            => xyConversion.Bin_to_Dec(number);

        public static string Bin_to_Hex(string number)
            => xyConversion.Bin_to_Hex(number);

        public static string Bin_to_Oct(string number)
            => xyConversion.Bin_to_Oct(number);

        // ⚠️ DIESE ZWEI WERDEN OFT VERGESSEN
        public static string DeLetterer(string input)
            => xyConversion.DeLetterer(input);

        public static string Letterer(string input)
            => xyConversion.Letterer(input);
    }
}
