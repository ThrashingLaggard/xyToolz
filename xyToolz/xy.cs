using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;


namespace xyToolz
{

    /// <summary>
    /// Little helpers:
    /// 
    /// Repeat  or  reverse   strings
    /// ---
    /// TryCatch for  async  and  sync  methods
    /// ---
    /// Print  on Console
    /// ---
    /// String to Bytes     
    /// ---
    /// Bytes to String
    /// ---
    /// Open Editor 
    ///         -> also with file
    ///         
    ///         ---------------------------
    ///         
    /// "Useless" experiments:
    /// 
    /// Biep
    /// ---
    /// Crash
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Benennungsstile", Justification = "Because XyQol and XYQOL look like shit, and I dont have a better naming idea for my libs yet.")]
    public static class xy
    {
        #region TryCatch – Error-handling helpers

        public static async Task<object> TryCatch(Func<object, object, Task<object>> dangerousMethod, object param1, object param2)
        {
            try
            {
                return await dangerousMethod(param1, param2);
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return null!;
            }
        }
        public static object TryCatch(Func<object, object, object> dangerousMethod, object param1, object param2)
        {
            try
            {
                return dangerousMethod(param1, param2);
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                return null!;
            }
        }
        public static async Task<object> TryCatch(Func<object, Task<object>> dangerousMethod, object param)
        {
            try
            {
                return await dangerousMethod(param);
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return null!;
            }
        }
        public static object TryCatch(Func<object, object> dangerousMethod, object param)
        {
            try
            {
                return dangerousMethod(param);
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                return null!;
            }
        }
        /// <summary>
        /// Wraps a synchronous delegate with exception handling.
        /// </summary>
        public static object TryCatch(Func<object[], object> method, params object[] args)
        {
            try { return method(args); }
            catch (Exception ex) { xyLog.ExLog(ex); return null!; }
        }

        /// <summary>
        /// Wraps an asynchronous delegate with exception handling.
        /// </summary>
        public static async Task<object> TryCatch(Func<object[], Task<object>> method, params object[] args)
        {
            try { return await method(args); }
            catch (Exception ex) { await xyLog.AsxExLog(ex); return null!; }
        }

        /// <summary>
        /// Wraps a parameterless synchronous delegate with exception handling.
        /// </summary>
        public static object TryCatch(Func<object> method)
        {
            try { return method(); }
            catch (Exception ex) { xyLog.ExLog(ex); return null!; }
        }

        /// <summary>
        /// Wraps a parameterless asynchronous delegate with exception handling.
        /// </summary>
        public static async Task<object> TryCatch(Func<Task<object>> method)
        {
            try { return await method(); }
            catch (Exception ex) { await xyLog.AsxExLog(ex); return null!; }
        }

        #endregion

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

        #region QOL – String Utilities

        /// <summary>
        /// Repeats a given string a specified number of times.
        /// </summary>
        /// <param name="text">The text to repeat.</param>
        /// <param name="count">How many times to repeat the string.</param>
        /// <returns>The repeated string.</returns>
        public static string Repeat(string text, ushort count)
        {
            StringBuilder sb = new();
            for (int i = 0; i < count; i++)
            {
                sb.Append(text);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Reverses the characters in a given string.
        /// </summary>
        /// <param name="input">The string to reverse.</param>
        /// <returns>The reversed string.</returns>
        public static string Reverse(string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        /// <summary>
        /// Prints the given message to the console using xyLog.
        /// </summary>
        /// <param name="message">The message to print.</param>
        public static void Print(string message) => xyLog.Log(message);

        #endregion

        #region System Utilities & Experiments

        /// <summary>
        /// Opens Notepad without any file.
        /// </summary>
        public static void Editor()
        {
            Process.Start("notepad.exe");
        }

        /// <summary>
        /// Opens Notepad and loads a specific file.
        /// </summary>
        /// <param name="filePath">The full path to the file to open.</param>
        public static void Editor(string filePath)
        {
            Process.Start("notepad.exe", filePath);
        }

        /// <summary>
        /// Triggers the console beep and logs it.
        /// </summary>
        public static void Piep()
        {
            Console.Beep();
            xyLog.Log("Beep!");
        }

        /// <summary>
        /// A recursive method that tests edge-case control flow with arbitrary branching using goto.
        /// </summary>
        /// <param name="high_number">A large number used to trigger different flow paths.</param>
        /// <returns>Diagnostic message string (not used meaningfully).</returns>
        public static string Crash(UInt128 high_number)
        {
            UInt128 a = 0;
            UInt128 b = 0;
            string output = "";

            if (high_number == 0)
            {
                Console.WriteLine("NEIN");
                return output;
            }

            high_number -= 88;
            Console.WriteLine(high_number);

            if (high_number < 8888) goto loop1;
            return Crash(high_number);

        loop1:
            Console.WriteLine("Hey");
            a++;
            if (b > (high_number / 2))
            {
                Piep();
                Console.WriteLine(a);
            }
            goto loop2;

        loop2:
            Console.WriteLine("Ho");
            if (a > (high_number / 2))
            {
                Piep();
                Console.WriteLine(b);
            }
            b++;
            goto loop1;
        }

        #endregion
    }
}
