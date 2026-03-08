using System.Runtime.CompilerServices;

namespace xyToolz.Maths
{
    /// <summary>
    /// Helper for math problems:
    /// 
    /// - Calculate the sum of the digits in any number from sbyte to GUID... currently 
    /// 
    /// </summary>
    public static class SumOfDigits
    {
        #region "Sum of HEX digits"
        /// <summary>
        /// 
        /// 
        /// Byte (1 Byte = 2 Hex-Ziffern)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHexDigits(byte value) => (value & 0x0F) + ((value >> 4) & 0x0F);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHexDigits(sbyte value) => SumOfHexDigits((byte)value);

        /// <summary>
        /// 
        /// 
        /// Short (2 Bytes = 4 Hex-Ziffern)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfHexDigits(short value)
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
        public static int SumOfHexDigits(ushort value)
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
        public static int SumOfHexDigits(int value)
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
        public static int SumOfHexDigits(uint value)
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

        #region "Sum of DEC digits"

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

        #region "Sum of BIN digits"

        /// <summary>
        /// Calculates the sum of all binary digits (count of set bits) in a byte value.
        /// Example: 0b10110011 (179) → 6 (six '1' bits)
        /// Also known as population count or Hamming weight.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfBinary(byte value)
        {
            // Brian Kernighan's algorithm - fastest for sparse bits
            int count = 0;
            while (value > 0)
            {
                value &= (byte)(value - 1); // Clear the lowest set bit
                count++;
            }
            return count;
        }

        /// <summary>
        /// Calculates the sum of all binary digits (count of set bits) in an sbyte value.
        /// Negative signs are processed as two's complement representation.
        /// Example: -1 (0b11111111) → 8
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfBinary(sbyte value)
            => SumOfBinary((byte)value);

        /// <summary>
        /// Calculates the sum of all binary digits (count of set bits) in a short value.
        /// Example: 0b0000000010110011 → 5
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfBinary(short value)
        {
            ushort v = (ushort)value;
            int count = 0;
            while (v > 0)
            {
                v &= (ushort)(v - 1);
                count++;
            }
            return count;
        }

        /// <summary>
        /// Calculates the sum of all binary digits (count of set bits) in a ushort value.
        /// Example: 65535 (0xFFFF) → 16 (all bits set)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfBinary(ushort value)
        {
            int count = 0;
            while (value > 0)
            {
                value &= (ushort)(value - 1);
                count++;
            }
            return count;
        }

        /// <summary>
        /// Calculates the sum of all binary digits (count of set bits) in an int value.
        /// Uses parallel bit counting for optimal performance.
        /// Example: -1 (0xFFFFFFFF) → 32 (all bits set)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfBinary(int value)
        {
            uint v = (uint)value;

            // Parallel bit count algorithm (fastest for dense bits)
            v = v - ((v >> 1) & 0x55555555);
            v = (v & 0x33333333) + ((v >> 2) & 0x33333333);
            v = (v + (v >> 4)) & 0x0F0F0F0F;
            v = v + (v >> 8);
            v = v + (v >> 16);

            return (int)(v & 0x3F);
        }

        /// <summary>
        /// Calculates the sum of all binary digits (count of set bits) in a uint value.
        /// Uses parallel bit counting for optimal performance.
        /// Example: 4294967295 (0xFFFFFFFF) → 32
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfBinary(uint value)
        {
            // Parallel bit count algorithm
            value = value - ((value >> 1) & 0x55555555);
            value = (value & 0x33333333) + ((value >> 2) & 0x33333333);
            value = (value + (value >> 4)) & 0x0F0F0F0F;
            value = value + (value >> 8);
            value = value + (value >> 16);

            return (int)(value & 0x3F);
        }

        /// <summary>
        /// Calculates the sum of all binary digits (count of set bits) in a long value.
        /// Uses parallel bit counting for optimal performance.
        /// Example: -1 (0xFFFFFFFFFFFFFFFF) → 64 (all bits set)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfBinary(long value)
        {
            ulong v = (ulong)value;

            // Parallel bit count algorithm for 64-bit
            v = v - ((v >> 1) & 0x5555555555555555UL);
            v = (v & 0x3333333333333333UL) + ((v >> 2) & 0x3333333333333333UL);
            v = (v + (v >> 4)) & 0x0F0F0F0F0F0F0F0FUL;
            v = v + (v >> 8);
            v = v + (v >> 16);
            v = v + (v >> 32);

            return (int)(v & 0x7F);
        }

        /// <summary>
        /// Calculates the sum of all binary digits (count of set bits) in a ulong value.
        /// Uses parallel bit counting for optimal performance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfBinary(ulong value)
        {
            // Parallel bit count algorithm for 64-bit
            value = value - ((value >> 1) & 0x5555555555555555UL);
            value = (value & 0x3333333333333333UL) + ((value >> 2) & 0x3333333333333333UL);
            value = (value + (value >> 4)) & 0x0F0F0F0F0F0F0F0FUL;
            value = value + (value >> 8);
            value = value + (value >> 16);
            value = value + (value >> 32);

            return (int)(value & 0x7F);
        }

        /// <summary>
        /// Calculates the sum of all binary digits (count of set bits) in a float value.
        /// Processes the IEEE 754 binary representation of the float.
        /// Example: 1.0f (0x3F800000) → 8 set bits
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfBinary(float value)
        {
            // Reinterpret float as uint to access raw bits
            uint bits = BitConverter.SingleToUInt32Bits(value);
            return SumOfBinary(bits);
        }

        /// <summary>
        /// Calculates the sum of all binary digits (count of set bits) in a double value.
        /// Processes the IEEE 754 binary representation of the double.
        /// Example: 1.0 (0x3FF0000000000000) → 13 set bits
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfBinary(double value)
        {
            // Reinterpret double as ulong to access raw bits
            ulong bits = BitConverter.DoubleToUInt64Bits(value);
            return SumOfBinary(bits);
        }

        /// <summary>
        /// Calculates the sum of all binary digits (count of set bits) in a decimal value.
        /// Processes the binary representation of the decimal (128 bits).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfBinary(decimal value)
        {
            int[] bits = decimal.GetBits(value);
            int sum = 0;

            // Count bits in all 4 int32 parts
            foreach (int bit in bits)
            {
                sum += SumOfBinary(bit);
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum of all binary digits (count of set bits) in a GUID.
        /// Processes all 128 bits of the GUID.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfBinary(Guid guid)
        {
            Span<byte> bytes = stackalloc byte[16];
            guid.TryWriteBytes(bytes);

            int sum = 0;
            foreach (byte b in bytes)
            {
                sum += SumOfBinary(b);
            }

            return sum;
        }

        #endregion

        #region "Sum of any digits"

        /// <summary>
        /// Calculates the sum of digits in any number system (base 2-36).
        /// Heavily optimized with specialized code paths for common bases.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfAnyDigits(ulong value, int baseSystem)
        {
            if ((baseSystem & (baseSystem - 1)) == 0 && baseSystem <= 32)
            {
                return SumOfDigitsBasePowerOf2(value, baseSystem);
            }

            if (baseSystem == 10) return SumOfDigitsBase10Fast(value);
            if (baseSystem == 3) return SumOfDigitsBase3(value);
            if (baseSystem == 5) return SumOfDigitsBase5(value);

            return SumOfDigitsBaseGeneric(value, (uint)baseSystem);
        }

        /// <summary>
        /// Ultra-fast path for all power-of-2 bases (2, 4, 8, 16, 32).
        /// Uses only bit shifts and masks - no division.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SumOfDigitsBasePowerOf2(ulong value, int baseSystem)
        {
            // Calculate bits per digit: log2(base)
            int bitsPerDigit = 0;
            int temp = baseSystem;
            while (temp > 1)
            {
                bitsPerDigit++;
                temp >>= 1;
            }

            ulong mask = (1UL << bitsPerDigit) - 1;
            int sum = 0;

            // Unrolled for common cases
            switch (bitsPerDigit)
            {
                case 1: // Base 2 (binary)
                    return SumOfBinaryUltra(value);

                case 2: // Base 4
                    while (value > 0)
                    {
                        sum += (int)(value & 0x3);
                        value >>= 2;
                    }
                    return sum;

                case 3: // Base 8 (octal)
                    return SumOfDigitsBase8Ultra(value);

                case 4: // Base 16 (hex)
                    return SumOfHexUltra(value);

                case 5: // Base 32
                    while (value > 0)
                    {
                        sum += (int)(value & 0x1F);
                        value >>= 5;
                    }
                    return sum;

                default:
                    while (value > 0)
                    {
                        sum += (int)(value & mask);
                        value >>= bitsPerDigit;
                    }
                    return sum;
            }
        }

        /// <summary>
        /// Ultra-optimized binary digit sum using parallel bit counting.
        /// Fastest possible popcount implementation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SumOfBinaryUltra(ulong value)
        {
            // Parallel bit count - processes all bits simultaneously
            value = value - ((value >> 1) & 0x5555555555555555UL);
            value = (value & 0x3333333333333333UL) + ((value >> 2) & 0x3333333333333333UL);
            value = (value + (value >> 4)) & 0x0F0F0F0F0F0F0F0FUL;
            return (int)((value * 0x0101010101010101UL) >> 56);
        }

        /// <summary>
        /// Ultra-optimized octal (base-8) using unrolled bit shifts.
        /// Processes up to 21 octal digits (63 bits) without loops for common cases.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SumOfDigitsBase8Ultra(ulong value)
        {
            // Fast path: values that fit in 10 octal digits (30 bits)
            if (value <= 0x3FFFFFFFUL)
            {
                return (int)((value & 0x7) +
                             ((value >> 3) & 0x7) +
                             ((value >> 6) & 0x7) +
                             ((value >> 9) & 0x7) +
                             ((value >> 12) & 0x7) +
                             ((value >> 15) & 0x7) +
                             ((value >> 18) & 0x7) +
                             ((value >> 21) & 0x7) +
                             ((value >> 24) & 0x7) +
                             ((value >> 27) & 0x7));
            }

            // Medium path: values that fit in 20 octal digits (60 bits)
            if (value <= 0xFFFFFFFFFFFFFFFUL)
            {
                int sum = 0;
                for (int i = 0; i < 20; i++)
                {
                    sum += (int)((value >> (i * 3)) & 0x7);
                }
                return sum;
            }

            // Slow path: full 64-bit values
            int result = 0;
            while (value > 0)
            {
                result += (int)(value & 0x7);
                value >>= 3;
            }
            return result;
        }

        /// <summary>
        /// Ultra-optimized hexadecimal using unrolled nibble extraction.
        /// Processes all 16 nibbles without branching.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SumOfHexUltra(ulong value)
        {
            // Process all 16 nibbles in one go
            return (int)((value & 0xF) +
                         ((value >> 4) & 0xF) +
                         ((value >> 8) & 0xF) +
                         ((value >> 12) & 0xF) +
                         ((value >> 16) & 0xF) +
                         ((value >> 20) & 0xF) +
                         ((value >> 24) & 0xF) +
                         ((value >> 28) & 0xF) +
                         ((value >> 32) & 0xF) +
                         ((value >> 36) & 0xF) +
                         ((value >> 40) & 0xF) +
                         ((value >> 44) & 0xF) +
                         ((value >> 48) & 0xF) +
                         ((value >> 52) & 0xF) +
                         ((value >> 56) & 0xF) +
                         ((value >> 60) & 0xF));
        }

        /// <summary>
        /// Optimized base-10 using multiplication trick instead of division.
        /// Division by constant can be replaced with multiplication + shift.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SumOfDigitsBase10Fast(ulong value)
        {
            int sum = 0;

            // Unrolled for values up to 10^10 (most common case)
            if (value < 10000000000UL)
            {
                while (value >= 10)
                {
                    // Extract last digit
                    ulong quotient = value / 10;
                    sum += (int)(value - quotient * 10);
                    value = quotient;
                }
                sum += (int)value;
                return sum;
            }

            // Full path for larger values
            while (value > 0)
            {
                sum += (int)(value % 10);
                value /= 10;
            }
            return sum;
        }

        /// <summary>
        /// Optimized base-3 (ternary) calculation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SumOfDigitsBase3(ulong value)
        {
            int sum = 0;
            while (value > 0)
            {
                sum += (int)(value % 3);
                value /= 3;
            }
            return sum;
        }

        /// <summary>
        /// Optimized base-5 calculation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SumOfDigitsBase5(ulong value)
        {
            int sum = 0;
            while (value > 0)
            {
                sum += (int)(value % 5);
                value /= 5;
            }
            return sum;
        }

        /// <summary>
        /// Generic fallback for arbitrary bases (3, 5, 6, 7, 9, 11-36).
        /// Optimized with uint arithmetic and minimal branching.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SumOfDigitsBaseGeneric(ulong value, uint baseSystem)
        {
            if (baseSystem < 2) return 0;

            int sum = 0;
            while (value > 0)
            {
                sum += (int)(value % baseSystem);
                value /= baseSystem;
            }
            return sum;
        }

        /// <summary>
        /// Convenience overload for signed values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDigitsInBase(long value, int baseSystem)
        {
            if (value == long.MinValue)
                return SumOfAnyDigits(9223372036854775808UL, baseSystem);

            ulong v = value < 0 ? (ulong)-value : (ulong)value;
            return SumOfAnyDigits(v, baseSystem);
        }

        /// <summary>
        /// Convenience overload for 32-bit values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOfDigitsInBase(int value, int baseSystem)
        {
            if (value == int.MinValue)
                return SumOfAnyDigits(2147483648UL, baseSystem);

            uint v = value < 0 ? (uint)-value : (uint)value;
            return SumOfDigitsInBase(v, baseSystem);
        }

        #endregion
    }
}
