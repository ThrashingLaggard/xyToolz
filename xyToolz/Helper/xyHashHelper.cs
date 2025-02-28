using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Helper
{
      public static class xyHashHelper
      {
            public static string Pepper { get; set; }
            private const int SaltLength = 16;  // 128 Bit
            private const int KeyLenth = 32;    // 256 Bit
            private const int Iterations = 1000;

            public static string HashPassword(string password)
            {
                  string hashedPassword = "";
                  byte [ ] salt = RandomNumberGenerator.GetBytes (SaltLength);




                  return hashedPassword;
            }

    }
}

//public class Passwords
//{
//      private const int SaltSize = 16; // 128-bit
//      private const int KeySize = 32;  // 256-bit
//      private const int Iterations = 10000;

//      /// <summary>
//      /// 
//      /// </summary>
//      /// <param name="password"></param>
//      /// <returns></returns>
//      public static string HashPassword ( string password )
//      {
//            byte [ ] salt = RandomNumberGenerator.GetBytes (SaltSize);

//            using var pbkdf2 = new Rfc2898DeriveBytes (password, salt, Iterations, HashAlgorithmName.SHA256);
//            byte [ ] hash = pbkdf2.GetBytes (KeySize);
//            return Convert.ToBase64String (salt) + ":" + Convert.ToBase64String (hash);
//      }

//      /// <summary>
//      /// 
//      /// </summary>
//      /// <param name="password"></param>
//      /// <param name="hashedPassword"></param>
//      /// <returns></returns>
//      public static bool VerifyPassword ( string password, string hashedPassword )
//      {
//            string [ ] parts = hashedPassword.Split (':');
//            if (parts.Length != 2)
//            {
//                  return false;
//            }

//            byte [ ] salt = Convert.FromBase64String (parts [ 0 ]);
//            byte [ ] hash = Convert.FromBase64String (parts [ 1 ]);

//            using var pbkdf2 = new Rfc2898DeriveBytes (password, salt, Iterations, HashAlgorithmName.SHA256);
//            byte [ ] hashToCheck = pbkdf2.GetBytes (KeySize);
//            return CryptographicOperations.FixedTimeEquals (hash, hashToCheck);
//      }
//}
