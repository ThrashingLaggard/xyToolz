using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Chrono
{
    /// <summary>
    /// Helper Methods for time operations
    /// </summary>
    public static class xyTime
    {
        /// <summary>
        /// Get the hash code for 'DateTimeOffset.Now()'
        /// </summary>
        /// <returns>int hashCode</returns>
         [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int HashNow() => DateTimeOffset.Now.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTimeOffset Now() => DateTimeOffset.Now;

        /// <summary>
        /// Returns the UTC DateTime component of the whole 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime GetDateTime(this DateTimeOffset offset) => offset.UtcDateTime;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan GetOffset(this DateTimeOffset offset) => offset.Offset;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan GetTimeOfDay(this DateTimeOffset offset) => offset.TimeOfDay;
    }
}
