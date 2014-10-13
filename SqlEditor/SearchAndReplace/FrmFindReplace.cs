using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Infragistics.Win.UltraWinGrid;
using log4net;
using SqlEditor.Properties;
using SqlEditor.SearchAndReplace.Engine;
using SqlEditor.SearchAndReplace.Engine.DocumentIterator;
using Utilities.Collections;
using Utilities.Text;

namespace SqlEditor.SearchAndReplace
{
    public enum SearchMode
    {
        Find = 0,
        Replace = 1
    }

    public partial class FrmFindReplace : Form
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //private readonly Dictionary<TextEditorControl, HighlightGroup> _highlightGroups = new Dictionary<TextEditorControl, HighlightGroup>();
        private TextEditorControl _textEditor;
        private readonly BindingList<string> _searchTerms = new BindingList<string>();
        private readonly BindingList<string> _replaceTerms = new BindingList<string>();
        private ISelection _selection;
        private bool _ignoreSelectionChanges;
        private bool _findFirst;

        public FrmFindReplace()
        {
            InitializeComponent();

            LoadSettings();
            
            _ucFind.DataSource = _searchTerms;
            _ucFind.DisplayLayout.Bands[0].ColHeadersVisible = false;
            _ucReplaceFind.DataSource = _searchTerms;
            _ucReplaceFind.DisplayLayout.Bands[0].ColHeadersVisible = false;
            _ucReplaceWith.DataSource = _replaceTerms;
            _ucReplaceWith.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
                }
            }
            else
            {
                _log.Warn("Form geometry is not set.");
            }

            _log.Debug("Loading search terms ...");
            try
            {
                if (!string.IsNullOrEmpty(Settings.Default.FrmFindReplace_SearchTerms))
                {
                    var list = Deserialize(Settings.Default.FrmFindReplace_SearchTerms);
                    _searchTerms.AddRange(list);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error loading search terms.");
                _log.Error(ex.Message, ex);
            }

            _log.Debug("Loading replace terms ...");
            try
            {
                if (!string.IsNullOrEmpty(Settings.Default.FrmFindReplace_ReplaceTerms))
                {
                    var list = Deserialize(Settings.Default.FrmFindReplace_ReplaceTerms);
                    _replaceTerms.AddRange(list);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error loading replace terms.");
                _log.Error(ex.Message, ex);
            }
        }

        public void Show(SearchMode searchMode)
        {
            var editor = FrmMdiParent.Instance.GetActiveSqlTextEditor();
            var currentSelection = GetCurrentTextSelection();
            var searchText = currentSelection != null && !IsMultipleLineSelection(currentSelection) ? currentSelection.SelectedText : null;
            if (searchText == null)
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
                _cbInSelection.Checked = IsMultipleLineSelection(_selection);
                _ucReplaceFind.SelectAll();
                _ucReplaceFind.Focus();
            }
        }

        void SetSearchOptions(SearchMode searchMode)
        {
            if (searchMode == SearchMode.Find)
            {
                SearchOptions.FindPattern = _ucFind.Text;
                MoveSearchTermToTop(_ucFind.Text, _searchTerms);
            }
            else if (searchMode == SearchMode.Replace)
            {
                SearchOptions.FindPattern = _ucReplaceFind.Text;
                SearchOptions.ReplacePattern = _ucReplaceWith.Text;
                MoveSearchTermToTop(_ucReplaceFind.Text, _searchTerms);
                MoveSearchTermToTop(_ucReplaceWith.Text, _replaceTerms);
            }
            SearchOptions.MatchCase = _cbMatchCase.Checked;
            SearchOptions.MatchWholeWord = _cbMatchWholeWord.Checked;
            SearchOptions.SearchStrategyType = SearchStrategyType.Normal;
            if (_rbSearchModeRegularExpression.Checked)
            {
                SearchOptions.SearchStrategyType = SearchStrategyType.RegEx;
            }
            SearchOptions.DocumentIteratorType = DocumentIteratorType.CurrentDocument;
            if (_cbInSelection.Checked && searchMode == SearchMode.Replace)
            {
                SearchOptions.DocumentIteratorType = DocumentIteratorType.CurrentSelection;
                InitSelectionSearch();
            }
            else
            {
                RemoveSelectionSearchHandlers();
            }
        }

        private static void MoveSearchTermToTop(string text, IList<string> list)
        {
            var index = list.IndexOf(text);
            while (index >= 0)
            {
                list.RemoveAt(index);
                index = list.IndexOf(text);
            }
            list.Insert(0, text);
        }

        void RemoveSelectionSearchHandlers()
        {
            RemoveSelectionChangedHandler();
            RemoveActiveWindowChangedHandler();
        }

        void RemoveActiveWindowChangedHandler()
        {
            FrmMdiParent.Instance.ActiveWorksheetChanged -= ActiveWorksheetChanged;
        }

        void InitSelectionSearch()
        {
            _findFirst = true;
            _selection = GetCurrentTextSelection();
            AddSelectionChangedHandler(FrmMdiParent.Instance.GetActiveSqlTextEditor());
            FrmMdiParent.Instance.ActiveWorksheetChanged += ActiveWorksheetChanged;
        }

        void ActiveWorksheetChanged(object source, ActiveWorksheetChangedEventArgs e)
        {
            TextEditorControl activeTextEditorControl = FrmMdiParent.Instance.GetActiveSqlTextEditor();
            if (activeTextEditorControl != this._textEditor)
            {
                AddSelectionChangedHandler(activeTextEditorControl);
                TextSelectionChanged(source, e);
            }
        }

        void AddSelectionChangedHandler(TextEditorControl textEditor)
        {
            RemoveSelectionChangedHandler();

            this._textEditor = textEditor;
            if (textEditor != null)
            {
                this._textEditor.ActiveTextAreaControl.SelectionManager.SelectionChanged += TextSelectionChanged;
            }
        }

        void RemoveSelectionChangedHandler()
        {
            if (_textEditor != null)
            {
                _textEditor.ActiveTextAreaControl.SelectionManager.SelectionChanged -= TextSelectionChanged;
            }
        }
        /// <summary>
        /// When the selected text is changed make sure the 'Current Selection'
        /// option is not selected if no text is selected.
        /// </summary>
        /// <remarks>The text selection can change either when the user
        /// selects different text in the editor or the active window is
        /// changed.</remarks>
        void TextSelectionChanged(object source, EventArgs e)
        {
            if (!_ignoreSelectionChanges)
            {
                _selection = GetCurrentTextSelection();
                _findFirst = true;
            }
        }
		

        /// <summary>
        /// Returns the first ISelection object from the currently active text editor
        /// </summary>
        static ISelection GetCurrentTextSelection()
        {
            var textArea = FrmMdiParent.Instance.GetActiveSqlTextEditor();
            if (textArea != null)
            {
                var selectionManager = textArea.ActiveTextAreaControl.SelectionManager;
                if (selectionManager.HasSomethingSelected)
                {
                    return selectionManager.SelectionCollection[0];
                }
            }
            return null;
        }

        static bool IsMultipleLineSelection(ISelection selection)
        {
            if (IsTextSelected(selection))
            {
                return selection.SelectedText.IndexOf('\n') != -1;
            }
            return false;
        }

        bool IsSelectionSearch
        {
            get
            {
                return SearchOptions.DocumentIteratorType == DocumentIteratorType.CurrentSelection;
            }
        }

        static bool IsTextSelected(ISelection selection)
        {
            if (selection != null)
            {
                return !selection.IsEmpty;
            }
            return false;
        }

        void SetCaretPosition(TextArea textArea, int offset)
        {
            textArea.Caret.Position = textArea.Document.OffsetToPosition(offset);
        }

        bool FindNextInSelection()
        {
            int startOffset = Math.Min(_selection.Offset, _selection.EndOffset);
            int endOffset = Math.Max(_selection.Offset, _selection.EndOffset);

            if (_findFirst)
            {
                SetCaretPosition(_textEditor.ActiveTextAreaControl.TextArea, startOffset);
            }

            try
            {
                _ignoreSelectionChanges = true;
                if (_findFirst)
                {
                    _findFirst = false;
                    return SearchReplaceManager.FindFirstInSelection(startOffset, endOffset - startOffset);
                }
                else
                {
                    _findFirst = !SearchReplaceManager.FindNextInSelection();
                    if (_findFirst)
                    {
                        SearchReplaceUtilities.SelectText(_textEditor, startOffset, endOffset);
                    }
                    return _findFirst;
                }
            }
            finally
            {
                _ignoreSelectionChanges = false;
            }
        }

        private void BtnFindNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_ucFind.Text))
            {
                _ulStatus.Text = "No search term specified.";
                _ulStatus.ForeColor = Color.Red;
                return;
            }

            SetSearchOptions(SearchMode.Find);
            var found = SearchReplaceManager.FindNext();
            if (found)
            {
                _ulStatus.Text = "Match found.";
                _ulStatus.ForeColor = Color.Green;
            }
            else
            {
                _ulStatus.Text = "No match found.";
                _ulStatus.ForeColor = Color.Red;
            }
            Focus();
        }



        //public TextRange FindNext(string searchText, bool throwOnInvalidRegex = false)
        //{
        //    try
        //    {
        //        _ulStatus.Text = "Ready.";
        //        _ulStatus.ForeColor = Color.Green;

        //        RemoveHighlighting();

        //        if (string.IsNullOrEmpty(searchText))
        //        {
        //            _ulStatus.ForeColor = Color.Red;
        //            _ulStatus.Text = "Nothing specified to find.";
        //            return null;
        //        }
            
        //        _searcher.SearchTerm = searchText;

        //        // Make sure that the editor we're searching for is focused
        //        //if (!_editor.Visible)
        //        //{
        //        //    _editor.Focus();
        //        //    Focus();
        //        //}

        //        var caret = _textEditor.ActiveTextAreaControl.Caret;
        //        var startFrom = caret.Offset;
        //        if (_searcher.SearchDirection == _searcher.PreviousSearchDirection && _searcher.SearchDirection == SearchDirection.Down)
        //        {
        //            startFrom = caret.Offset;
        //        }
        //        else if (_searcher.SearchDirection == _searcher.PreviousSearchDirection && _searcher.SearchDirection == SearchDirection.Up)
        //        {
        //            startFrom = caret.Offset - 1;
        //        }
        //        else if (_searcher.SearchDirection == SearchDirection.Down && _searcher.PreviousSearchDirection == SearchDirection.Up)
        //        {
        //            startFrom = caret.Offset;
        //        }
        //        else if (_searcher.SearchDirection == SearchDirection.Up && _searcher.PreviousSearchDirection == SearchDirection.Down)
        //        {
        //            startFrom = caret.Offset - 1;
        //        }
            
        //        TextRange range;
        //        try
        //        {
        //            range = _searcher.FindNext(startFrom, out _lastSearchLoopedAround);
        //        }
        //        catch (InvalidRegexException ex)
        //        {
        //            _ulStatus.ForeColor = Color.Red;
        //            _ulStatus.Text = "Invalid regular expression: " + ex.Message;
        //            if (throwOnInvalidRegex)
        //            {
        //                throw;
        //            }
        //            return null;
        //        }

        //        if (range != null)
        //        {
        //            SelectResult(range);
        //            if (_lastSearchLoopedAround)
        //            {
        //                _ulStatus.Text = "Match was found at the " + (_searcher.SearchDirection == SearchDirection.Down ?  "beginning" : "end" ) + " of the document.";
        //                _ulStatus.ForeColor = Color.Green;
        //            }
        //        }
        //        else
        //        {
        //            _ulStatus.ForeColor = Color.Red;
        //            _ulStatus.Text = "Nothing found.";
        //        }

        //        // Add find term to the combo box results
        //        if (!_searchTerms.Contains(searchText))
        //        {
        //            _searchTerms.Insert(0, searchText);
        //        }
        //        return range;
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error(ex.Message, ex);
        //        Dialog.ShowErrorDialog(Application.ProductName, "Error occurred.", ex.Message, ex.StackTrace);
        //    }
        //    return null;
        //}

        //private void SelectResult(TextRange range)
        //{
        //    var p1 = _textEditor.Document.OffsetToPosition(range.Offset);
        //    var p2 = _textEditor.Document.OffsetToPosition(range.Offset + range.Length);
        //    _textEditor.ActiveTextAreaControl.SelectionManager.SetSelection(p1, p2);
        //    _textEditor.ActiveTextAreaControl.ScrollTo(p1.Line, p1.Column);
        //    // Also move the caret to the end of the selection, because when the user 
        //    // presses F3, the caret is where we start searching next time.
        //    _textEditor.ActiveTextAreaControl.Caret.Position =
        //        _textEditor.Document.OffsetToPosition(range.Offset + range.Length);
        //}



        //private void BtnHighlightAll_Click(object sender, EventArgs e)
        //{
        //    if (!_highlightGroups.ContainsKey(_textEditor))
        //        _highlightGroups[_textEditor] = new HighlightGroup(_textEditor);
        //    var group = _highlightGroups[_textEditor];

        //    if (string.IsNullOrEmpty(LookFor))
        //        // Clear highlights
        //        group.ClearMarkers();
        //    else
        //    {
        //        _searcher.SearchTerm = _ucFind.Text;
        //        _searcher.MatchCase = _cbMatchCase.Checked;
        //        _searcher.MatchWholeWordOnly = _cbMatchWholeWord.Checked;

        //        int offset = 0, count = 0;
        //        for (; ; )
        //        {
        //            bool looped;
        //            TextRange range = _searcher.FindNext(offset, out looped);
        //            if (range == null || looped)
        //                break;
        //            offset = range.Offset + range.Length;
        //            count++;

        //            var m = new TextMarker(range.Offset, range.Length,
        //                    TextMarkerType.SolidBlock, Color.Yellow, Color.Black);
        //            group.AddMarker(m);
        //        }
        //        if (count == 0)
        //        {
        //            Dialog.ShowDialog(Application.ProductName, "Search text not found.", string.Empty);
        //        }
        //        else
        //        {
        //            _textEditor.Refresh();
        //        }
        //    }
        //}

        //private void RemoveHighlighting()
        //{
        //    if (!_highlightGroups.ContainsKey(_textEditor))
        //        _highlightGroups[_textEditor] = new HighlightGroup(_textEditor);
        //    HighlightGroup group = _highlightGroups[_textEditor];
        //    group.ClearMarkers();
        //}

        private void FindAndReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {	// Prevent dispose, as this form can be re-used
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                if (this.Owner != null)
                    this.Owner.Select(); // prevent another app from being activated instead

                e.Cancel = true;
                Hide();

                // Discard search region
                if (_textEditor != null) _textEditor.Refresh(); // must repaint manually
            }
            else
            {
                SaveSettings();
            }
        }

        private void SaveSettings()
        {
            _log.Debug("Saving form geometry ...");
            try
            {
                Settings.Default.FrmFindReplace_Geometry = Utilities.Forms.RestoreFormPosition.GeometryToString(this);
                
            }
            catch (Exception ex)
            {
                _log.Error("Error saving for geometry.");
                _log.Error(ex.Message, ex);
            }

            _log.Debug("Saving search terms ...");
            try
            {
                var list = _searchTerms.Take(25).ToList();
                _searchTerms.Clear();
                _searchTerms.AddRange(list);
                var xml = Serialize(_searchTerms);
                Settings.Default.FrmFindReplace_SearchTerms = xml;
            }
            catch (Exception ex)
            {
                _log.Error("Error saving search terms.");
                _log.Error(ex.Message, ex);
            }

            _log.Debug("Saving replace terms ...");
            try
            {
                var list = _replaceTerms.Take(25).ToList();
                _replaceTerms.Clear();
                _replaceTerms.AddRange(list);
                var xml = Serialize(_replaceTerms);
                Settings.Default.FrmFindReplace_ReplaceTerms = xml;
            }
            catch (Exception ex)
            {
                _log.Error("Error saving replace terms.");
                _log.Error(ex.Message, ex);
            }

            Settings.Default.Save();
        }

        //private void BtnReplace_Click(object sender, EventArgs e)
        //{
        //    var sm = _textEditor.ActiveTextAreaControl.SelectionManager;
        //    if (string.Equals(sm.SelectedText, _ucFind.Text, StringComparison.OrdinalIgnoreCase))
        //        InsertText(_ucReplaceWith.Text);
        //    FindNext(_ucFind.Text);
        //}

        //private void BtnReplaceAll_Click(object sender, EventArgs e)
        //{
        //    var count = 0;
        //    // BUG FIX: if the replacement string contains the original search string
        //    // (e.g. replace "red" with "very red") we must avoid looping around and
        //    // replacing forever! To fix, start replacing at beginning of region (by 
        //    // moving the caret) and stop as soon as we loop around.
        //    _textEditor.Focus();
        //    _textEditor.ActiveTextAreaControl.Caret.Position =
        //        _textEditor.Document.OffsetToPosition(_searcher.BeginOffset);

        //    _textEditor.Document.UndoStack.StartUndoGroup();
        //    try
        //    {
        //        while (FindNext(_ucFind.Text) != null)
        //        {
        //            if (_lastSearchLoopedAround)
        //                break;

        //            // Replace
        //            count++;
        //            InsertText(_ucReplaceWith.Text);
        //        }
        //    }
        //    finally
        //    {
        //        _textEditor.Document.UndoStack.EndUndoGroup();
        //        Focus();
        //    }
        //    if (count == 0)
        //    {
        //        _ulStatus.ForeColor = Color.Red;
        //        _ulStatus.Text = "No occurrences found.";
        //        //Dialog.ShowDialog(Application.ProductName, "No occurrences found.", string.Empty);
        //    }
        //    else
        //    {
        //        _ulStatus.ForeColor = Color.Blue;
        //        _ulStatus.Text = string.Format("Replaced {0} occurrence{1}.", count, count > 1 ? "s" : string.Empty);
        //        //Dialog.ShowDialog(Application.ProductName, string.Format("Replaced {0} occurrence{1}.", count, count > 1 ? "s" : string.Empty), string.Empty);
        //        //Close();
        //    }
        //    Focus();
        //}

        //private void InsertText(string text)
        //{
        //    var textArea = _textEditor.ActiveTextAreaControl.TextArea;
        //    textArea.Document.UndoStack.StartUndoGroup();
        //    try
        //    {
        //        if (textArea.SelectionManager.HasSomethingSelected)
        //        {
        //            textArea.Caret.Position = textArea.SelectionManager.SelectionCollection[0].StartPosition;
        //            textArea.SelectionManager.RemoveSelectedText();
        //        }
        //        textArea.InsertString(text);
        //    }
        //    finally
        //    {
        //        textArea.Document.UndoStack.EndUndoGroup();
        //    }
        //}

        //public string LookFor { get { return _ucFind.Text; } }

        //private void TxtReplaceWith_Enter(object sender, EventArgs e)
        //{
        //    _ucReplaceWith.SelectAll();
        //}

        private void SearchMode_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbSearchModeNormal.Checked)
            {
                _cbMatchWholeWord.Enabled = true;
            }
            else if (_rbSearchModeRegularExpression.Checked)
            {
                _cbMatchWholeWord.Checked = false;
                _cbMatchWholeWord.Enabled = false;
            }
        }

        public static string Serialize(Collection<string> list)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(Collection<string>));
                serializer.Serialize(stringWriter, list);
                return stringWriter.ToString();
            }
        }

        public static Collection<string> Deserialize(string xml)
        {
            Collection<string> other;
            using (StringReader stringReader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(Collection<string>));
                other = (Collection<string>) (serializer.Deserialize(stringReader));
            }
            return other;
        }

        private void UtcTabs_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (e.Tab.Key == "Find")
            {
                this.AcceptButton = _ubFindNext;
                _ucFind.Focus();
            }
            if (e.Tab.Key == "Replace")
            {
                this.AcceptButton = _ubReplaceFindNext;
                _ucReplaceFind.Focus();
            }
        }

        private void UbReplaceFindNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_ucReplaceFind.Text))
            {
                _ulStatus.Text = "No search term specified.";
                _ulStatus.ForeColor = Color.Red;
                return;
            }

            SetSearchOptions(SearchMode.Replace);
            var found = false;
            if (IsSelectionSearch)
            {
                if (IsTextSelected(_selection))
                {
                    found = FindNextInSelection();
                }
            }
            else
            {
                found = SearchReplaceManager.FindNext();
            }

            if (found)
            {
                _ulStatus.Text = "Match found.";
                _ulStatus.ForeColor = Color.Green;
            }
            else
            {
                _ulStatus.Text = "No match found.";
                _ulStatus.ForeColor = Color.Red;
            }
            Focus();

            //var result = FindNext(_ucReplaceFind.Text);
            //if (result != null)
            //{
            //    _replaceMatchFound = true;
            //}
            //else
            //{
            //    _replaceMatchFound = false;
            //}
        }

        private void UbReplace_Click(object sender, EventArgs e)
        {
            var found = false;
            SetSearchOptions(SearchMode.Replace);
            if (IsSelectionSearch)
            {
                if (IsTextSelected(_selection))
                {
                    found = ReplaceInSelection();
                }
            }
            else
            {
                found = SearchReplaceManager.Replace();
            }

            if (found)
            {
                _ulStatus.Text = "Match found.";
                _ulStatus.ForeColor = Color.Green;
            }
            else
            {
                _ulStatus.Text = "No match found.";
                _ulStatus.ForeColor = Color.Red;
            }
            Focus();

            

            //var sm = _editor.ActiveTextAreaControl.SelectionManager;
            //if (_replaceMatchFound && sm.HasSomethingSelected && sm.SelectionCollection.Count == 1)
            //{
            //    InsertText(_ucReplaceWith.Text);
            //}

            //var result = FindNext(_ucReplaceFind.Text);
            //if (result != null)
            //{
            //    _replaceMatchFound = true;
            //}
            //else
            //{
            //    _replaceMatchFound = false;
            //}
        }

        bool ReplaceInSelection()
        {
            var startOffset = Math.Min(_selection.Offset, _selection.EndOffset);
            var endOffset = Math.Max(_selection.Offset, _selection.EndOffset);
            
            if (_findFirst)
            {
                SetCaretPosition(_textEditor.ActiveTextAreaControl.TextArea, startOffset);
            }

            try
            {
                _ignoreSelectionChanges = true;
                if (_findFirst)
                {
                    _findFirst = false;
                    return SearchReplaceManager.ReplaceFirstInSelection(startOffset, endOffset - startOffset);
                }
                else
                {
                    _findFirst = !SearchReplaceManager.ReplaceNextInSelection();
                    if (_findFirst)
                    {
                        SearchReplaceUtilities.SelectText(_textEditor, startOffset, endOffset);
                    }
                    return _findFirst;
                }
            }
            finally
            {
                _ignoreSelectionChanges = false;
            }
        }

        int RunAllInSelection(int action)
        {
            var startOffset = Math.Min(_selection.Offset, _selection.EndOffset);
            var endOffset = Math.Max(_selection.Offset, _selection.EndOffset);

            SearchReplaceUtilities.SelectText(_textEditor, startOffset, endOffset);
            SetCaretPosition(_textEditor.ActiveTextAreaControl.TextArea, startOffset);

            try
            {
                _ignoreSelectionChanges = true;
                var count = 0;
                if (action == 1)
                    count = SearchReplaceManager.MarkAll(startOffset, endOffset - startOffset);
                else if (action == 2)
                    count = SearchReplaceManager.ReplaceAll(startOffset, endOffset - startOffset);
                SearchReplaceUtilities.SelectText(_textEditor, startOffset, endOffset);
                return count;
            }
            finally
            {
                _ignoreSelectionChanges = false;
            }
        }

        private void Uc_Enter(object sender, EventArgs e)
        {
            var ultraCombo = (UltraCombo) sender;
            ultraCombo.SelectAll();
        }

        private void UbReplaceAll_Click(object sender, EventArgs e)
        {
            SetSearchOptions(SearchMode.Replace);
            var replacedCount = 0;
            if (IsSelectionSearch)
            {
                if (IsTextSelected(_selection))
                {
                   replacedCount = RunAllInSelection(2);
                }
            }
            else
            {
                replacedCount = SearchReplaceManager.ReplaceAll();
            }

            if (replacedCount == 0)
            {
                _ulStatus.ForeColor = Color.Red;
                _ulStatus.Text = "No match found.";
            }
            else
            {
                _ulStatus.ForeColor = Color.Blue;
                _ulStatus.Text = string.Format("Replaced {0} match{1}.", replacedCount, replacedCount != 1 ? "es" : string.Empty);
            }

            //try
            //{
            //    var count = 0;
            //    // BUG FIX: if the replacement string contains the original search string
            //    // (e.g. replace "red" with "very red") we must avoid looping around and
            //    // replacing forever! To fix, start replacing at beginning of region (by 
            //    // moving the caret) and stop as soon as we loop around.
            //    //_editor.ActiveTextAreaControl.Caret.Position =
            //    //    _editor.Document.OffsetToPosition(_searcher.BeginOffset);

            //    _textEditor.Document.UndoStack.StartUndoGroup();
            //    try
            //    {
            //        TextRange result;
            //        while ((result = FindNext(_ucReplaceFind.Text, true)) != null)
            //        {
            //            //if (_lastSearchLoopedAround)
            //            //    break;

            //            // Replace
            //            count++;
            //            InsertText(_ucReplaceWith.Text);
            //        }
            //    }
            //    finally
            //    {
            //        _textEditor.Document.UndoStack.EndUndoGroup();
            //        //Focus();
            //    }
            //    if (count == 0)
            //    {
            //        _ulStatus.ForeColor = Color.Blue;
            //        _ulStatus.Text = "No occurrences found.";
            //        //Dialog.ShowDialog(Application.ProductName, "No occurrences found.", string.Empty);
            //    }
            //    else
            //    {
            //        _ulStatus.ForeColor = Color.Blue;
            //        _ulStatus.Text = string.Format("Replaced {0} occurrence{1}.", count, count > 1 ? "s" : string.Empty);
            //        //Dialog.ShowDialog(Application.ProductName, string.Format("Replaced {0} occurrence{1}.", count, count > 1 ? "s" : string.Empty), string.Empty);
            //        //Close();
            //    }
            //    //Focus();
            //}
            //catch (InvalidRegexException ex)
            //{
            //    _ulStatus.ForeColor = Color.Red;
            //    _ulStatus.Text = "Invalid regular expression: " + ex.Message;
            //}
            //catch (Exception)
            //{
                
            //    throw;
            //}
        }
    }
}
