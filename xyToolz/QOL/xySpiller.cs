using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.QOL
{
    /// <summary>
    /// Output all sorts of collections
    /// </summary>
    public class xySpiller
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Spill<T>(IEnumerable<T> targetValues, bool hasWhitespace = true, bool? hasSeperator = true, string? seperator = ",") 
            => Join(targetValues, hasWhitespace, hasSeperator, seperator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Spill<T>(T[] targetValues, bool hasWhitespace = true, bool? hasSeperator = true, string? seperator = ",") 
            => Join(targetValues,hasWhitespace, hasSeperator, seperator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Spill<T>(IList<T> targetValues, bool? hasWhitespace = true, bool? hasSeperator = true, string? seperator = ",")
            => Join(targetValues,hasWhitespace, hasSeperator, seperator );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Spill<TKey, TValue>(Dictionary<TKey, TValue> targetValues, bool hasWhitespace = true, bool? hasSeperator = true, string? seperator = ",") 
            => Join( targetValues.Select(kvp => $"{kvp.Key}:{kvp.Value}"), hasWhitespace, hasSeperator, seperator);



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string Join<T>(IEnumerable<T> values, bool? hasWhitespace = true, bool? hasSeperator = true, string? seperator = ",")
            => string.Join((((bool)hasSeperator)? seperator: string.Empty) + (((bool)hasWhitespace) ? " " : string.Empty), values);



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string JoinDebug<T>(IEnumerable<T> values, bool? hasWhitespace = true, bool? hasSeperator = true, string? seperator = ",")
        {
            string empty = string.Empty;
            string whiteSpace = " ";
            
            if (hasSeperator is false)
            {
                seperator = empty;
            }

            if (hasWhitespace is true)
            {
                seperator += whiteSpace;
            }

            string result = string.Join(seperator, values);

            return result;  
        }
    }
}
