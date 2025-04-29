using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyPorts.Basix;
using xyPorts.TCP;
using xyPorts.UDP;
using xyToolz;

namespace xyPorts.Master
{
    /// <summary>
    /// Provides basic information and operations for ports and some additional operations via the xyTcpPort and xyUdpPort classes
    /// </summary>
    public class xyPortManager
    {
        /// <summary>
        /// Provides basic information about and some operations for ports 
        /// </summary>
        public xyPort xyPort { get; set; }

        /// <summary>
        /// provides basic information about and some operations for TCP ports
        /// </summary>
        internal xyTcpPort xyTcpPort { get; set; }

        /// <summary>
        /// provides basic information about and some operations for UDP ports
        /// </summary>
        internal xyUdpPort xyUdpPort { get; set; }

        /// <summary>
        /// Close the target port by killing the process using it
        /// </summary>
        /// <param name="port"></param>
        public static void ClosePort(int port)
        {
            // Führen Sie den netstat-Befehl aus und lesen Sie die Ausgabe
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "netstat",
                    Arguments = "-ano",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            using (var reader = new StreamReader(process.StandardOutput.BaseStream))
            {
                string output = reader.ReadToEnd();
                xyLog.Log(output);
                process.WaitForExit();

                // Suchen Sie nach der Zeile, die den Port enthält
                var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                var portLine = lines.FirstOrDefault(line => line.Contains($":{port}"));

                if (portLine != null)
                {
                    // Extrahieren Sie die PID aus der Zeile
                    var parts = portLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int pid = int.Parse(parts.Last()); // PID ist das letzte Element in der Zeile

                    Console.WriteLine($"Port {port} wird von Prozess {pid} verwendet.");

                    try
                    {
                        Process proc = Process.GetProcessById(pid);
                        proc.Kill(); // Prozess beenden
                        Console.WriteLine($"Prozess {pid} wurde beendet.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Fehler beim Beenden des Prozesses: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Port {port} ist nicht in Verwendung.");
                }
            }
        }
    }
}
