using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;
using log4net;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlEditor.SqlTextEditor;
using Utilities.Forms.Dialogs;

namespace SqlEditor
{
    public enum TextType
    {
        Text = 0,
        Xml =1 
    }
    public partial class FrmTextVisualizer : Form
    {
        private readonly TextType _textType;
        private readonly FrmFindReplaceSimple _findForm = new FrmFindReplaceSimple();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FrmTextVisualizer(string text, TextType textType)
        {
            _textType = textType;
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

        private void Utm_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "Save":
                        Save();
                        break;

                    case "Copy":
                        _editor.DoEditAction(new Copy());
                        break;

                    case "Undo":
                        _editor.DoEditAction(new Undo());
                        break;

                    case "Redo":
                        _editor.DoEditAction(new Redo());
                        break;

                    case "Find":
                        _findForm.ShowFor(_editor, false);
                        break;

                    case "Replace":
                        _findForm.ShowFor(_editor, true);
                        break;

                }
            }
            catch (Exception ex)
            {
                const string message = "Error performing operation.";
                _log.Error(message);
                _log.Error(ex.Message, ex);
                Dialog.ShowErrorDialog(Application.ProductName, message, ex.Message, ex.StackTrace);
            }
        }

        private void Save()
        {
            string selectedFile;
            var commonFileDialogFilters = new[]
                                          {
                                              new CommonFileDialogFilter("Text files", ".txt"),
                                              new CommonFileDialogFilter("All files", "*.*")
                                          };
            if (_textType == TextType.Xml)
            {
                commonFileDialogFilters = new[]
                                          {
                                              new CommonFileDialogFilter("XML files", ".xml"),
                                              new CommonFileDialogFilter("All files", "*.*")
                                          };
            }
            
            var dialogResult = Dialog.ShowSaveFileDialog("Select a file", out selectedFile,
                                                 commonFileDialogFilters);
            if (dialogResult != CommonFileDialogResult.Ok) return;
            _editor.SaveFile(selectedFile);
        }
    }
}
