using System.Text;
using xyToolz.StaticLogging;


namespace xyToolz.Misc
{


    public class xyEncoder()
    {
        #region Encoding – String and Byte Conversion

        /// <summary>
        /// Converts a UTF-8 string into a byte array.
        /// </summary>
        /// <param name="input">The UTF-8 string to convert.</param>
        /// <returns>A byte array representation of the input string.</returns>
        public static byte[] StringToBytes(string input)
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
        public static byte[] BaseToBytes(string base64)
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

        /// <summary>
        /// Converts a byte array into a UTF-8 string.
        /// </summary>
        /// <param name="bytes">The input byte array.</param>
        /// <returns>A UTF-8 string representation of the byte array.</returns>
        public static string BytesToString(byte[] bytes)
        {
            try
            {
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                return null!;
            }
        }

        /// <summary>
        /// Converts a byte array into a Base64-encoded string.
        /// </summary>
        /// <param name="bytes">The input byte array.</param>
        /// <returns>A Base64 string representation of the byte array.</returns>
        public static string BytesToBase(byte[] bytes)
        {
            try
            {
                return Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                return null!;
            }
        }



        #endregion

    }
}