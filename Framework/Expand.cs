using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace BExpansion
{
    /// <summary>
    /// A class for all expansion objects of other classes.
    /// </summary>
    public static class Expand
    {
        /// <summary>
        /// Concatenates the members of a collection, using the specified separator between each member.
        /// This is a wrapper for the String.Join function.
        /// </summary>
        /// <typeparam name="T">The type of the values of the members.</typeparam>
        /// <param name="iEnumerable">The collection.</param>
        /// <param name="separator">The separator</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> iEnumerable, string separator)
            => String.Join(separator, iEnumerable);

        /// <summary>
        /// Adds a directory separator char to the end of a path if there is not one already.
        /// </summary>
        /// <param name="path">The path to add the char to.</param>
        /// <returns>A string containing the path and a direcotry separator char at the end.</returns>
        public static string AddDirectorySeparatorChar(this string path)
        {
            if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                return path;
            return path + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Returns the string with all invalid path characters filtered.
        /// </summary>
        /// <param name="path">The path to replace the items of.</param>
        /// <param name="replacementChar">The replacement character.</param>
        /// <returns>The string with all invalid path characters filtered.</returns>
        public static string MakePathSafe(this string path, char replacementChar = '\0')
        {
            return Path.GetInvalidPathChars().Aggregate(path, (current, invalidPathChar) => current.Replace(invalidPathChar, replacementChar));
        }

        /// <summary>
        /// Returns the string with all invalid filename characters filtered.
        /// </summary>
        /// <param name="path">The path to replace the items of.</param>
        /// <param name="replacementChar">The replacement character.</param>
        /// <returns>The string with all invalid filename characters filtered.</returns>
        public static string MakeFileNameSafe(this string path, char replacementChar = '\0')
        {
            return Path.GetInvalidFileNameChars().Aggregate(path, (current, invalidPathChar) => current.Replace(invalidPathChar, replacementChar));
        }

        /// <summary>
        /// Returns the next value from the RandomNumberGenerator.
        /// </summary>
        /// <param name="r">The RandomNumberGenerator object.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The next value from the RandomNumberGenerator within the specified range.</returns>
        public static int Next(this RandomNumberGenerator r, int min, int max)
        {
            var b = new byte[1];
            r.GetBytes(b);
            return (b[0] * (max - min)) / 255 + min;
        }

        /// <summary>
        /// Returns the next value from the RandomNumberGenerator.
        /// </summary>
        /// <param name="r">The RandomNumberGenerator object.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The next value from the RandomNumberGenerator within the specified range.</returns>
        public static int Next(this RandomNumberGenerator r, int max) => Next(r, 0, max);

        /// <summary>
        /// Generate a random string. Not cryptographically secure.
        /// </summary>
        /// <returns>A random string</returns>
        public static string GenerateRandomString(this Random r)
        {
            return (char)r.Next(97, 122) + Convert.ToString(r.Next(100000));
        }

        /// <summary>
        /// Generate a cryptographically secure random string.
        /// </summary>
        /// <returns>A random string</returns>
        public static string GenerateRandomString(this RandomNumberGenerator r)
        {
            return (char)r.Next(97, 122) + Convert.ToString(r.Next(100000));
        }
    }
}
