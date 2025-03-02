using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace xyToolz.Helper
{
    public static class xyLogFormatter
    {
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

            public static string LogPerformance(string operationName, TimeSpan duration)
            {
                  var logDetails = new StringBuilder();
                  logDetails.AppendLine("Timestamp:  " + DateTime.Now);
                  logDetails.AppendLine("Operation: " + operationName);
                  logDetails.AppendLine("Duration: " + duration.TotalMilliseconds + " ms");

                  return logDetails.ToString();

            }
      }
}
