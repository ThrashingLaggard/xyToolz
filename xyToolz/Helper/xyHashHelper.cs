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
