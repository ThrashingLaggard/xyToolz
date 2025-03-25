using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz
{

    /// <summary>
    /// Use the Windows Data Protection API to encrypt and decrypt data
    /// </summary>
    public static class xyDataProtectionHelper
    {

        /// <summary>
        /// Use the Windows Data Protection API to encrypt data
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Plattformkompatibilität überprüfen", Justification = "Uses the windows dpapi... so yeah... windows")]
        public static byte[] Protect(string data)
        {
            try
            {
                if (xy.StringBytes(data) is byte[] bytesFromInput)
                {
                    return ProtectedData.Protect(bytesFromInput, null, DataProtectionScope.CurrentUser);
                }
            }
            catch(Exception ex)
            {
                xyLog.ExLog(ex);
            }
            return null!;
        }

        /// <summary>
        /// Use the Windows Data Protection API to decrypt data
        /// </summary>
        /// <param name="protectedData"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Plattformkompatibilität überprüfen", Justification = "Uses the windows dpapi... so yeah... windows")]
        public static string Unprotect(byte[] protectedData)
        {
            try
            {
                byte[] unprotectedBytes =  ProtectedData.Unprotect(protectedData, null, DataProtectionScope.CurrentUser);
                if (xy.ByteString(unprotectedBytes) is string unprotectedString)
                {
                    return unprotectedString;
                }
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
            return null!;
        }


    }
}
