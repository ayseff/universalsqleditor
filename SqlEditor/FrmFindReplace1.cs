using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;
using System.IO;
using SqlEditor.Properties;
using Utilities.Forms.Dialogs;
using Utilities.Text;
using log4net;

namespace SqlEditor
{
    public enum SearchMode
    {
        Find = 0,
        Replace = 1,
        Mark = 2
    }

    public enum SearchDirection
    {
        Down = 0,
        Up = 1
    }

    public partial class FrmFindReplace1 : Form
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<TextEditorControl, HighlightGroup> _highlightGroups = new Dictionary<TextEditorControl, HighlightGroup>();
        private bool _lastSearchLoopedAround;
        private readonly TextEditorSearcherNew _searcher;
        private TextEditorControl _editor;

        public FrmFindReplace1()
        {
            InitializeComponent();
            _searcher = new TextEditorSearcherNew();

            LoadSettings();
        }

        private void LoadSettings()
        {
            _log.Debug("Loading for geometry ...");
            if (!Settings.Default.FrmFindReplace_Geometry.IsNullEmptyOrWhitespace())
            {
                try
                {
                    Utilities.Forms.RestoreFormPosition.GeometryFromString(Settings.Default.FrmFindReplace_Geometry, this);
                }
                catch (Exception ex)
                {
                    _log.Error("Error loading form geometry.");
                    _log.Error(ex.Message, ex);
                    throw;
                }
            }
            else
            {
                _log.Warn("Form geometry is not set.");
            }
        }



        
        private TextEditorControl Editor
        {
            set
            {
                _editor = value;
                _searcher.Document = _editor.Document;
                UpdateTitleBar();
            }
        }

        private void UpdateTitleBar()
        {
            //var text = ReplaceMode ? "Find & Replace" : "Find";
            //if (_editor != null && _editor.FileName != null)
            //    text += " - " + Path.GetFileName(_editor.FileName);
            //if (_searcher.HasScanRegion)
            //    text += " (selection only)";
            //this.Text = text;
        }

        public void ShowFor(TextEditorControl editor, SearchMode searchMode)
        {
            Editor = editor;
            _searcher.ClearScanRegion();

            string searchText = null;
            var sm = editor.ActiveTextAreaControl.SelectionManager;
            if (sm.HasSomethingSelected && sm.SelectionCollection.Count == 1)
            {
                var sel = sm.SelectionCollection[0];
                if (sel.StartPosition.Line == sel.EndPosition.Line)
                {
                    searchText = sm.SelectedText;
                }
                else
                {
                    _searcher.SetScanRegion(sel);
                }
            }
            else
            {
                // Get the current word that the caret is on
                var caret = editor.ActiveTextAreaControl.Caret;
                var start = TextUtilities.FindWordStart(editor.Document, caret.Offset);
                var endAt = TextUtilities.FindWordEnd(editor.Document, caret.Offset);
                searchText = editor.Document.GetText(start, endAt - start);
            }

            this.Owner = (Form)editor.TopLevelControl;
            this.Show();

            // Activate appropriate tab
            if (searchMode == SearchMode.Find)
            {
                _utcTabs.Tabs["Find"].Selected = true;
                if (searchText != null)
                {
                    _ucFind.Text = searchText;
                }
                _ucFind.SelectAll();
                _ucFind.Focus();
            }
            else if (searchMode == SearchMode.Replace)
            {
                _utcTabs.Tabs["Replace"].Selected = true;
                if (searchText != null)
                {
                    _ucReplaceFind.Text = searchText;
                }
                _cbInSelection.Checked = _searcher.HasScanRegion;
                _ucReplaceFind.SelectAll();
                _ucReplaceFind.Focus();
            }
            else if (searchMode == SearchMode.Mark)
            {
                _utcTabs.Tabs["Mark"].Selected = true;
                //_ucFind.Focus();
            }
        }

        private void BtnFindPrevious_Click(object sender, EventArgs e)
        {
            FindNext();
        }
        private void BtnFindNext_Click(object sender, EventArgs e)
        {
            FindNext();
        }



        public TextRange FindNext()
        {
            _ulStatus.Text = "Ready.";
            _ulStatus.ForeColor = Color.Green;

            RemoveHighlighting();

            var searchText = _ucFind.Text;
            if (string.IsNullOrEmpty(searchText))
            {
                _ulStatus.ForeColor = Color.Red;
                _ulStatus.Text = "Nothing specified to find.";
                return null;
            }

            //_lastSearchWasBackward = _searcher.SearchDirection == SearchDirection.Up;
            _searcher.SearchTerm = searchText;

            // Make sure that the editor we're searching for is focused
            //if (!_editor.Visible)
            //{
            //    _editor.Focus();
            //    Focus();
            //}

            //var caret = _editor.ActiveTextAreaControl.Caret;
            //if (viaF3 && _searcher.HasScanRegion && !caret.Offset.
            //    IsInRange(_searcher.BeginOffset, _searcher.EndOffset))
            //{
            //    // user moved outside of the originally selected region
            //    _searcher.ClearScanRegion();
            //    UpdateTitleBar();
            //}

            var caret = _editor.ActiveTextAreaControl.Caret;
            int startFrom = caret.Offset;
            if (_searcher.SearchDirection == _searcher.PreviousSearchDirection && _searcher.SearchDirection == SearchDirection.Down)
            {
                startFrom = caret.Offset;
            }
            else if (_searcher.SearchDirection == _searcher.PreviousSearchDirection && _searcher.SearchDirection == SearchDirection.Up)
            {
                startFrom = caret.Offset - 1;
            }
            else if (_searcher.SearchDirection == SearchDirection.Down && _searcher.PreviousSearchDirection == SearchDirection.Up)
            {
                startFrom = caret.Offset;
            }
            else if (_searcher.SearchDirection == SearchDirection.Up && _searcher.PreviousSearchDirection == SearchDirection.Down)
            {
                startFrom = caret.Offset - 1;
            }
            //startFrom = caret.Offset + (_searcher.SearchDirection == SearchDirection.Down ? 1 : 0);
            var range = _searcher.FindNext(startFrom, out _lastSearchLoopedAround);
            if (range != null)
            {
                SelectResult(range);
                if (_lastSearchLoopedAround)
                {
                    _ulStatus.Text = "Match was found at the " + (_searcher.SearchDirection == SearchDirection.Down ?  "beginning" : "end" ) + " of the document.";
                    _ulStatus.ForeColor = Color.Green;
                }
            }
            else
            {
                _ulStatus.ForeColor = Color.Red;
                _ulStatus.Text = "Nothing found.";
            }
            return range;
        }

        private void SelectResult(TextRange range)
        {
            var p1 = _editor.Document.OffsetToPosition(range.Offset);
            var p2 = _editor.Document.OffsetToPosition(range.Offset + range.Length);
            _editor.ActiveTextAreaControl.SelectionManager.SetSelection(p1, p2);
            _editor.ActiveTextAreaControl.ScrollTo(p1.Line, p1.Column);
            // Also move the caret to the end of the selection, because when the user 
            // presses F3, the caret is where we start searching next time.
            _editor.ActiveTextAreaControl.Caret.Position =
                _editor.Document.OffsetToPosition(range.Offset + range.Length);
        }



        private void BtnHighlightAll_Click(object sender, EventArgs e)
        {
            if (!_highlightGroups.ContainsKey(_editor))
                _highlightGroups[_editor] = new HighlightGroup(_editor);
            var group = _highlightGroups[_editor];

            if (string.IsNullOrEmpty(LookFor))
                // Clear highlights
                group.ClearMarkers();
            else
            {
                _searcher.SearchTerm = _ucFind.Text;
                _searcher.MatchCase = _cbMatchCase.Checked;
                _searcher.MatchWholeWordOnly = _cbMatchWholeWord.Checked;

                int offset = 0, count = 0;
                for (; ; )
                {
                    bool looped;
                    TextRange range = _searcher.FindNext(offset, out looped);
                    if (range == null || looped)
                        break;
                    offset = range.Offset + range.Length;
                    count++;

                    var m = new TextMarker(range.Offset, range.Length,
                            TextMarkerType.SolidBlock, Color.Yellow, Color.Black);
                    group.AddMarker(m);
                }
                if (count == 0)
                {
                    Dialog.ShowDialog(Application.ProductName, "Search text not found.", string.Empty);
                }
                else
                {
                    _editor.Refresh();
                }
            }
        }

        private void RemoveHighlighting()
        {
            if (!_highlightGroups.ContainsKey(_editor))
                _highlightGroups[_editor] = new HighlightGroup(_editor);
            HighlightGroup group = _highlightGroups[_editor];
            group.ClearMarkers();
        }

        private void FindAndReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {	// Prevent dispose, as this form can be re-used
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                if (this.Owner != null)
                    this.Owner.Select(); // prevent another app from being activated instead

                e.Cancel = true;
                Hide();

                // Discard search region
                _searcher.ClearScanRegion();
                _editor.Refresh(); // must repaint manually
            }
            else
            {
                SaveSettings();
            }
        }

        private void SaveSettings()
        {
            try
            {
                Settings.Default.FrmFindReplace_Geometry = Utilities.Forms.RestoreFormPosition.GeometryToString(this);
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                _log.Error("Error saving for geometry.");
                _log.Error(ex.Message, ex);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnReplace_Click(object sender, EventArgs e)
        {
            var sm = _editor.ActiveTextAreaControl.SelectionManager;
            if (string.Equals(sm.SelectedText, _ucFind.Text, StringComparison.OrdinalIgnoreCase))
                InsertText(_ucReplaceWith.Text);
            FindNext();
        }

        private void BtnReplaceAll_Click(object sender, EventArgs e)
        {
            var count = 0;
            // BUG FIX: if the replacement string contains the original search string
            // (e.g. replace "red" with "very red") we must avoid looping around and
            // replacing forever! To fix, start replacing at beginning of region (by 
            // moving the caret) and stop as soon as we loop around.
            _editor.Focus();
            _editor.ActiveTextAreaControl.Caret.Position =
                _editor.Document.OffsetToPosition(_searcher.BeginOffset);

            _editor.Document.UndoStack.StartUndoGroup();
            try
            {
                while (FindNext() != null)
                {
                    if (_lastSearchLoopedAround)
                        break;

                    // Replace
                    count++;
                    InsertText(_ucReplaceWith.Text);
                }
            }
            finally
            {
                _editor.Document.UndoStack.EndUndoGroup();
                Focus();
            }
            if (count == 0)
            {
                _ulStatus.ForeColor = Color.Red;
                _ulStatus.Text = "No occurrences found.";
                //Dialog.ShowDialog(Application.ProductName, "No occurrences found.", string.Empty);
            }
            else
            {
                _ulStatus.ForeColor = Color.Blue;
                _ulStatus.Text = string.Format("Replaced {0} occurrence{1}.", count, count > 1 ? "s" : string.Empty);
                //Dialog.ShowDialog(Application.ProductName, string.Format("Replaced {0} occurrence{1}.", count, count > 1 ? "s" : string.Empty), string.Empty);
                //Close();
            }
            Focus();
        }

        private void InsertText(string text)
        {
            var textArea = _editor.ActiveTextAreaControl.TextArea;
            textArea.Document.UndoStack.StartUndoGroup();
            try
            {
                if (textArea.SelectionManager.HasSomethingSelected)
                {
                    textArea.Caret.Position = textArea.SelectionManager.SelectionCollection[0].StartPosition;
                    textArea.SelectionManager.RemoveSelectedText();
                }
                textArea.InsertString(text);
            }
            finally
            {
                textArea.Document.UndoStack.EndUndoGroup();
            }
        }

        public string LookFor { get { return _ucFind.Text; } }

        private void TxtReplaceWith_Enter(object sender, EventArgs e)
        {
            _ucReplaceWith.SelectAll();
        }

        private void SearchMode_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbSearchModeNormal.Checked)
            {
                _searcher.IsRegex = false;
                _cbMatchWholeWord.Enabled = true;
                _rbDirectionUp.Enabled = true;
                _rbDirectionDown.Enabled = true;
            }
            else if (_rbSearchModeRegularExpression.Checked)
            {
                _searcher.IsRegex = true;
                _cbMatchWholeWord.Checked = false;
                _cbMatchWholeWord.Enabled = false;
                _rbDirectionUp.Checked = false;
                _rbDirectionDown.Checked = true;
                _rbDirectionUp.Enabled = false;
                _rbDirectionDown.Enabled = false;
            }
        }

        private void CbMatchWholeWord_CheckedChanged(object sender, EventArgs e)
        {
            if (_cbMatchWholeWord.Checked)
            {
                _searcher.MatchWholeWordOnly = true;
            }
            else
            {
                _searcher.MatchWholeWordOnly = false;
            }
        }

        private void CbMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            if (_cbMatchCase.Checked)
            {
                _searcher.MatchCase = true;
            }
            else
            {
                _searcher.MatchCase = false;
            }
        }

        private void RbDirection_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbDirectionDown.Checked)
            {
                _searcher.SearchDirection = SearchDirection.Down;
            }
            else
            {
                _searcher.SearchDirection = SearchDirection.Up;
            }
        }

        private void CbWrapAround_CheckedChanged(object sender, EventArgs e)
        {
            if (_cbWrapAround.Checked)
            {
                _searcher.WrapAround = true;
            }
            else
            {
                _searcher.WrapAround = false;
            }
        }

        private void CbInSelection_CheckedChanged(object sender, EventArgs e)
        {
            if (_cbInSelection.Checked)
            {
                var sm = _editor.ActiveTextAreaControl.SelectionManager;
                if (sm.HasSomethingSelected && sm.SelectionCollection.Count == 1)
                {
                    var sel = sm.SelectionCollection[0];
                    _searcher.SetScanRegion(sel);
                }
                else
                {
                    Dialog.ShowErrorDialog(Application.ProductName, "Editor must have a selection to use this option.",
                        string.Empty, null);
                    _cbInSelection.Checked = false;
                }
            }
            else
            {
                _searcher.ClearScanRegion();
            }
        }
    }
}
