using System;
using System.Globalization;

namespace Utilities.Text
{
    public static class TextExtensions
    {
        public static bool IsNullEmptyOrWhitespace(this string text)
        {
            return string.IsNullOrEmpty(text) ||  string.IsNullOrWhiteSpace(text);
        }

        public static string ToPascalCase(this string text)
        {
            if (text.IsNullEmptyOrWhitespace())
            {
                return text;
            }
            else if (text.Length <= 1)
            {
                return text.ToUpper();
            }
            return text[0].ToString(CultureInfo.InvariantCulture).ToUpper() + text.Substring(1).ToLower();
        }

        public static string Chop(this string text, int length)
        {
            if (text == null)
            {
                return null;
            }
            return text.Substring(0, Math.Min(length, text.Length));
        }
    }
}
