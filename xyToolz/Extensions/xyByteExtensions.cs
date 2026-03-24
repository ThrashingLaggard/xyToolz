using System.Text;
using xyToolz.Helper.Logging;


namespace xyToolz.Extensions
{
       public static class xyByteExtensions
        {

            /// <summary>
            /// Converts a byte array into a UTF-8 string.
            /// </summary>
            /// <param name="bytes">The input byte array.</param>
            /// <returns>A UTF-8 string representation of the byte array.</returns>
            public static string ToUtf8( this byte[] bytes)
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
            public static string ToBase(this byte[] bytes)
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
        }    
}
