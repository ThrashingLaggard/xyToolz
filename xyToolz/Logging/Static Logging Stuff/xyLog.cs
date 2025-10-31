using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace xyToolz.Helper.Logging
{

    /// <summary>
    /// Provides centralized logging functionality for the application.
    /// Supports both synchronous and asynchronous logging for messages and exceptions.
    /// Formats output with metadata such as timestamps, caller name, and log level.
    ///
    /// <para><b>Available Features:</b></para>
    /// <list type="bullet">
    /// <item><description>Synchronous logging of messages and exceptions</description></item>
    /// <item><description>Asynchronous logging variants</description></item>
    /// <item><description>Event-based log handling</description></item>
    /// <item><description>Formatted exception output (text + JSON)</description></item>
    /// <item><description>Caller name tracking via CallerMemberName</description></item>
    /// </list>
    ///
    /// <para><b>Thread Safety:</b></para>
    /// <para>Thread-safe log file access using locking mechanism</para>
    ///
    /// <para><b>Limitations:</b></para>
    /// <para>No filtering, buffering, or custom log routing implemented</para>
    ///
    /// <para><b>Performance:</b></para>
    /// <para>Suitable for low-to-medium frequency logging, may block on sync operations</para>
    ///
    /// <para><b>Configuration:</b></para>
    /// <para>Log file paths and sizes are hardcoded</para>
    ///
    /// <para><b>Method Overview:</b></para>
    /// <list type="table">
    /// <item><term>Log</term><description>Log plain message synchronously</description></item>
    /// <item><term>AsxLog</term><description>Log plain message asynchronously</description></item>
    /// <item><term>ExLog</term><description>Log exception details synchronously</description></item>
    /// <item><term>AsxExLog</term><description>Log exception details asynchronously</description></item>
    /// <item><term>JsonExLog</term><description>Log exception as formatted JSON (sync)</description></item>
    /// <item><term>JsonAsxExLog</term><description>Log exception as formatted JSON (async)</description></item>
    /// </list>
    ///
    /// <para><b>Example Usage:</b></para>
    /// <code>
    /// try
    /// {
    ///     throw new InvalidOperationException("Test error");
    /// }
    /// catch (Exception ex)
    /// {
    ///     xyLog.ExLog(ex);
    /// }
    /// </code>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Benennungsstile", Justification = "<Because i said so!>")]
    public static class xyLog
    {
        /// <summary>
        /// Event triggered when a standard log message is sent.
        /// </summary>
        public static event Action<string, string>? LogMessageSent;

        /// <summary>
        /// Event triggered when an exception log message is sent.
        /// </summary>
        public static event Action<string, string>? ExLogMessageSent;

        /// <summary>
        /// Helper method to manually trigger the LogMessageSent event.
        /// </summary>
        public static void OnMessageSent(string message)
        {
            LogMessageSent?.Invoke(message, "msg sent via evt");
        }

        /// <summary>
        /// Helper method to manually trigger the ExLogMessageSent event.
        /// </summary>
        public static void OnExMessageSent(string message)
        {
            ExLogMessageSent?.Invoke(message, "exmsg sent via evt");
        }

        // Default configuration and internal state
        private static readonly string _logFilePath = "logs/app.log";
        private static readonly string _exLogFilePath = "logs/exceptions.log";
        private static readonly long _maxLogFileSize = 10485760;
        private static readonly object _threadSafetyLock = new ();
        private readonly static xyLogArchiver _archiver = new(_maxLogFileSize);
        // private static IEnumerable<xyLogTargets> _eTargets = new List<xyLogTargets>();

        /// <summary>
        /// Converts raw integer targets into the xyLogTargets enum.
        /// </summary>
        /// <param name="logTargets">List of target identifiers as UInt16</param>
        /// <returns>Enumerable of xyLogTargets</returns>
        internal static IEnumerable<xyLogTargets> SetLogTargets(ushort[] logTargets)
        {
            if (logTargets.Length < 1) return [] ;
            xyLogTargets[] targets = new xyLogTargets[logTargets.Length];

            for (int i = 0; i < logTargets.Length; i++)
            {
                targets[i] = (xyLogTargets)logTargets[i];
            }
            return targets;
        }

        #region Logging

        /// <summary>
        /// Centralized method for printing log messages to console and firing events.
        /// </summary>
        /// <param name="formattedMessage">Already formatted log string</param>
        /// <param name="callerName">Optional caller name</param>
        private static void Output(string formattedMessage, string? callerName)
        {
            Console.WriteLine(formattedMessage);
            Console.Out.Flush();
            LogMessageSent?.Invoke(formattedMessage, callerName!);
        }

        /// <summary>
        /// Centralized method for printing log messages to console and firing events.
        /// </summary>
        /// <param name="formattedMessage">Already formatted log string</param>
        /// <param name="callerName">Optional caller name</param>
        private static async Task OutputAsync(string formattedMessage, string? callerName)
        {
            await Console.Out.WriteLineAsync(formattedMessage);
            Console.Out.Flush();
            LogMessageSent?.Invoke(formattedMessage, callerName!);
        }

        /// <summary>
        /// Logs a simple text message synchronously.
        /// </summary>
        public static string Log(string message, [CallerMemberName] string? callerName = null)
        {
            string formattedMsg = FormatMsg(message, callerName, LogLevel.Debug);
            Output(formattedMsg, callerName);
            return formattedMsg;
        }

        /// <summary>
        /// Logs a simple text message asynchronously.
        /// </summary>
        public static async Task<string> AsxLog(string message, [CallerMemberName] string? callerName = null)
        {
            string formattedMsg = await Task.Run(() => FormatMsg(message, callerName, LogLevel.Debug));
            if (formattedMsg.Length == 0)
            {
                return "What in the fucking hell happened here?";
            }
            await OutputAsync(formattedMsg, callerName);
                
            return formattedMsg;
        }

        /// <summary>
        /// Logs details of an exception synchronously.
        /// </summary>
        public static void ExLog(Exception ex, LogLevel level = LogLevel.Error, string? message =  null, [CallerMemberName] string? callerName = null)
        {
            string exMessage = FormatEx(ex, level,message, callerName);
            Output(exMessage, callerName);
        }

        /// <summary>
        /// Logs details of an exception asynchronously.
        /// </summary>
        public static async Task AsxExLog(Exception ex, LogLevel level = LogLevel.Error, [CallerMemberName] string? callerName = null)
        {
            string exMessage =  await Task.Run(() =>FormatEx(ex, level, callerName));
            
            await OutputAsync(exMessage, callerName);
        }

        /// <summary>
        /// Logs an exception as JSON formatted string (synchronous).
        /// </summary>
        public static void JsonExLog(Exception ex, [CallerMemberName] string? callerName = null)
        {
            string json = xyLogFormatter.FormatExceptionAsJson(ex);
            Output(json, callerName);
        }

        /// <summary>
        /// Logs an exception as JSON formatted string (asynchronous).
        /// </summary>
        public static async Task JsonAsxExLog(Exception ex)
        {
            string json = xyLogFormatter.FormatExceptionAsJson(ex);
            await Console.Out.WriteLineAsync(json);
        }
        #endregion
        /// <summary>
        /// Synchronous: Writes a log message into file and console
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public static bool WriteLog(string message, [CallerMemberName] string? callerName = null)
        {
            lock (_threadSafetyLock)
            {
                CheckFileSizeAndMoveLogsToArchiveWhenTooBig();
                try
                {
                    string formattedMessage = FormatMsg(message, callerName);
                    File.AppendAllText(_logFilePath, formattedMessage);
                    Output(formattedMessage, callerName);
                    return true;
                }
                catch (Exception ex)
                {
                    Log("BaseLogger.WriteLog() failed...!", callerName);
                    WriteExLog(ex, LogLevel.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// Synchronous: Writes details for the given exception to into console and file
        /// </summary>
        public static bool WriteExLog(Exception ex, LogLevel level = LogLevel.Error, [CallerMemberName] string? callerName = null)
        {
            lock (_threadSafetyLock)
            {
                try
                {
                    CheckFileSizeAndMoveLogsToArchiveWhenTooBig();

                    string exceptionDetails = FormatEx(ex, level, callerName);
                    File.AppendAllText(_exLogFilePath, exceptionDetails);
                    Output(exceptionDetails, callerName);
                    return true;
                }
                catch (Exception innerEx)
                {
                    ExLog(innerEx, LogLevel.Warning, callerName);
                }
                return false;
            }
        }

        /// <summary>
        /// Synchronous: Writes a json message into file and console
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public static bool WriteJson(string message, [CallerMemberName] string? callerName = null)
        {
            lock (_threadSafetyLock)
            {
                CheckFileSizeAndMoveLogsToArchiveWhenTooBig();
                try
                {
                    string formattedMessage = FormatMsg(message, callerName);
                    File.AppendAllText(_logFilePath, formattedMessage);
                    Output(formattedMessage, callerName);
                    return true;
                }
                catch (Exception ex)
                {
                    Log("BaseLogger.WriteLog() failed...!", callerName);
                    WriteExLog(ex, LogLevel.Error);
                    return false;
                }
            }
        }

        #region Service

        /// <summary>
        /// Execute a message and protocol the result
        /// 
        /// logger.ExecuteWithLogging(">Screaming()", () =>Screaming(testText),Exception => logger.ExLog(Exception,LogLevel.Warning,">Screaming()"));
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionName"></param>
        /// <param name="action"></param>
        /// <param name="logOnError"></param>
        /// <returns></returns>
        public static T ExecuteDebugging<T>(string actionName, Func<T> action, Action<Exception>? logOnError = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionName))
                {
                    ExLog(new ArgumentException("ActionName invalid"), LogLevel.Error);
                }
                string go = $"Start: {actionName}";
                string yes = $"Erfolgreich: {actionName} abgeschlossen.";
                string no = $"Warnung: {actionName} hat keine Ergebnisse geliefert.";

                Log(go);
                var result = action();

                if (result == null || result is IEnumerable<object> enumerable && !enumerable.Any())
                {
                    Log(no);
                }
                else
                {
                    Log(yes);
                }
                return result;
            }
            catch (Exception ex)
            {
                logOnError?.Invoke(ex);// Den im Aufruf per Lambda-Ausdruck generierten Delegaten (in diesem Fall sogar mit Parameter) ansprechen, falls dieser != null 
                ExLog(ex, LogLevel.Critical, actionName);  // Wenn die Exception nicht anders definiert ist, als die des Delegaten, kommt ggf zweimal die gleiche Nachricht, mit unterschiedlichen Callern
            }

            return default!;
        }





        /// <summary>
        /// Execute "a function" with one main return value with logging BUT ASYNC
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionName"></param>
        /// <param name="action"></param>
        /// <param name="logOnError"></param>
        /// <returns></returns>
        public static async Task<T> ExecuteDebuggingAsync<T>(string actionName, Func<T> action, Action<Exception>? logOnError = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionName))
                {
                    await AsxExLog(new ArgumentException("ActionName invalid"), LogLevel.Error);
                }
                string go = $"Start: {actionName}";
                string yes = $"Erfolgreich: {actionName} abgeschlossen.";
                string no = $"Warnung: {actionName} hat keine Ergebnisse geliefert.";

                await AsxLog(go);
                var result = action();

                if (result == null || result is IEnumerable<object> enumerable && !enumerable.Any())
                {
                    await AsxLog(no);
                }
                else
                {
                    await AsxLog(yes);
                }
                return result;
            }
            catch (Exception ex)
            {
                logOnError?.Invoke(ex);// Den im Aufruf per Lambda-Ausdruck generierten Delegaten (in diesem Fall sogar mit Parameter) ansprechen, falls dieser != null 
                await AsxExLog(ex, LogLevel.Critical, actionName);  // Wenn die Exception nicht anders definiert ist, als die des Delegaten, kommt ggf zweimal die gleiche Nachricht, mit unterschiedlichen Callern
            }

            return default!;
        }







        /// <summary>
        /// Checks the filesize for both logtypes and archives the logs if they get "too big"
        /// </summary>
        private static void CheckFileSizeAndMoveLogsToArchiveWhenTooBig()
        {
            try
            {
                // Standard-Logs
                _archiver.MoveLogToArchiveFileIfTooBig(_logFilePath);

                // ExLogs
                _archiver.MoveLogToArchiveFileIfTooBig(_exLogFilePath);
            }
            catch (Exception ArchivingLogsEx)
            {
                WriteExLog(ArchivingLogsEx);
            }
        }

        /// <summary>
        /// Format the given message for a detailed overview
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callerName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string FormatMsg(string message, string? callerName = null, LogLevel? level = null)
        {
            return xyLogFormatter.FormatMessageForLogging(message, callerName, level);
        }
        /// <summary>
        /// Formats the Exception´s details for consistent logging.
        /// </summary>
        private static string FormatEx(Exception ex, LogLevel level, string? message = null, string? callerName = null)
        {
            return xyLogFormatter.FormatExceptionDetails(ex, level, message,callerName);
        }

        #endregion

        ///// <summary>
        ///// Reads the configuration from the json file or sets default values if that doesnt work
        ///// </summary>
        ///// <returns></returns>
        //public static async Task<DefaultLoggerConfig> LoadLoggerConfigurationFromJson()
        //{
        //      return await Task.Run(() => _configurator.LoadLoggerConfigurationFromJson());
        //}
    }
}
