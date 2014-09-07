// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision$</version>
// </file>

using System;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using SqlEditor.SearchAndReplace.Engine.DocumentIterator;
using SqlEditor.SearchAndReplace.Engine.SearchStrategy;

namespace SqlEditor.SearchAndReplace.Engine
{
	public sealed class SearchReplaceUtilities
	{
		public static bool IsTextAreaSelected {
			get {
				return true;
			}
		}
		
        //public static TextEditorControl GetActiveTextEditor()
        //{
        //    IViewContent content = WorkbenchSingleton.Workbench.ActiveViewContent;
        //    if (content is ITextEditorControlProvider) {
        //        return ((ITextEditorControlProvider)content).TextEditorControl;
        //    }
        //    return null;
        //}
		
		public static bool IsWholeWordAt(ITextBufferStrategy document, int offset, int length)
		{
			return (offset - 1 < 0 || Char.IsWhiteSpace(document.GetCharAt(offset - 1))) &&
				(offset + length + 1 >= document.Length || Char.IsWhiteSpace(document.GetCharAt(offset + length)));
		}
		
		public static ISearchStrategy CreateSearchStrategy(SearchStrategyType type)
		{
			switch (type) {
				case SearchStrategyType.Normal:
					return new BruteForceSearchStrategy(); // new KMPSearchStrategy();
				case SearchStrategyType.RegEx:
					return new RegExSearchStrategy();
				default:
					throw new System.NotImplementedException("CreateSearchStrategy for type " + type);
			}
		}
		
		public static IDocumentIterator CreateDocumentIterator(DocumentIteratorType type)
		{
			switch (type) {
				case DocumentIteratorType.CurrentDocument:
				case DocumentIteratorType.CurrentSelection:
					return new CurrentDocumentIterator();
				case DocumentIteratorType.AllOpenFiles:
					return new AllOpenDocumentIterator();
				default:
					throw new System.NotImplementedException("CreateDocumentIterator for type " + type);
			}
		}

	    //public static bool IsSearchable(string fileName)
        //{
        //    if (fileName == null)
        //        return false;
			
        //    if (excludedFileExtensions == null) {
        //        excludedFileExtensions = AddInTree.BuildItems<string>("/AddIns/DefaultTextEditor/Search/ExcludedFileExtensions", null, false);
        //    }
        //    string extension = Path.GetExtension(fileName);
        //    if (extension != null) {
        //        foreach (string excludedExtension in excludedFileExtensions) {
        //            if (String.Compare(excludedExtension, extension, true) == 0) {
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}
		
		public static void SelectText(TextEditorControl textArea, int offset, int endOffset)
		{
			int textLength = textArea.ActiveTextAreaControl.Document.TextLength;
			if (textLength < endOffset) {
				endOffset = textLength - 1;
			}
			textArea.ActiveTextAreaControl.Caret.Position = textArea.Document.OffsetToPosition(endOffset);
			textArea.ActiveTextAreaControl.TextArea.SelectionManager.ClearSelection();
			textArea.ActiveTextAreaControl.TextArea.SelectionManager.SetSelection(new DefaultSelection(textArea.Document, textArea.Document.OffsetToPosition(offset),
			                                                                                           textArea.Document.OffsetToPosition(endOffset)));
			textArea.Refresh();
		}
	}
}
