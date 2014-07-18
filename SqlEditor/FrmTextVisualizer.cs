using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using SqlEditor.SqlTextEditor;

namespace SqlEditor
{
    public enum TextType
    {
        Text = 0,
        Xml =1 
    }
    public partial class FrmTextVisualizer : Form
    {
        public FrmTextVisualizer(string text, TextType textType)
        {
            InitializeComponent();
            if (textType == TextType.Xml)
            {
                InitializeXmlEditor(_editor);
            }
            else
            {
                InitializeTextEditor(_editor);
            }
            _editor.Document.TextContent = text;
            if (textType == TextType.Xml)
            {
                _editor.Document.FoldingManager.UpdateFoldings(string.Empty, null);
            }
        }

        private static void InitializeTextEditor(TextEditorControlBase textEditorControl)
        {
            textEditorControl.TextEditorProperties = InitializeProperties();
            textEditorControl.ActiveTextAreaControl.TextArea.Refresh(textEditorControl.ActiveTextAreaControl.TextArea.FoldMargin);
        }

        private static void InitializeXmlEditor(TextEditorControlBase textEditorControl)
        {
            textEditorControl.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("XML");
            textEditorControl.Document.FoldingManager.FoldingStrategy = new XmlFoldingStrategy();
            textEditorControl.Document.FormattingStrategy = new XmlFormattingStrategy();
            textEditorControl.TextEditorProperties = InitializeProperties();
            textEditorControl.Document.FoldingManager.UpdateFoldings(string.Empty, null);
            textEditorControl.ActiveTextAreaControl.TextArea.Refresh(textEditorControl.ActiveTextAreaControl.TextArea.FoldMargin);
        }

        private static ITextEditorProperties InitializeProperties()
        {
            var properties = new DefaultTextEditorProperties
            {
                Font = new Font("Courier new", 9, FontStyle.Regular),
                IndentStyle = IndentStyle.Smart,
                ShowSpaces = false,
                LineTerminator = Environment.NewLine,
                ShowTabs = false,
                ShowInvalidLines = false,
                ShowEOLMarker = false,
                TabIndent = 2,
                CutCopyWholeLine = true,
                LineViewerStyle = LineViewerStyle.None,
                MouseWheelScrollDown = true,
                MouseWheelTextZoom = true,
                AllowCaretBeyondEOL = false,
                AutoInsertCurlyBracket = true,
                BracketMatchingStyle = BracketMatchingStyle.After,
                ConvertTabsToSpaces = false,
                ShowMatchingBracket = true,
                EnableFolding = true,
                ShowVerticalRuler = false,
                IsIconBarVisible = true,
                Encoding = Encoding.Unicode
            };
            return properties;
        }
    }
}
