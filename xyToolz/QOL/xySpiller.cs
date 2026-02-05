using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.QOL
{
    public class xySpiller
    {
        
        public static string Spill<T>(T[] targetValues, bool hasWhitespace) => string.Join(hasWhitespace ? ", " : ",", targetValues ?? []);
        public static string Spill<T>(IList<T> targetValues, bool hasWhitespace) => string.Join(hasWhitespace ? ", " : ",", targetValues ?? []);
        public static string Spill<T>(IEnumerable<T> targetValues, bool hasWhitespace) => string.Join(hasWhitespace ? ", " : ",", targetValues ?? []);
        public static string Spill<TKey, TValue>(Dictionary<TKey, TValue> targetValues, bool hasWhitespace) => string.Join(hasWhitespace ? ", " : ",", targetValues.Select(kvp => $"{kvp.Key}:{kvp.Value}"));
        
    }
}
