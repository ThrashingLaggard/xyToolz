using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.QOL
{
    /// <summary>
    /// Provides static helper methods for converting collections into formatted, delimiter-separated strings.
    /// </summary>
    /// <remarks>
    /// All public overloads are thin wrappers around an internal <c>Join</c> method and are marked
    /// <see cref="MethodImplOptions.AggressiveInlining"/> to eliminate call overhead in hot paths.
    /// A step-by-step debug variant (<c>JoinDebug</c>) is available for diagnostic purposes.
    /// </remarks>
    public class xySpiller
    {
        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> to a formatted, delimiter-separated string.
        /// </summary>
        /// <typeparam name="T">The element type of the sequence.</typeparam>
        /// <param name="targetValues">The sequence of values to format.</param>
        /// <param name="hasWhitespace">
        /// <see langword="true"/> to append a space after each delimiter; otherwise <see langword="false"/>.
        /// </param>
        /// <param name="hasSeperator">
        /// <see langword="true"/> to insert <paramref name="seperator"/> between elements;
        /// <see langword="false"/> to join elements with no delimiter.
        /// </param>
        /// <param name="seperator">
        /// The delimiter string inserted between elements when <paramref name="hasSeperator"/> is
        /// <see langword="true"/>. Defaults to <c>","</c>.
        /// </param>
        /// <returns>A single string containing all elements separated by the configured delimiter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Spill<T>(IEnumerable<T> targetValues, bool hasWhitespace = true, bool? hasSeperator = true, string? seperator = ",")
            => Join(targetValues, hasWhitespace, hasSeperator, seperator);

        /// <summary>
        /// Converts an array to a formatted, delimiter-separated string.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="targetValues">The array of values to format.</param>
        /// <param name="hasWhitespace">
        /// <see langword="true"/> to append a space after each delimiter; otherwise <see langword="false"/>.
        /// </param>
        /// <param name="hasSeperator">
        /// <see langword="true"/> to insert <paramref name="seperator"/> between elements;
        /// <see langword="false"/> to join elements with no delimiter.
        /// </param>
        /// <param name="seperator">
        /// The delimiter string inserted between elements when <paramref name="hasSeperator"/> is
        /// <see langword="true"/>. Defaults to <c>","</c>.
        /// </param>
        /// <returns>A single string containing all elements separated by the configured delimiter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Spill<T>(T[] targetValues, bool hasWhitespace = true, bool? hasSeperator = true, string? seperator = ",")
            => Join(targetValues, hasWhitespace, hasSeperator, seperator);

        /// <summary>
        /// Converts an <see cref="IList{T}"/> to a formatted, delimiter-separated string.
        /// </summary>
        /// <typeparam name="T">The element type of the list.</typeparam>
        /// <param name="targetValues">The list of values to format.</param>
        /// <param name="hasWhitespace">
        /// <see langword="true"/> to append a space after each delimiter; otherwise <see langword="false"/>.
        /// </param>
        /// <param name="hasSeperator">
        /// <see langword="true"/> to insert <paramref name="seperator"/> between elements;
        /// <see langword="false"/> to join elements with no delimiter.
        /// </param>
        /// <param name="seperator">
        /// The delimiter string inserted between elements when <paramref name="hasSeperator"/> is
        /// <see langword="true"/>. Defaults to <c>","</c>.
        /// </param>
        /// <returns>A single string containing all elements separated by the configured delimiter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Spill<T>(IList<T> targetValues, bool? hasWhitespace = true, bool? hasSeperator = true, string? seperator = ",")
            => Join(targetValues, hasWhitespace, hasSeperator, seperator);

        /// <summary>
        /// Converts a <see cref="Dictionary{TKey, TValue}"/> to a formatted string where each entry
        /// is rendered as <c>key:value</c> and entries are separated by the configured delimiter.
        /// </summary>
        /// <typeparam name="TKey">The type of the dictionary keys.</typeparam>
        /// <typeparam name="TValue">The type of the dictionary values.</typeparam>
        /// <param name="targetValues">The dictionary whose entries are to be formatted.</param>
        /// <param name="hasWhitespace">
        /// <see langword="true"/> to append a space after each delimiter; otherwise <see langword="false"/>.
        /// </param>
        /// <param name="hasSeperator">
        /// <see langword="true"/> to insert <paramref name="seperator"/> between entries;
        /// <see langword="false"/> to join entries with no delimiter.
        /// </param>
        /// <param name="seperator">
        /// The delimiter string inserted between entries when <paramref name="hasSeperator"/> is
        /// <see langword="true"/>. Defaults to <c>","</c>.
        /// </param>
        /// <returns>
        /// A single string of all key-value pairs in <c>key:value</c> format,
        /// separated by the configured delimiter.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Spill<TKey, TValue>(Dictionary<TKey, TValue> targetValues, bool hasWhitespace = true, bool? hasSeperator = true, string? seperator = ",")
            => Join(targetValues.Select(kvp => $"{kvp.Key}:{kvp.Value}"), hasWhitespace, hasSeperator, seperator);

        /// <summary>
        /// Joins the elements of a sequence into a single string using the specified delimiter and spacing options.
        /// </summary>
        /// <typeparam name="T">The element type of the sequence.</typeparam>
        /// <param name="values">The sequence of values to join.</param>
        /// <param name="hasWhitespace">
        /// <see langword="true"/> to append a space after the delimiter; otherwise <see langword="false"/>.
        /// </param>
        /// <param name="hasSeperator">
        /// <see langword="true"/> to use <paramref name="seperator"/> as the delimiter;
        /// <see langword="false"/> to use an empty string.
        /// </param>
        /// <param name="seperator">
        /// The delimiter string used between elements when <paramref name="hasSeperator"/> is
        /// <see langword="true"/>. Defaults to <c>","</c>.
        /// </param>
        /// <returns>A single string of all elements joined by the resolved delimiter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string Join<T>(IEnumerable<T> values, bool? hasWhitespace = true, bool? hasSeperator = true, string? seperator = ",")
            => string.Join((((bool)hasSeperator) ? seperator : string.Empty) + (((bool)hasWhitespace) ? " " : string.Empty), values);

        /// <summary>
        /// Joins the elements of a sequence into a single string using the specified delimiter and spacing options.
        /// Intended for diagnostic use; implements the same logic as <see cref="Join{T}"/> with
        /// explicit intermediate steps to aid debugging.
        /// </summary>
        /// <typeparam name="T">The element type of the sequence.</typeparam>
        /// <param name="values">The sequence of values to join.</param>
        /// <param name="hasWhitespace">
        /// <see langword="true"/> to append a trailing space to the delimiter; otherwise <see langword="false"/>.
        /// </param>
        /// <param name="hasSeperator">
        /// <see langword="true"/> to use <paramref name="seperator"/> as the delimiter;
        /// <see langword="false"/> to replace it with an empty string.
        /// </param>
        /// <param name="seperator">
        /// The delimiter string used between elements. Defaults to <c>","</c>.
        /// Pass <see langword="null"/> to fall back to the default.
        /// </param>
        /// <returns>A single string of all elements joined by the resolved delimiter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string JoinDebug<T>(IEnumerable<T> values, bool? hasWhitespace = true, bool? hasSeperator = true, string? seperator = ",")
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