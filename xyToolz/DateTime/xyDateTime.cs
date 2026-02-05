using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.TimeDate
{
    public class xyDateTime
    {
        /// <summary>
        /// Get the hash code for 'DateTime.Now()'
        /// </summary>
        /// <returns>int hashCode</returns>
        public static int GetHashFromNow() => DateTime.Now.GetHashCode();
    }
}
