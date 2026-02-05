using System.Runtime.CompilerServices;

namespace xyToolz.Maths
{
    /// <summary>
    /// Helper for math problems:
    /// 
    /// - Calculate the sum of the digits in any number from sbyte to GUID
    /// 
    /// </summary>
    public static class xyMaths
    {
        #region "Sum of the digits"
        /// <summary>
        /// 
        /// 
        /// Byte (1 Byte = 2 Hex-Ziffern)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHex(byte value) => (value & 0x0F) + ((value >> 4) & 0x0F);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHex(sbyte value) => SumOfHex((byte)value);

        /// <summary>
        /// 
        /// 
        /// Short (2 Bytes = 4 Hex-Ziffern)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHex(short value)
        {
            Span<byte> bytes = stackalloc byte[2];
            BitConverter.TryWriteBytes(bytes, value);
            return SumBytes(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHex(ushort value)
        {
            Span<byte> bytes = stackalloc byte[2];
            BitConverter.TryWriteBytes(bytes, value);
            return SumBytes(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHex(int value)
        {
            uint u = (uint)value;
            // 8 Nibbles (4 Bytes × 2)

            return (int)(u & 0x0F) +
              (int)((u >> 4) & 0x0F) +
              (int)((u >> 8) & 0x0F) +
              (int)((u >> 12) & 0x0F) +
              (int)((u >> 16) & 0x0F) +
              (int)((u >> 20) & 0x0F) +
              (int)((u >> 24) & 0x0F) +
              (int)((u >> 28) & 0x0F);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHex(uint value)
        {
            return (int)(value & 0x0F) +
                    (int)((value >> 4) & 0x0F) +
                    (int)((value >> 8) & 0x0F) +
                    (int)((value >> 12) & 0x0F) +
                    (int)((value >> 16) & 0x0F) +
                    (int)((value >> 20) & 0x0F) +
                    (int)((value >> 24) & 0x0F) +
                    (int)((value >> 28) & 0x0F);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHexDigits(long value)
        {
            ulong u = (ulong)value;
            int sum = 0;

            // 16 Nibbles (8 Bytes × 2)
            for (int i = 0; i < 16; i++)
            {
                sum += (int)((u >> (i * 4)) & 0x0F);
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHexDigits(ulong value)
        {
            int sum = 0;
            for (int i = 0; i < 16; i++)
            {
                sum += (int)((value >> (i * 4)) & 0x0F);
            }
            return sum;
        }

        /// <summary>
        /// 
        /// 
        /// Float (4 Bytes)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHexDigits(float value)
        {
            Span<byte> bytes = stackalloc byte[4];
            BitConverter.TryWriteBytes(bytes, value);
            return SumBytes(bytes);
        }

        /// <summary>
        /// 
        /// 
        /// Double (8 Bytes)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHexDigits(double value)
        {
            Span<byte> bytes = stackalloc byte[8];
            BitConverter.TryWriteBytes(bytes, value);
            return SumBytes(bytes);
        }



        /// <summary>
        /// 
        /// 
        /// Decimal (16 Bytes)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHexDigits(decimal value)
        {
            int[] bits = decimal.GetBits(value);
            int sum = 0;

            foreach (int bit in bits)
            {
                uint u = (uint)bit;
                // 8 Nibbles pro int
                for (int i = 0; i < 8; i++)
                {
                    sum += (int)((u >> (i * 4)) & 0x0F);
                }
            }

            return sum;
        }

        /// <summary>
        /// 
        /// 
        /// GUID (16 Bytes)
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHexDigits(Guid guid)
        {
            Span<byte> bytes = stackalloc byte[16];
            guid.TryWriteBytes(bytes);
            return SumBytes(bytes);
        }

        // Helper-Methode für Byte-Spans
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SumBytes(ReadOnlySpan<byte> bytes)
        {
            int sum = 0;
            foreach (byte b in bytes)
            {
                sum += (b & 0x0F) + ((b >> 4) & 0x0F);
            }
            return sum;
        }
        #endregion

        #region Sum of Decimal Digits

        /// <summary>
        /// Calculates the sum of all decimal digits in a byte value (0-255).
        /// Example: 123 → 1 + 2 + 3 = 6
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDecimalDigits(byte value)
        {
            int sum = 0;
            while (value > 0)
            {
                sum += value % 10;
                value /= 10;
            }
            return sum;
        }

        /// <summary>
        /// Calculates the sum of all decimal digits in an sbyte value (-128 to 127).
        /// Negative signs are ignored. Example: -123 → 1 + 2 + 3 = 6
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDecimalDigits(sbyte value)
        {
            // Manual absolute value to avoid Math.Abs
            int v = value < 0 ? -value : value;
            int sum = 0;
            while (v > 0)
            {
                sum += v % 10;
                v /= 10;
            }
            return sum;
        }

        /// <summary>
        /// Calculates the sum of all decimal digits in a short value.
        /// Negative signs are ignored. Example: -1234 → 1 + 2 + 3 + 4 = 10
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDecimalDigits(short value)
        {
            // Manual absolute value to avoid Math.Abs
            int v = value < 0 ? -value : value;
            int sum = 0;
            while (v > 0)
            {
                sum += v % 10;
                v /= 10;
            }
            return sum;
        }

        /// <summary>
        /// Calculates the sum of all decimal digits in a ushort value.
        /// Example: 65535 → 6 + 5 + 5 + 3 + 5 = 24
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDecimalDigits(ushort value)
        {
            int sum = 0;
            while (value > 0)
            {
                sum += value % 10;
                value /= 10;
            }
            return sum;
        }

        /// <summary>
        /// Calculates the sum of all decimal digits in an int value.
        /// Negative signs are ignored. Example: -12345 → 1 + 2 + 3 + 4 + 5 = 15
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDecimalDigits(int value)
        {
            // Handle int.MinValue edge case: convert to uint to avoid overflow
            if (value == int.MinValue)
            {
                // int.MinValue = -2147483648, digits sum = 2+1+4+7+4+8+3+6+4+8 = 47
                return 47;
            }

            // Manual absolute value
            uint v = value < 0 ? (uint)-value : (uint)value;
            int sum = 0;
            while (v > 0)
            {
                sum += (int)(v % 10);
                v /= 10;
            }
            return sum;
        }

        /// <summary>
        /// Calculates the sum of all decimal digits in a uint value.
        /// Example: 4294967295 → 4 + 2 + 9 + 4 + 9 + 6 + 7 + 2 + 9 + 5 = 57
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDecimalDigits(uint value)
        {
            int sum = 0;
            while (value > 0)
            {
                sum += (int)(value % 10);
                value /= 10;
            }
            return sum;
        }

        /// <summary>
        /// Calculates the sum of all decimal digits in a long value.
        /// Negative signs are ignored.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDecimalDigits(long value)
        {
            // Handle long.MinValue edge case: convert to ulong to avoid overflow
            if (value == long.MinValue)
            {
                // long.MinValue = -9223372036854775808, digits sum = 9+2+2+3+3+7+2+0+3+6+8+5+4+7+7+5+8+0+8 = 103
                return 103;
            }

            // Manual absolute value
            ulong v = value < 0 ? (ulong)-value : (ulong)value;
            int sum = 0;
            while (v > 0)
            {
                sum += (int)(v % 10);
                v /= 10;
            }
            return sum;
        }

        /// <summary>
        /// Calculates the sum of all decimal digits in a ulong value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDecimalDigits(ulong value)
        {
            int sum = 0;
            while (value > 0)
            {
                sum += (int)(value % 10);
                value /= 10;
            }
            return sum;
        }

        /// <summary>
        /// Calculates the sum of all decimal digits in a float value.
        /// Only processes the integer part, ignoring decimals and sign.
        /// Example: -123.456f → 1 + 2 + 3 = 6
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDecimalDigits(float value)
        {
            // Manual truncate and absolute value
            long v = (long)value;
            if (v < 0) v = -v;

            int sum = 0;
            while (v > 0)
            {
                sum += (int)(v % 10);
                v /= 10;
            }
            return sum;
        }

        /// <summary>
        /// Calculates the sum of all decimal digits in a double value.
        /// Only processes the integer part, ignoring decimals and sign.
        /// Example: -123.456 → 1 + 2 + 3 = 6
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDecimalDigits(double value)
        {
            // Manual truncate and absolute value
            long v = (long)value;
            if (v < 0) v = -v;

            int sum = 0;
            while (v > 0)
            {
                sum += (int)(v % 10);
                v /= 10;
            }
            return sum;
        }

        /// <summary>
        /// Calculates the sum of all decimal digits in a decimal value.
        /// Only processes the integer part, ignoring decimals and sign.
        /// Example: -123.456m → 1 + 2 + 3 = 6
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDecimalDigits(decimal value)
        {
            // Manual absolute value and truncation
            if (value < 0) value = -value;
            value = decimal.Truncate(value);

            int sum = 0;
            while (value > 0)
            {
                sum += (int)(value % 10);
                value = decimal.Truncate(value / 10);
            }
            return sum;
        }

        #endregion
    }
}
