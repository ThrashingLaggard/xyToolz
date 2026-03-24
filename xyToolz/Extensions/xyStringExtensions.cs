using System.Text;
using xyToolz.Helper.Logging;


namespace xyToolz.Extensions
{
        public static class xyStringExtensions
        {
                /// <summary>
                /// Repeating a given string a specified number of times.
                /// </summary>
                /// <remarks>
                /// Use char overload for way better performance
                /// </remarks>
                /// <param name="text">The text to repeat.</param>
                /// <param name="count">The end amount.</param>
                /// <returns>The repeated string.</returns>
                public static string Repeat(this string text, ushort count)
                {
                    StringBuilder sb_Repeater = new();
                    for (int i = 0; i < count; i++)
                    {
                        sb_Repeater.Append(text);
                    }
                    return sb_Repeater.ToString();
                }

            /// <summary>
            /// Reverses the characters in a given string.
            /// </summary>
            /// <remarks>
            /// This variant appearently (*Stack Overflow) also reverses the unicode modifiers in character, thus if in doubt, use the Unicode variant of this, utilizing a StringBuilder
            /// </remarks>
            /// <param name="input">The string to reverse.</param>
            /// <returns>The reversed string.</returns>
            public static string Reverse(this string input)
            {
                char[] arr_InputChars = input.ToCharArray();
                Array.Reverse(arr_InputChars);
                return new (arr_InputChars);
            }

            /// <summary>
            /// Reverses the characters in a given string.
            /// </summary>
            /// <remarks>
            /// This variant is appearently safe for usage with unicode
            /// </remarks>
            /// <param name="input">The string to reverse.</param>
            /// <returns>The reversed string.</returns>
            public static string ReverseUnicode(this string input)
            {
                StringBuilder reverseBuilder = new (input.Length);
                for (int i = input.Length -1; i >= 0; i--)  // unter vorbehalt
                {
                    reverseBuilder.Append(input[i]);
                }
                return reverseBuilder.ToString();
            }


            /// <summary>
            /// Converts a UTF-8 string into a byte array.
            /// </summary>
            /// <param name="input">The UTF-8 string to convert.</param>
            /// <returns>A byte array representation of the input string.</returns>
            public static byte[] ToBytes(this string input)
            {
                try
                {
                    return Encoding.UTF8.GetBytes(input);
                }
                catch (Exception ex)
                {
                    xyLog.ExLog(ex);
                    return null!;
                }
            }

            /// <summary>
            /// Converts a Base64-encoded string into a byte array.
            /// </summary>
            /// <param name="base64">The Base64-encoded input string.</param>
            /// <returns>The decoded byte array.</returns>
            public static byte[] BaseToBytes(this string base64)
            {
                try
                {
                    return Convert.FromBase64String(base64);
                }
                catch (Exception ex)
                {
                    xyLog.ExLog(ex);
                    return null!;
                }
            }



    }
}
