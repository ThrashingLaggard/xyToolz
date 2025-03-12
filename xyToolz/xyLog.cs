using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using xyToolz.Helper.Logging;

namespace xyToolz
{
      public static class xyLog
      {
            /// <summary>
            /// Event für die DebugingConsole
            /// </summary>
            public static event Action<string>? LogMessageSent;

            public static void OnMessageSent(string message )
            {
                  LogMessageSent?.Invoke (message);
            }


            private static readonly string _logFilePath = "logs/app.log"; // Standard logs
            private static readonly string _exLogFilePath = "logs/exceptions.log"; // Exceptional logs
            private static readonly long _maxLogFileSize = 10485760; // 10 MB lol
            private static readonly Object _threadSafetyLock = new object ( );
            private static xyLogArchiver _archiver = new (_maxLogFileSize);
            private static Boolean IsLoggingToSystemConsole = true;
            private static IEnumerable<xyLogTargets> _eTargets = new List<xyLogTargets>();      // 0 - System     |||   1 - OwnDebug
            

           

            internal static IEnumerable<xyLogTargets> SetLogTargets ( UInt16 [ ] logTargets)
            {
                  if (logTargets.Length < 1) return new xyLogTargets [ ] {0}; 
                  xyLogTargets[] targets = new xyLogTargets[logTargets.Length ];

                  for (int i = 0 ; i < logTargets.Length ; i++)
                  {
                        targets [ i ] = ( xyLogTargets )logTargets [ i ] ;
                  }
                  return targets;
            }



            #region Logging
            /// <summary>
            /// Writes a log-Message into console and returns the message as string
            /// </summary>
            /// <param name="message"></param>
            /// <param name="callerName"></param>
          
            public static string Log(string message, [CallerMemberName] string? callerName = null)
        {
            string formattedMsg = FormatMsg(message, callerName, LogLevel.Debug);
            Console.WriteLine(formattedMsg);
            Console.Out.Flush();
            
            LogMessageSent?.Invoke(formattedMsg);
            return formattedMsg;
        }

            /// <summary>
            /// Writes details for the given exception to into console
            /// </summary>
            /// <param name="ex"></param>
            /// <param name="level"></param>
            /// <param name="callerName"></param>
            public static void ExLog(Exception ex, LogLevel level = LogLevel.Error, [CallerMemberName] string? callerName = null)
            {
                    string exMessage = FormatEx(ex, level, callerName);
                    Console.WriteLine(exMessage);
                    Console.Out.Flush();
                    LogMessageSent?.Invoke(exMessage);
        }

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
                              Console.WriteLine(formattedMessage);
                              Console.Out.Flush();
                                LogMessageSent?.Invoke(formattedMessage);
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
                              Console.WriteLine(exceptionDetails);
                              Console.Out.Flush();
                                LogMessageSent?.Invoke(exceptionDetails);
                                return true;
                        }
                        catch (Exception innerEx)
                        {
                              ExLog(innerEx, LogLevel.Warning, callerName);
                        }
                        return false;
                  }
            }
            #endregion

            #region Service

            /// <summary>
            /// Führt eine Aktion aus und protokolliert deren Start, Erfolg oder Fehler, einschließlich spezifischer Behandlungen von Ausnahmen.
            /// </summary>
            /// <typeparam name="T">Der Rückgabetyp der Aktion, die ausgeführt wird.      </typeparam>
            /// <param name="actionName">Der Name der Aktion, der in den Logmeldungen verwendet wird. </param>
            /// <param name="action">Eine Methode oder Funktion (als Delegate `Func<T>`), die ausgeführt werden soll.   </param>
            /// <param name="logOnError">Eine optionale Aktion (Delegate `Action<Exception>`), die bei Auftreten eines Fehlers ausgeführt wird. Kann null sein. </param>
            /// <returns>
            /// Das Ergebnis der ausgeführten Aktion vom Typ `T`. Wenn ein Fehler auftritt, wird der Standardwert von `T` zurückgegeben (`default(T)`).
            /// </returns>
            /// <example>
            /// 
            ///  logger.ExecuteWithLogging<string>(">Screaming()", () =>Screaming(testText),Exception => logger.ExLog(Exception,LogLevel.Warning,">Screaming()"));
            /// 
            /// 
            /// </example>
            public static T ExecuteWithLogging<T>(string actionName, Func<T> action, Action<Exception>? logOnError = null)
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

                        if (result == null || (result is IEnumerable<object> enumerable && !enumerable.Any()))
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
            private static string FormatEx(Exception ex, LogLevel level, string? callerName = null)
            {
                  return xyLogFormatter.FormatExceptionDetails(ex, level, callerName);
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
