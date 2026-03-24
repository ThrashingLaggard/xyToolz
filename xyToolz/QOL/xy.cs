using System.Diagnostics;
using System.Runtime.CompilerServices;
using xyToolz.Helper.Logging;


namespace xyToolz.QOL
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
#pragma warning disable CS8981 // Der Typname enthält nur ASCII-Zeichen in Kleinbuchstaben. Solche Namen können möglicherweise für die Sprache reserviert werden.
    public static partial class xy
#pragma warning restore CS8981 // Der Typname enthält nur ASCII-Zeichen in Kleinbuchstaben. Solche Namen können möglicherweise für die Sprache reserviert werden.
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
            try 
            {
                return method();
            }
            catch (Exception ex) 
            {
                xyLog.ExLog(ex); return null!;
            }
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

        #region QOL – String Utilities


        /// <summary>
        /// Repeating a given char for the specified amount
        /// </summary>
        /// <remarks>
        /// Use this overload for better performance!
        /// </remarks>
        /// <param name="text">The target character</param>
        /// <param name="count">The end amount</param>
        /// <returns>The new string</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Repeat(char text, int count) => new (text, count);


        /// <summary>
        /// Prints the given message to the console using Console.Writeline()
        /// </summary>
        /// <param name="message">The message to print.</param>
        public static void Print(this string message) => Console.WriteLine(message);

        /// <summary>
        /// Prints the given message to the console using xyLog.Log()
        /// </summary>
        /// <param name="message"></param>
        public static void Log(this string message) => xyLog.Log(message);


        #endregion

        #region System Utilities & Experiments

        public static async Task Start(string processName)
        {
            await TryCatch(async () =>
            {
                if (Process.Start(processName) is Process proc)
                {
                    return await Task.FromResult<object>(proc);
                }
                return Task.CompletedTask;
            });
        }




        /// <summary>
        /// Asynchronously opens a file or folder in the system’s default file explorer.
        /// Cross‑platform (Windows, Linux, macOS) and fully logged.
        /// </summary>
        /// <param name="fullPath">Absolute path to the file or directory.</param>
        /// <returns>True if opened successfully; otherwise, false.</returns>
        public static async Task<bool> Open(string fullPath)
        {
            string invalidPathMsg = "The given path was null or empty.";
            string notFoundMsg = "Target does not exist:";
            string successTemplate = "Explorer opened in {0} ms: {1}";
            string unsupportedOsMsg = "No suitable explorer command found for this OS.";

            if (string.IsNullOrWhiteSpace(fullPath))
            {
                await xyLog.AsxLog(invalidPathMsg);
                return false;
            }
            else
            {
                if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
                {
                    await xyLog.AsxLog($"{notFoundMsg} {fullPath}");
                    return false;
                }
            }

            Stopwatch stopwatch = new ();

            // Switch Expressions stiften doch Freude
            (string? cmd, string args) = OperatingSystem.IsWindows() switch
            {
                true => ("explorer", $"\"{fullPath}\""),
                _ when OperatingSystem.IsLinux() => ("xdg-open", fullPath),
                _ when OperatingSystem.IsMacOS() => ("open", fullPath),
                _ => (null, "Hier könnte ihre Werbung stehen!")
            };

            if (cmd is null)
            {
                await xyLog.AsxLog(unsupportedOsMsg);
                return false;
            }

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = cmd,
                    Arguments = args,
                    UseShellExecute = true,
                    CreateNoWindow = true
                };

                stopwatch.Start();
                Process.Start(psi);
                stopwatch.Stop();

                await xyLog.AsxLog(string.Format(successTemplate, stopwatch.ElapsedMilliseconds, fullPath));
                return true;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// Opens Notepad without any file.
        /// </summary>
        public static async Task Editor()
        {
            await Start("notepad.exe");
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Piep()
        {
            Console.Beep();
            xyLog.Log("Beep!");
        }

        /// <summary>
        /// A recursive method that tests edge-case control flow with arbitrary branching using goto.
        /// </summary>
        /// <param name="high_number">A large number used to trigger different flow paths.</param>
        /// <returns>Diagnostic message string (very important!!!!).</returns>
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
            if (b > high_number / 2)
            {
                Piep();
                Console.WriteLine(a);
            }
            goto loop2;

        loop2:
            Console.WriteLine("Ho");
            if (a > high_number / 2)
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
