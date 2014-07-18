using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

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

        /// <summary>
        /// Pretty prints XML document
        /// </summary>
        /// <param name="xmlText">Actual XML text of the document</param>
        /// <returns>Pretty printed XML document</returns>
        public static string PrettyPrintXml(this string xmlText)
        {
            using (var mStream = new MemoryStream())
            {
                using (var writer = new XmlTextWriter(mStream, Encoding.Unicode))
                {
                    var document = new XmlDocument();

                    // Load the XmlDocument with the XML.
                    document.LoadXml(xmlText);

                    writer.Formatting = Formatting.Indented;

                    // Write the XML into a formatting XmlTextWriter
                    document.WriteContentTo(writer);
                    writer.Flush();
                    mStream.Flush();

                    // Have to rewind the MemoryStream in order to read its contents.
                    mStream.Position = 0;

                    // Read MemoryStream contents into a StreamReader.
                    string formattedXml;
                    using (var sReader = new StreamReader(mStream))
                    {
                        formattedXml = sReader.ReadToEnd();
                    }

                    return formattedXml;
                }
            }
        }
    }
}
