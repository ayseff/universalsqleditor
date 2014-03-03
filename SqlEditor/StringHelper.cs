using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SqlEditor
{
    public class StringHelper
    {
        public static string[] Split(string text, string[] delimiters, params string[] qualifiers)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (text == String.Empty)
            {
                return new[] { text };
            }
            else if (delimiters == null)
            {
                throw new ArgumentNullException("delimiters");
            }
            else if (qualifiers == null)
            {
                throw new ArgumentNullException("qualifiers");
            }
            else if (delimiters.Length == 0)
            {
                return new[] { text };
            }
            else if (qualifiers.Length == 0)
            {
                return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            }

            string currentQualifier = null;
            int startIndex = 0;
            List<string> values = new List<string>();

            for (int charIndex = 0; charIndex < text.Length; charIndex++)
            {
                if (currentQualifier != null && String.CompareOrdinal(text.Substring(charIndex, Math.Min(currentQualifier.Length, text.Length - charIndex)), currentQualifier) == 0)
                {
                    charIndex += currentQualifier.Length;
                    currentQualifier = null;
                }
                else if (currentQualifier == null)
                {
                    foreach (string qualifier in qualifiers)
                    {
                        if (String.CompareOrdinal(text.Substring(charIndex, Math.Min(qualifier.Length, text.Length - charIndex)), qualifier) == 0)
                        {
                            currentQualifier = qualifier;
                            break;
                        }
                    }
                }

                foreach (string delimiter in delimiters)
                {
                    if (currentQualifier == null && delimiter != null && String.CompareOrdinal(text.Substring(charIndex, Math.Min(delimiter.Length, text.Length - charIndex)), delimiter) == 0)
                    {
                        string val = text.Substring(startIndex, charIndex - startIndex);
                        if (val != delimiter)
                        {
                            values.Add(val);
                        }
                        else
                        {
                            values.Add(String.Empty);
                        }

                        startIndex = charIndex + 1;
                        break;
                    }
                }
            }

            if (startIndex < text.Length)
                values.Add(text.Substring(startIndex));

            return values.ToArray();
        }

        public string[] Split(string expression, string delimiter, string qualifier, bool ignoreCase)
        {
            string statement = String.Format
                ("{0}(?=(?:[^{1}]*{1}[^{1}]*{1})*(?![^{1}]*{1}))",
                                Regex.Escape(delimiter), Regex.Escape(qualifier));

            RegexOptions options = RegexOptions.Compiled | RegexOptions.Multiline;
            if (ignoreCase) options = options | RegexOptions.IgnoreCase;

            Regex regex = new Regex(statement, options);
            return regex.Split(expression);
        }

        public static int IndexOfNextNonQualifiedCharacter(string text, int startPosition, string[] delimiters, params string[] qualifiers)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (text == String.Empty)
            {
                return 0;
            }
            else if (delimiters == null)
            {
                throw new ArgumentNullException("delimiters");
            }
            else if (qualifiers == null)
            {
                throw new ArgumentNullException("qualifiers");
            }
            else if (delimiters.Length == 0)
            {
                return 0;
            }
            else if (qualifiers.Length == 0)
            {
                int posn = text.Length - 1;
                foreach (string delimiter in delimiters)
                {
                    int delimiterPosition = text.IndexOf(delimiter);
                    if (delimiterPosition >= 0  && delimiterPosition < posn)
                    {
                        posn = delimiterPosition;
                    }
                }
                return posn;
            }

            string currentQualifier = null;
            for (int charIndex = startPosition; charIndex < text.Length; charIndex++)
            {
                if (currentQualifier != null && String.Compare(text.Substring(charIndex, Math.Min(currentQualifier.Length, text.Length - charIndex)), currentQualifier) == 0)
                {
                    charIndex += currentQualifier.Length;
                    currentQualifier = null;
                }
                else if (currentQualifier == null)
                {
                    foreach (string qualifier in qualifiers)
                    {
                        if (String.Compare(text.Substring(charIndex, Math.Min(qualifier.Length, text.Length - charIndex)), qualifier) == 0)
                        {
                            currentQualifier = qualifier;
                            break;
                        }
                    }
                }

                foreach (string delimiter in delimiters)
                {
                    if (currentQualifier == null && delimiter != null && String.Compare(text.Substring(charIndex, Math.Min(delimiter.Length, text.Length - charIndex)), delimiter) == 0)
                    {
                        return charIndex;
                    }
                }
            }

            return text.Length;
        }

        public static int IndexOfPreviousNonQualifiedCharacter(string text, int startPosition, string[] delimiters, params string[] qualifiers)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (text == String.Empty)
            {
                return 0;
            }
            else if (delimiters == null)
            {
                throw new ArgumentNullException("delimiters");
            }
            else if (qualifiers == null)
            {
                throw new ArgumentNullException("qualifiers");
            }
            else if (delimiters.Length == 0)
            {
                return 0;
            }

            for (int i = 0; i < delimiters.Length; i++)
            {
                delimiters[i] = Reverse(delimiters[i]);
            }

            for (int i = 0; i < qualifiers.Length; i++)
            {
                qualifiers[i] = Reverse(qualifiers[i]);
            }

            var reversedText = Reverse(text);
            int pos = IndexOfNextNonQualifiedCharacter(reversedText, text.Length - startPosition, delimiters,
                                                        qualifiers);
            return text.Length - pos;
        }

        public static string Reverse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            char[] charArray = text.ToCharArray();
            Array.Reverse( charArray );
            return new string( charArray );
        }

        public static string ToCamelCase(string text)
        {
            string camelCaseText;
            if (text.Length == 0)
            {
                return text;
            }
            else if (text.Length == 1)
            {
                camelCaseText = text.ToUpper();
            }
            else
            {
                camelCaseText = text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
            }
            return camelCaseText;
        }

        public static int PreviousIndexOfWhiteSpace(string text, int startPosition)
        {
            for (int i = startPosition; i > 0; i--)
            {
                if (char.IsWhiteSpace(text.Substring(i - 1, 1)[0]))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
