using System;

namespace xyToolz.Chrono
{
    /// <summary>
    /// Helper class for date operations
    /// </summary>
    public class xyDate
    {
        /// <summary>
        /// Get today's date
        /// </summary>
        /// <returns>DateTime currentDate</returns>
        public static DateTimeOffset Today() => DateTimeOffset.Now.Date;
        /// <summary>
        /// Get tomorrow's date
        /// </summary>
        /// <returns>DateTime nextDate</returns>
        public static DateTimeOffset Tomorrow() => DateTimeOffset.Now.Date +  TimeSpan.FromDays(1);
        
        /// <summary>
        /// Get yesterday's date
        /// </summary>
        /// <returns>DateTime lastDate</returns>
        public static DateTimeOffset Yesterday() => DateTimeOffset.Now.Date -  TimeSpan.FromDays(1);
    }
}
