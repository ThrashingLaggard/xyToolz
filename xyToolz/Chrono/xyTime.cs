using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Chrono
{
    /// <summary>
    /// Helper Methods for time operations
    /// </summary>
    public class xyTime
    {
        /// <summary>
        /// Get the hash code for 'DateTime.Now()'
        /// </summary>
        /// <returns>int hashCode</returns>
        public static int HashNow() => DateTimeOffset.Now.GetHashCode();

        public static DateTimeOffset Now() => DateTimeOffset.Now;

    }
}
