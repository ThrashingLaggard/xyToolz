using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace xyToolz.Helper.Logging
{
      /// <summary>
      /// Format text for logging:
      /// -Exceptions
      /// -Messages
      /// -Mails
      /// -Performance
      /// </summary>
    public static class xyLogFormatter
    {
            /// <summary>
            /// Get a detailed view for the caught exceptions
            /// </summary>
            /// <param name="ex"></param>
            /// <param name="level"></param>
            /// <param name="callerName"></param>
            /// <returns></returns>
            public static string FormatExceptionDetails(Exception ex, LogLevel level, string? callerName = null)
            {
                  StringBuilder sb = new StringBuilder();
                  sb.AppendLine($"{DateTime.Now} [{callerName ?? ""}] [{level}] Exception Details:");

                  // Allgemeine Exception-Daten
                  sb.AppendLine($"Type: {ex.GetType().Name}");
                  sb.AppendLine($"Message: {ex.Message}");
                  sb.AppendLine($"TargetSite: {ex.TargetSite}");
                  sb.AppendLine($"Source: {ex.Source}");
                  sb.AppendLine($"StackTrace: {ex.StackTrace}");

                  // Zusätzliche Daten (falls vorhanden)
                  if (ex.Data != null && ex.Data.Count > 0)
                  {
                        sb.AppendLine("Custom Data:");
                        foreach (var key in ex.Data.Keys)
                        {
                              sb.AppendLine($"  {key}: {ex.Data[key]}");
                        }
                  }

                  // InnerException rekursiv formatieren
                  if (ex.InnerException != null)
                  {
                        sb.AppendLine("Inner Exception Details:");
                        sb.AppendLine(FormatExceptionDetails(ex.InnerException, level));
                  }

                  return sb.ToString();
            }

            /// <summary>
            /// Format a normal string for a detailed output
            /// </summary>
            /// <param name="message"></param>
            /// <param name="callerName"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public static string FormatMessageForLogging(string message, string? callerName = null, LogLevel? level = null)
            {
                  {
                        // Setze das aktuelle Datum und die Uhrzeit
                        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                        // Log-Level standardmäßig auf "Information" setzen, falls keiner angegeben ist
                        string logLevel = level?.ToString() ?? "Information";

                        // Aufrufernamen standardisieren
                        callerName = string.IsNullOrEmpty(callerName) ? "UnknownCaller" : callerName;

                        // Nachricht formatieren
                        string formattedMessage = $"[{timestamp}] [{logLevel}] [{callerName}] {message}";

                        return formattedMessage;
                  }
            }

            /// <summary>
            /// Format captured mails
            /// </summary>
            /// <param name="mailMessage"></param>
            /// <returns></returns>
            public static string FormatMailDetails(MailMessage mailMessage)
            {
                  var logDetails = new StringBuilder();
                  logDetails.AppendLine("Timestamp:  " + DateTime.Now);
                  logDetails.AppendLine("From: " + mailMessage.From?.Address);
                  logDetails.AppendLine("Sender: " + mailMessage.Sender);
                  logDetails.AppendLine("To: " + string.Join(", ", mailMessage.To.Select(m => m.Address)));
                  logDetails.AppendLine("Subject: " + mailMessage.Subject);
                  logDetails.AppendLine("AttachmentsCount" + mailMessage.Attachments.Count);
                  logDetails.AppendLine("Header: " + mailMessage.Headers);
                  logDetails.AppendLine("Body: " + mailMessage.Body);

                  return logDetails.ToString();
            }

            /// <summary>
            /// Format the performance log
            /// </summary>
            /// <param name="operationName"></param>
            /// <param name="duration"></param>
            /// <returns></returns>
            public static string FormatPerformanceLog(string operationName, TimeSpan duration)
            {
                  var logDetails = new StringBuilder();
                  logDetails.AppendLine("Timestamp:  " + DateTime.Now);
                  logDetails.AppendLine("Operation: " + operationName);
                  logDetails.AppendLine("Duration: " + duration.TotalMilliseconds + " ms");

                  return logDetails.ToString();

            }
      }
}
