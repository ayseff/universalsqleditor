using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities.IO.FileSystem
{
    /// <summary>
    /// Class that determines whether a file name matches a pattern.
    /// </summary>
    public class FindFilesRegEx
    {
        //static void Main(string[] args)
        //{
        //    string[] names = { "hello.t", "HelLo.tx", "HeLLo.txt", "HeLLo.txtsjfhs", "HeLLo.tx.sdj", "hAlLo20984.txt" };
        //    string[] matches;
        //    matches = FindFilesEmulator("hello.tx", names);
        //    matches = FindFilesEmulator("H*o*.???", names);
        //    matches = FindFilesEmulator("hello.txt", names);
        //    matches = FindFilesEmulator("lskfjd30", names);
        //}

        /// <summary>
        /// Returns files that match the pattern.
        /// </summary>
        /// <param name="pattern">Pattern to which files have to match.</param>
        /// <param name="names">File names to check against the pattern.</param>
        /// <returns>An array of files names that match the pattern.</returns>
        public static string[] FindFiles(string pattern, string[] names)
        {
            var regex = FindFilesPatternToRegex.Convert(pattern);
            if (pattern.Length < 1000)
            {
                return names.Where(name => regex.IsMatch(name)).ToArray();
            }
            return names.AsParallel().Where(name => regex.IsMatch(name)).ToArray();
        }

        /// <summary>
        /// Determines whether a file name matches the pattern.
        /// </summary>
        /// <param name="name">File name to check against the patterns.</param>
        /// /// <param name="patterns">Patterns to which file has to match.</param>
        /// <returns><code>true</code> if the file matches any of the patterns, <code>false</code> otherwise.</returns>
        public static bool MatchesFilePattern(string name, params string[] patterns)
        {
            return patterns.Select(FindFilesPatternToRegex.Convert).Any(regex => regex.IsMatch(name));
        }

        /// <summary>
        /// Returns files that match the patterns.
        /// </summary>
        /// <param name="patterns">Patterns to which files have to match.</param>
        /// <param name="names">File names to check against the patterns.</param>
        /// <returns>An array of files names that match any of the patterns.</returns>
        public static string[] FindFiles(string[] patterns, string[] names)
        {
            var matches = new List<string>();
            Parallel.ForEach(patterns, (pattern, state) =>
                                           {
                                               var match = FindFiles(pattern, names);
                                               lock (matches)
                                               {
                                                   matches.AddRange(match);
                                               }
                                           });
            //foreach (var pattern in patterns)
            //{
            //    matches.AddRange(FindFiles(pattern, names));
            //}
            return matches.ToArray();
        }

        internal static class FindFilesPatternToRegex
        {
            //private static readonly Regex _hasQuestionMarkRegEx = new Regex(@"\?", RegexOptions.Compiled);
            private static readonly Regex _ilegalCharactersRegex = new Regex("[" + @"\/:<>|" + "\"]", RegexOptions.Compiled);
            //private static readonly Regex _catchExtentionRegex = new Regex(@"^\s*.+\.([^\.]+)\s*$", RegexOptions.Compiled);
            //private const string NON_DOT_CHARACTERS = @"[^.]*";

            public static Regex Convert(string pattern)
            {
                if (pattern == null)
                {
                    throw new ArgumentNullException();
                }
                pattern = pattern.Trim();
                if (pattern.Length == 0)
                {
                    throw new ArgumentException("Pattern is empty.");
                }
                if (_ilegalCharactersRegex.IsMatch(pattern))
                {
                    throw new ArgumentException("Patterns contains illegal characters.");
                }
                //var hasExtension = _catchExtentionRegex.IsMatch(pattern);
                //var matchExact = false;
                //if (_hasQuestionMarkRegEx.IsMatch(pattern))
                //{
                //    matchExact = true;
                //}
                //else if (hasExtension)
                //{
                //    matchExact = true; // _catchExtentionRegex.Match(pattern).Groups[1].Length != 3;
                //}
                var regexString = Regex.Escape(pattern);
                regexString = "^" + Regex.Replace(regexString, @"\\\*", ".*");
                regexString = Regex.Replace(regexString, @"\\\?", ".");
                //if (!matchExact && hasExtension)
                //{
                //    regexString += NON_DOT_CHARACTERS;
                //}
                regexString += "$";
                var regex = new Regex(regexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                return regex;
            }
        }
    }
}