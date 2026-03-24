using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Enumerables
{


    /// <summary>
    /// Providing very efficient helper methods to reduce the needed dictionary calls
    /// </summary>
    public static class xyDictionaryExtensions
    {

        // The problem:
        // For the usual reading or writing operations, the dictionary is called two times
        //public static TValue GetOrAddTwoCalls<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : notnull
        //{
        //    if (dictionary.TryGetValue(key,out var result))                                                                                   // First call to read the value for the key
        //    {
        //        return result;
        //    }
        //    else
        //    {
        //        dictionary[key] = value;                                                                                                                  // Another call to write a value to the key
        //        return value;
        //    }
        //}

        /// <summary>
        /// Very efficient extention method
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue? GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue? value) where TKey : notnull
        {
            ref TValue? val = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary,key,out bool exists);   // First call to get the reference of the key
            if (exists)
            {
                return val;
            }

            val = value;                                                                                                                                            // Only using the key's reference, not needing to call the rest of dictionary!!!!
            return value;
        }

        /// <summary>
        /// Very efficient extention method for updating
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue? value) where TKey : notnull
        {
            ref TValue? val = ref CollectionsMarshal.GetValueRefOrNullRef(dictionary,key);                              // First call to get the reference of the key
            if (Unsafe.IsNullRef(ref val))
            {
                return false;
            }

            val = value;                                                                                                                                            // Only using the key's reference, not needing to call the rest of dictionary!!!!
            return true;
        }
    }
}
