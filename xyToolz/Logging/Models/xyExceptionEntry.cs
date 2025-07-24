using System.Collections;
using System.Reflection;

namespace xyToolz.Logging.Models
{
    public class xyExceptionEntry
    {
        /// <summary>
        /// For easy administration
        /// </summary>
        public uint ID { get; set; }
        
        /// <summary>
        /// The target exception
        /// </summary>
        public required Exception Exception { get; init; }

        /// <summary>
        /// The inner exception
        /// </summary>
        public Exception InnerException { get; init; }

        /// <summary>
        /// What kind of exception does this?
        /// </summary>
        public required Type TypeOfException { get; init; }

        /// <summary>
        /// Time of occurance
        /// </summary>
        public required DateTime Timestamp { get; init; }

        /// <summary>
        /// The root of the problem
        /// </summary>
        public  string Source { get; init; }

        /// <summary>
        /// Show the frames on the call stack
        /// </summary>
        public string StrackTrace { get; init; }

        /// <summary>
        /// Where did it happen
        /// </summary>
        public MethodBase TargetSite { get; init; }

        /// <summary>
        /// The message embedded in the exception
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// Add interesting information
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Space for additional data
        /// </summary>
        public IDictionary CustomData { get; set; }


        public xyExceptionEntry(Exception exception_)
        {
            Exception = exception_;
            InnerException = exception_.InnerException ?? default!;
            TypeOfException = exception_.GetType();
            Timestamp = DateTime.Now;
            Source = exception_.Source ?? default!;
            StrackTrace = exception_.StackTrace ?? default!;
            TargetSite = exception_.TargetSite ?? default!;
            Message = exception_.Message;
            CustomData = exception_.Data;
        }


    }
}