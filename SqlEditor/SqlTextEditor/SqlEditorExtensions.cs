using System;
using System.Linq;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using SqlEditor.Annotations;
using Utilities.Text;

namespace SqlEditor.SqlTextEditor
{
    public static class SqlEditorExtensions
    {
        public static void AppendText([NotNull] this TextEditorControl textEditorControl, string text)
        {
            if (textEditorControl == null) throw new ArgumentNullException("textEditorControl");
            textEditorControl.Document.Insert(textEditorControl.ActiveTextAreaControl.Caret.Offset, text);
        }

        public static void AppendTextAtEnd([NotNull] this TextEditorControl textEditorControl, string text)
        {
            if (textEditorControl == null) throw new ArgumentNullException("textEditorControl");
            textEditorControl.Document.TextContent += text;
            textEditorControl.Refresh();
        }

        public static string GetQueryText([NotNull] this TextEditorControl textEditorControl,
                                          [NotNull] string[] queryDelimiters)
        {
            if (textEditorControl == null) throw new ArgumentNullException("textEditorControl");
            if (queryDelimiters == null) throw new ArgumentNullException("queryDelimiters");

            var selectedText = textEditorControl.ActiveTextAreaControl.SelectionManager.SelectedText.Trim();
            if (!selectedText.IsNullEmptyOrWhitespace())
            {
                if (queryDelimiters.Any(d => selectedText.EndsWith(d)))
                {
                    selectedText = selectedText.Substring(0, selectedText.Length - 1);
                }
                return selectedText;
            }

            var pos = textEditorControl.ActiveTextAreaControl.Caret.Position;
            var position = textEditorControl.Document.PositionToOffset(pos);
            var end = StringHelper.IndexOfNextNonQualifiedCharacter(textEditorControl.Document.TextContent, position,
                                                                    queryDelimiters, "'", "\"");

            var start = StringHelper.IndexOfPreviousNonQualifiedCharacter(textEditorControl.Document.TextContent, position,
                                                                          queryDelimiters, "'", "\"");


            var query = textEditorControl.Document.GetText(start, end - start).Trim();
            if (queryDelimiters.Any(d => query.EndsWith(d)))
            {
                query = query.Substring(0, query.Length - 1);
            }
            return query;
        }

        public static void DoEditAction([NotNull] this TextEditorControl editor,
                                        [NotNull] ICSharpCode.TextEditor.Actions.IEditAction action)
        {
            if (editor == null) throw new ArgumentNullException("editor");
            if (action == null) throw new ArgumentNullException("action");

            var area = editor.ActiveTextAreaControl.TextArea;
            editor.BeginUpdate();
            try
            {
                lock (editor.Document)
                {
                    action.Execute(area);
                    if (area.SelectionManager.HasSomethingSelected && area.AutoClearSelection /*&& caretchanged*/)
                    {
                        if (area.Document.TextEditorProperties.DocumentSelectionMode == DocumentSelectionMode.Normal)
                        {
                            area.SelectionManager.ClearSelection();
                        }
                    }
                }
            }
            finally
            {
                editor.EndUpdate();
                area.Caret.UpdateCaretPosition();
            }
        }

        public static void ToProperCase([NotNull] this TextEditorControl editor)
        {
            if (editor == null) throw new ArgumentNullException("editor");
            if (!editor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) return;

            var selectedText = editor.ActiveTextAreaControl.SelectionManager.SelectedText;
            var camelCaseText = StringHelper.ToCamelCase(selectedText);
            var selectionStart = editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset;
            var selectionLength = editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Length;
            editor.Document.Replace(selectionStart, selectionLength, camelCaseText);
        }

        public static void ToUpperCase([NotNull] this TextEditorControl editor)
        {
            if (editor == null) throw new ArgumentNullException("editor");
            if (!editor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) return;

            var upperText = editor.ActiveTextAreaControl.SelectionManager.SelectedText.ToUpper();
            var selectionStart = editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset;
            var selectionLength = editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Length;
            editor.Document.Replace(selectionStart, selectionLength, upperText);
        }

        public static void ToLowerCase([NotNull] this TextEditorControl editor)
        {
            if (editor == null) throw new ArgumentNullException("editor");
            if (!editor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) return;

            var lowerText = editor.ActiveTextAreaControl.SelectionManager.SelectedText.ToLower();
            var selectionStart = editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset;
            var selectionLength = editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Length;
            editor.Document.Replace(selectionStart, selectionLength, lowerText);
        }
    }
}
