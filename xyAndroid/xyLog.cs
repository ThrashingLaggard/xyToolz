using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.Util;

namespace xyAndroid
{
    public static class xyLog
    {
            private const string Tag = "LogcatLibrary";

            public static void Debug(string message)
            {
                  Log.Debug(Tag, message);
            }

            public static void Info(string message)
            {
                  Log.Info(Tag, message);
            }

            public static void Warn(string message)
            {
                  Log.Warn(Tag, message);
            }

            public static void Error(string message)
            {
                  Log.Error(Tag, message);
            }

      }
}
