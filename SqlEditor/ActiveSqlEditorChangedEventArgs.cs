using System;
using ICSharpCode.TextEditor;

namespace SqlEditor
{
    public class ActiveSqlEditorChangedEventArgs : EventArgs
    {
        public TextEditorControl TextEditorControl { get; set; }

        public ActiveSqlEditorChangedEventArgs(TextEditorControl textEditorControl)
        {
            TextEditorControl = textEditorControl;
        }
    }
}