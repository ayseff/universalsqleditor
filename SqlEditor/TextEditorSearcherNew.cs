using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using ICSharpCode.TextEditor.Document;
using SqlEditor.SearchAndReplace;

namespace SqlEditor
{
    /// <summary>This class finds occurrences of a search string in a text 
    /// editor's IDocument... it's like Find box without a GUI.</summary>
    public class TextEditorSearcherNew
    {
        //private Regex _regex;
        //private SearchDirection _searchDirection = SearchDirection.Down;
        //private SearchDirection _previousSearchDirection = SearchDirection.Down;
        //private IDocument _document;

        //public IDocument Document
        //{
        //    get { return _document; }
        //    set
        //    {
        //        if (_document != value)
        //        {
        //            ClearScanRegion();
        //            _document = value;
        //        }
        //    }
        //}

        //public SearchDirection PreviousSearchDirection
        //{
        //    get { return _previousSearchDirection; }
        //}

        //public SearchDirection SearchDirection
        //{
        //    get { return _searchDirection; }
        //    set
        //    {
        //        _previousSearchDirection = _searchDirection;
        //        _searchDirection = value;
        //    }
        //}

        //// I would have used the TextAnchor class to represent the beginning and 
        //// end of the region to scan while automatically adjusting to changes in 
        //// the document--but for some reason it is sealed and its constructor is 
        //// internal. Instead I use a TextMarker, which is perhaps even better as 
        //// it gives me the opportunity to highlight the region. Note that all the 
        //// markers and coloring information is associated with the text document, 
        //// not the editor control, so TextEditorSearcher doesn't need a reference 
        //// to the TextEditorControl. After adding the marker to the document, we
        //// must remember to remove it when it is no longer needed.
        //TextMarker _region;
        ///// <summary>Sets the region to search. The region is updated 
        ///// automatically as the document changes.</summary>
        //public void SetScanRegion(ISelection sel)
        //{
        //    SetScanRegion(sel.Offset, sel.Length);
        //}
        ///// <summary>Sets the region to search. The region is updated 
        ///// automatically as the document changes.</summary>
        //public void SetScanRegion(int offset, int length)
        //{
        //    var bkgColor = _document.HighlightingStrategy.GetColorFor("Default").BackgroundColor;
        //    _region = new TextMarker(offset, length, TextMarkerType.SolidBlock,
        //        bkgColor.HalfMix(Color.FromArgb(160, 160, 160)));
        //    _document.MarkerStrategy.AddMarker(_region);
        //}
        //public bool HasScanRegion
        //{
        //    get { return _region != null; }
        //}
        //public void ClearScanRegion()
        //{
        //    if (_region != null)
        //    {
        //        _document.MarkerStrategy.RemoveMarker(_region);
        //        _region = null;
        //    }
        //}
        //public void Dispose() { ClearScanRegion(); GC.SuppressFinalize(this); }
        //~TextEditorSearcherNew() { Dispose(); }

        ///// <summary>Begins the start offset for searching</summary>
        //public int BeginOffset
        //{
        //    get
        //    {
        //        if (_region != null)
        //            return _region.Offset;
        //        else
        //            return 0;
        //    }
        //}
        ///// <summary>Begins the end offset for searching</summary>
        //public int EndOffset
        //{
        //    get
        //    {
        //        if (_region != null)
        //            return _region.EndOffset;
        //        else
        //            return _document.TextLength;
        //    }
        //}

        //public bool MatchCase { get; set; }

        //public bool MatchWholeWordOnly { get; set; }

        //string _searchTerm;
        ////string _lookFor2; // uppercase in case-insensitive mode
        //public string SearchTerm
        //{
        //    get { return _searchTerm; }
        //    set { _searchTerm = value; }
        //}

        //public bool IsRegex { get; set; }

        //public bool WrapAround { get; set; }

        ///// <summary>Finds next instance of LookFor, according to the search rules 
        ///// (MatchCase, MatchWholeWordOnly).</summary>
        ///// <param name="beginAtOffset">Offset in Document at which to begin the search</param>
        ///// <param name="loopedAround">Whether to loop around.</param>
        ///// <remarks>If there is a match at beginAtOffset precisely, it will be returned.</remarks>
        ///// <returns>Region of document that matches the search string</returns>
        //public TextRange FindNext(int beginAtOffset, out bool loopedAround)
        //{
        //    Debug.Assert(!string.IsNullOrEmpty(_searchTerm));
        //    loopedAround = false;
        //    var searchBackward = SearchDirection == SearchDirection.Up;
        //    _previousSearchDirection = SearchDirection;

        //    var startAt = BeginOffset;
        //    var endAt = EndOffset;
        //    var curOffs = beginAtOffset.InRange(startAt, endAt);
        //    TextRange result;

        //    if (IsRegex)
        //    {
        //        var flags = RegexOptions.Compiled | RegexOptions.Multiline;
        //        if (!MatchCase)
        //        {
        //            flags = flags | RegexOptions.IgnoreCase;
        //        }
        //        try
        //        {
        //            _regex = new Regex(SearchTerm, flags);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new InvalidRegexException(ex.Message);
        //        }

        //        // Run a search
        //        result = FindNextRegexIn(curOffs, endAt);
        //        if (result == null && WrapAround)
        //        {
        //            loopedAround = true;
        //            result = FindNextRegexIn(startAt, curOffs);
        //        }
        //    }
        //    else
        //    {
        //        //_lookFor2 = MatchCase ? _searchTerm : _searchTerm.ToUpperInvariant();
        //        if (searchBackward)
        //        {
        //            result = FindNextIn(startAt, curOffs, true);
        //            if (result == null && WrapAround)
        //            {
        //                loopedAround = true;
        //                result = FindNextIn(curOffs, endAt, true);
        //            }
        //        }
        //        else
        //        {
        //            result = FindNextIn(curOffs, endAt, false);
        //            if (result == null && WrapAround)
        //            {
        //                loopedAround = true;
        //                result = FindNextIn(startAt, curOffs, false);
        //            }
        //        }
        //    }
            
        //    return result;
        //}

        //private TextRange FindNextIn(int offset1, int offset2, bool searchBackward)
        //{
        //    Debug.Assert(offset2 >= offset1);
        //    offset2 -= _searchTerm.Length;

        //    // Make behavior decisions before starting search loop
        //    Func<char, char, bool> matchFirstCh;
        //    Func<int, bool> matchWord;
        //    if (MatchCase)
        //        matchFirstCh = (lookFor, c) => (lookFor == c);
        //    else
        //        matchFirstCh = (lookFor, c) => (lookFor == Char.ToUpperInvariant(c) || lookFor == Char.ToLowerInvariant(c));
        //    if (MatchWholeWordOnly)
        //        matchWord = IsWholeWordMatch;
        //    else
        //        matchWord = IsPartWordMatch;

        //    // Search
        //    var lookForCh = SearchTerm[0];
        //    if (searchBackward)
        //    {
        //        for (var offset = offset2; offset >= offset1; offset--)
        //        {
        //            if (matchFirstCh(lookForCh, _document.GetCharAt(offset))
        //                && matchWord(offset))
        //                return new TextRange(offset, _searchTerm.Length);
        //        }
        //    }
        //    else
        //    {
        //        for (var offset = offset1; offset <= offset2; offset++)
        //        {
        //            if (matchFirstCh(lookForCh, _document.GetCharAt(offset))
        //                && matchWord(offset))
        //                return new TextRange(offset, _searchTerm.Length);
        //        }
        //    }
        //    return null;
        //}

        //private TextRange FindNextRegexIn(int offset1, int offset2)
        //{
        //    Debug.Assert(offset2 >= offset1);
        //    var searchText = _document.TextContent;
        //    var searchTextLength = offset2 - offset1;
        //    var match = _regex.Match(searchText, offset1, searchTextLength);
        //    if (match.Success)
        //    {
        //        return new TextRange(match.Index, match.Length); 
        //    }
        //    return null;
        //}
        //private bool IsWholeWordMatch(int offset)
        //{
        //    if (IsWordBoundary(offset) && IsWordBoundary(offset + _searchTerm.Length))
        //        return IsPartWordMatch(offset);
        //    else
        //        return false;
        //}
        //private bool IsWordBoundary(int offset)
        //{
        //    return offset <= 0 || offset >= _document.TextLength ||
        //           !IsAlphaNumeric(offset - 1) || !IsAlphaNumeric(offset);
        //}
        //private bool IsAlphaNumeric(int offset)
        //{
        //    char c = _document.GetCharAt(offset);
        //    return Char.IsLetterOrDigit(c) || c == '_';
        //}
        //private bool IsPartWordMatch(int offset)
        //{
        //    var substr = _document.GetText(offset, _searchTerm.Length);
        //    if (MatchCase)
        //    {
        //        return Equals(substr, SearchTerm);
        //    }
        //    return Equals(substr, SearchTerm) || String.Equals(substr, SearchTerm, StringComparison.InvariantCultureIgnoreCase);
        //}
    }
}