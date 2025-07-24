using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Logging.Models
{
    public class xyLogEntry
    {
        /// <summary>
        /// For easy administration
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        /// Add interesting information
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Additional information
        /// </summary>
        public string Comment { get; set; } = default!;

        /// <summary>
        /// Time of logging
        /// </summary>
        public required DateTime Timestamp { get; init; } 

        /// <summary>
        /// Where it was logged --> callername
        /// </summary>
        public required string Source { get; init; }

        /// <summary>
        /// The level of "severity"
        /// </summary>
        public LogLevel Level { get; set; } = LogLevel.Debug;

        /// <summary>
        /// The logging message
        /// </summary>
        public required string Message { get; init; }

        /// <summary>
        /// The exception connected to the log
        /// </summary>
        public Exception Exception { get; set; }



        public xyLogEntry(string source_, LogLevel level_, string message_, Exception? exception_ = null)
        {
            Timestamp = DateTime.Now;
            Source = source_;
            Level = level_;
            Message = message_;
            Exception = exception_ ?? default!;
        }

    }
}
