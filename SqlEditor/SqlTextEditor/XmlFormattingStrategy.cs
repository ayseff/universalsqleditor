// Type: ICSharpCode.TextEditor.Addons.XmlFormattingStrategy
// Assembly: ICSharpCode.TextEditor, Version=2.0.0.1462, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Users\mmedic\Downloads\gregsxmleditor\baseline\lib\ICSharpCode.TextEditor.dll

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;

namespace SqlEditor
{
    public class XmlFormattingStrategy : DefaultFormattingStrategy
    {
        public override void FormatLine(TextArea textArea, int lineNr, int caretOffset, char charTyped)
        {
            try
            {
                if ((int)charTyped == 62)
                {
                    StringBuilder stringBuilder1 = new StringBuilder();
                    for (int offset = Math.Min(caretOffset - 2, textArea.Document.TextLength - 1); offset >= 0; --offset)
                    {
                        char charAt = textArea.Document.GetCharAt(offset);
                        if ((int)charAt == 60)
                        {
                            string str1 = ((object)stringBuilder1).ToString().Trim();
                            if (!str1.StartsWith("/"))
                            {
                                if (!str1.EndsWith("/"))
                                {
                                    bool flag = true;
                                    try
                                    {
                                        new XmlDocument().LoadXml(textArea.Document.TextContent);
                                    }
                                    catch (Exception ex)
                                    {
                                        flag = false;
                                    }
                                    if (!flag)
                                    {
                                        StringBuilder stringBuilder2 = new StringBuilder();
                                        for (int index = str1.Length - 1; index >= 0 && !char.IsWhiteSpace(str1[index]); --index)
                                            stringBuilder2.Append(str1[index]);
                                        string str2 = ((object)stringBuilder2).ToString();
                                        if (str2.Length > 0)
                                        {
                                            if (!str2.StartsWith("!"))
                                            {
                                                if (!str2.StartsWith("?"))
                                                {
                                                    textArea.Document.Insert(caretOffset, "</" + str2 + ">");
                                                    break;
                                                }
                                                else
                                                    break;
                                            }
                                            else
                                                break;
                                        }
                                        else
                                            break;
                                    }
                                    else
                                        break;
                                }
                                else
                                    break;
                            }
                            else
                                break;
                        }
                        else
                            stringBuilder1.Append(charAt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if ((int) charTyped != 10)
            {
            }
            else
            {
                this.IndentLine(textArea, lineNr);
            }
        }

        protected override int SmartIndentLine(TextArea textArea, int lineNr)
        {
            if (lineNr <= 0)
                return this.AutoIndentLine(textArea, lineNr);
            try
            {
                this.TryIndent(textArea, lineNr, lineNr);
                return this.GetIndentation(textArea, lineNr).Length;
            }
            catch (XmlException ex)
            {
                return this.AutoIndentLine(textArea, lineNr);
            }
        }

        public override void IndentLines(TextArea textArea, int begin, int end)
        {
            this.TryIndent(textArea, begin, end);
        }

        private void TryIndent(TextArea textArea, int begin, int end)
        {
            string str1 = "";
            Stack stack = new Stack();
            IDocument document = textArea.Document;
            string indentationString = Tab.GetIndentationString(document);
            int lineNumber1 = begin;
            int x = 0;
            bool flag = false;
            XmlNodeType xmlNodeType = XmlNodeType.XmlDeclaration;
            XmlTextReader xmlTextReader = (XmlTextReader)null;
            using (StringReader stringReader = new StringReader(document.TextContent))
            {
                try
                {
                    xmlTextReader = new XmlTextReader((TextReader)stringReader);
                label_25:
                    while (xmlTextReader.Read())
                    {
                        if (flag)
                            str1 = stack.Count != 0 ? (string)stack.Pop() : "";
                        if (xmlTextReader.NodeType == XmlNodeType.EndElement)
                            str1 = stack.Count != 0 ? (string)stack.Pop() : "";
                        while (xmlTextReader.LineNumber > lineNumber1 && lineNumber1 <= end)
                        {
                            if (xmlNodeType == XmlNodeType.CDATA || xmlNodeType == XmlNodeType.Comment)
                            {
                                ++lineNumber1;
                            }
                            else
                            {
                                LineSegment lineSegment = document.GetLineSegment(lineNumber1);
                                string text1 = document.GetText((ISegment)lineSegment);
                                string text2 = !(text1.Trim() == ">") ? str1 + text1.Trim() : (string)stack.Peek() + text1.Trim();
                                if (text2 != text1)
                                {
                                    document.Replace(lineSegment.Offset, lineSegment.Length, text2);
                                    ++x;
                                }
                                ++lineNumber1;
                            }
                        }
                        if (xmlTextReader.LineNumber <= end)
                        {
                            flag = xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.IsEmptyElement;
                            string str2 = (string)null;
                            if (xmlTextReader.NodeType == XmlNodeType.Element)
                            {
                                stack.Push((object)str1);
                                if (xmlTextReader.LineNumber < begin)
                                    str1 = this.GetIndentation(textArea, xmlTextReader.LineNumber - 1);
                                str2 = xmlTextReader.Name.Length >= 16 ? str1 + indentationString : str1 + new string(' ', 2 + xmlTextReader.Name.Length);
                                str1 = str1 + indentationString;
                            }
                            xmlNodeType = xmlTextReader.NodeType;
                            if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.HasAttributes)
                            {
                                int lineNumber2 = xmlTextReader.LineNumber;
                                xmlTextReader.MoveToAttribute(0);
                                if (xmlTextReader.LineNumber != lineNumber2)
                                    str2 = str1;
                                xmlTextReader.MoveToAttribute(xmlTextReader.AttributeCount - 1);
                                while (true)
                                {
                                    if (xmlTextReader.LineNumber > lineNumber1 && lineNumber1 <= end)
                                    {
                                        LineSegment lineSegment = document.GetLineSegment(lineNumber1);
                                        string text1 = document.GetText((ISegment)lineSegment);
                                        string text2 = str2 + text1.Trim();
                                        if (text2 != text1)
                                        {
                                            document.Replace(lineSegment.Offset, lineSegment.Length, text2);
                                            ++x;
                                        }
                                        ++lineNumber1;
                                    }
                                    else
                                        goto label_25;
                                }
                            }
                        }
                        else
                            break;
                    }
                    xmlTextReader.Close();
                }
                catch (XmlException ex)
                {
                }
                finally
                {
                    if (xmlTextReader != null)
                        xmlTextReader.Close();
                }
            }
            if (x <= 1)
                return;
            document.UndoStack.Undo();
        }
    }
}
