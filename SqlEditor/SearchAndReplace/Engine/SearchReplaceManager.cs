// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.Collections.Generic;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using SqlEditor.SearchAndReplace.Engine.SearchStrategy;
using SqlEditor.SearchAndReplace.Engine.TextIterator;

namespace SqlEditor.SearchAndReplace.Engine
{
	public class SearchReplaceManager
	{
		//public static SearchAndReplaceDialog SearchAndReplaceDialog = null;
		
		static Search find = new Search();
		
		static SearchReplaceManager()
		{
			find.TextIteratorBuilder = new ForwardTextIteratorBuilder();
		}
		
		static void SetSearchOptions()
		{
			find.SearchStrategy   = SearchReplaceUtilities.CreateSearchStrategy(SearchOptions.SearchStrategyType);
			find.DocumentIterator = SearchReplaceUtilities.CreateDocumentIterator(SearchOptions.DocumentIteratorType);
		}
		
		public static bool Replace()
		{
			SetSearchOptions();
			if (lastResult != null) {
				//ITextEditorControlProvider provider = WorkbenchSingleton.Workbench.ActiveViewContent as ITextEditorControlProvider;
				//if (provider != null) {
			    var textarea = FrmMdiParent.Instance.GetActiveSqlTextEditor();
			    var activeWorksheet = FrmMdiParent.Instance.GetActiveWorksheet();
			    var fileName = activeWorksheet.WorksheetFile ?? activeWorksheet.Title;
				SelectionManager selectionManager = textarea.ActiveTextAreaControl.TextArea.SelectionManager;
					
				if (selectionManager.SelectionCollection.Count == 1
					&& selectionManager.SelectionCollection[0].Offset == lastResult.Offset
					&& selectionManager.SelectionCollection[0].Length == lastResult.Length
                    && lastResult.FileName == fileName)
				{
					string replacePattern = lastResult.TransformReplacePattern(SearchOptions.ReplacePattern);
						
					textarea.BeginUpdate();
					selectionManager.ClearSelection();
					textarea.Document.Replace(lastResult.Offset, lastResult.Length, replacePattern);
					textarea.ActiveTextAreaControl.Caret.Position = textarea.Document.OffsetToPosition(lastResult.Offset + replacePattern.Length);
					textarea.EndUpdate();
				}
				//}
			}
			return FindNext();
		}
		
		static TextSelection textSelection;
		
		public static bool ReplaceFirstInSelection(int offset, int length)
		{
			SetSearchOptions();
			return FindFirstInSelection(offset, length);
		}

        public static bool ReplaceNextInSelection()
		{
			if (lastResult != null) // && WorkbenchSingleton.Workbench.ActiveWorkbenchWindow != null) 
            {
				//ITextEditorControlProvider provider = WorkbenchSingleton.Workbench.ActiveViewContent as ITextEditorControlProvider;
				//if (provider != null) {
					//TextEditorControl textarea = provider.TextEditorControl;
                var textarea = FrmMdiParent.Instance.GetActiveSqlTextEditor();
                var activeWorksheet = FrmMdiParent.Instance.GetActiveWorksheet();
                var fileName = activeWorksheet.WorksheetFile ?? activeWorksheet.Title;
				SelectionManager selectionManager = textarea.ActiveTextAreaControl.TextArea.SelectionManager;
					
				if (selectionManager.SelectionCollection.Count == 1
					&& selectionManager.SelectionCollection[0].Offset == lastResult.Offset
					&& selectionManager.SelectionCollection[0].Length == lastResult.Length
                    && lastResult.FileName == fileName)
				{
					string replacePattern = lastResult.TransformReplacePattern(SearchOptions.ReplacePattern);
						
					textarea.BeginUpdate();
					selectionManager.ClearSelection();
					textarea.Document.Replace(lastResult.Offset, lastResult.Length, replacePattern);
					textarea.ActiveTextAreaControl.Caret.Position = textarea.Document.OffsetToPosition(lastResult.Offset + replacePattern.Length);
					textarea.EndUpdate();
						
					textSelection.Length -= lastResult.Length - replacePattern.Length;
				}
				//}
			}
			return FindNextInSelection();
		}
		
		public static void MarkAll()
		{
			SetSearchOptions();
			ClearSelection();
			find.Reset();
			if (!find.SearchStrategy.CompilePattern())
				return;
			List<TextEditorControl> textAreas = new List<TextEditorControl>();
			int count;
			for (count = 0;; count++) {
				SearchResultMatch result = SearchReplaceManager.find.FindNext();
				
				if (result == null) {
					break;
				} else {
					MarkResult(textAreas, result);
				}
			}
			find.Reset();
			foreach (TextEditorControl ctl in textAreas) {
				ctl.Refresh();
			}
			ShowMarkDoneMessage(count);
		}
		
		public static int MarkAll(int offset, int length)
		{
			SetSearchOptions();
			find.Reset();
			
			if (!find.SearchStrategy.CompilePattern())
				return 0;
			
			List<TextEditorControl> textAreas = new List<TextEditorControl>();
			int count;
			for (count = 0;; count++) {
				SearchResultMatch result = find.FindNext(offset, length);
				if (result == null) {
					break;
				} else 
                {
					MarkResult(textAreas, result);
                }
			}
			find.Reset();
			foreach (TextEditorControl ctl in textAreas) {
				ctl.Refresh();
			}
		    return count;
		}
		
		static void MarkResult(List<TextEditorControl> textAreas, SearchResultMatch result)
		{
			TextEditorControl textArea = OpenTextArea(result.FileName);
			if (textArea != null) {
				if (!textAreas.Contains(textArea)) {
					textAreas.Add(textArea);
				}
				textArea.ActiveTextAreaControl.Caret.Position = textArea.Document.OffsetToPosition(result.Offset);
				LineSegment segment = textArea.Document.GetLineSegmentForOffset(result.Offset);
				
				int lineNr = segment.LineNumber;
				if (!textArea.Document.BookmarkManager.IsMarked(lineNr)) {
					textArea.Document.BookmarkManager.ToggleMarkAt(new TextLocation(result.Offset - segment.Offset, lineNr));
				}
			}
		}
		
		static void ShowMarkDoneMessage(int count)
		{
			if (count == 0) {
				ShowNotFoundMessage();
			} else {
				//if (monitor != null) monitor.ShowingDialog = true;
                //MessageService.ShowMessage(StringParser.Parse("${res:ICSharpCode.TextEditor.Document.SearchReplaceManager.MarkAllDone}", new string[,]{{ "Count", count.ToString() }}),
                //                           "${res:Global.FinishedCaptionText}");
				//if (monitor != null) monitor.ShowingDialog = false;
			}
		}
		
		static void ShowReplaceDoneMessage(int count)
		{
			if (count == 0) {
				ShowNotFoundMessage();
			} else {
				//if (monitor != null) monitor.ShowingDialog = true;
                //MessageService.ShowMessage(StringParser.Parse("${res:ICSharpCode.TextEditor.Document.SearchReplaceManager.ReplaceAllDone}", new string[,]{{ "Count", count.ToString() }}),
                //                           "${res:Global.FinishedCaptionText}");
				//if (monitor != null) monitor.ShowingDialog = false;
			}
		}
		
		public static int ReplaceAll()
		{
			SetSearchOptions();
			ClearSelection();
			find.Reset();
			if (!find.SearchStrategy.CompilePattern())
				return 0;
			
			List<TextEditorControl> textAreas = new List<TextEditorControl>();
			TextEditorControl textArea = null;
			for (int count = 0;; count++) {
				SearchResultMatch result = SearchReplaceManager.find.FindNext();
				
				if (result == null) {
					if (count != 0) {
						foreach (TextEditorControl ta in textAreas) {
							ta.EndUpdate();
							ta.Refresh();
						}
					}
					find.Reset();
                    return count;
				} else {
					if (textArea == null || textArea.FileName != result.FileName) {
						// we need to open another text area
						textArea = OpenTextArea(result.FileName);
						if (textArea != null) {
							if (!textAreas.Contains(textArea)) {
								textArea.BeginUpdate();
								textArea.ActiveTextAreaControl.TextArea.SelectionManager.SelectionCollection.Clear();
								textAreas.Add(textArea);
							}
						}
					}
					if (textArea != null) {
						string transformedPattern = result.TransformReplacePattern(SearchOptions.ReplacePattern);
						find.Replace(result.Offset, result.Length, transformedPattern);
						if (find.CurrentDocumentInformation.Document == null) {
							textArea.Document.Replace(result.Offset, result.Length, transformedPattern);
						}
					} else {
						count--;
					}
				}
			}
		}
		
		public static int ReplaceAll(int offset, int length)
		{
			SetSearchOptions();
			find.Reset();
			
			if (!find.SearchStrategy.CompilePattern())
				return 0;
			
			for (var count = 0;; count++) {
				SearchResultMatch result = find.FindNext(offset, length);
				if (result == null)
				{
				    return count;
				}
				
				var replacement = result.TransformReplacePattern(SearchOptions.ReplacePattern);
				find.Replace(result.Offset,
				             result.Length,
				             replacement);
				length -= result.Length - replacement.Length;
				
				// HACK - Move the cursor to the correct offset - the caret gets
				// moved before the replace range if we replace a string with a
				// single character. The ProvidedDocInfo.Replace method assumes that
				// the current offset is at the end of the found text which it is not.
				find.CurrentDocumentInformation.CurrentOffset = result.Offset + replacement.Length - 1;
			}
		}
		
		static SearchResultMatch lastResult = null;
		
		public static bool FindNext()
		{
			SetSearchOptions();
			if (find == null ||
			    SearchOptions.FindPattern == null ||
			    SearchOptions.FindPattern.Length == 0) {
				return false;
			}
			
			if (!find.SearchStrategy.CompilePattern()) {
				find.Reset();
				lastResult = null;
				return false;
			}
			
			TextEditorControl textArea = null;
			while (textArea == null) {
				SearchResultMatch result = find.FindNext();
				if (result == null) 
                {
					find.Reset();
					lastResult = null;
					return false;
				} 
                else 
                {
					textArea = OpenTextArea(result.FileName);
					if (textArea != null) 
                    {
						if (lastResult != null  && lastResult.FileName == result.FileName &&
						    textArea.ActiveTextAreaControl.Caret.Offset != lastResult.Offset + lastResult.Length) 
                        {
							find.Reset();
						}
						int startPos = Math.Min(textArea.Document.TextLength, Math.Max(0, result.Offset));
						int endPos   = Math.Min(textArea.Document.TextLength, startPos + result.Length);
						
						SearchReplaceUtilities.SelectText(textArea, startPos, endPos);
						lastResult = result;
                        return true;
                    }
				}
			}
		    return false;
		}
		
		static bool _foundAtLeastOneItem = false;

		public static bool FindFirstInSelection(int offset, int length)
		{
			_foundAtLeastOneItem = false;
			textSelection = null;
			SetSearchOptions();
			
			if (find == null ||
			    SearchOptions.FindPattern == null ||
			    SearchOptions.FindPattern.Length == 0) {
				return false;
			}
			
			if (!find.SearchStrategy.CompilePattern()) {
				find.Reset();
				lastResult = null;
                return false;
			}
			
			textSelection = new TextSelection(offset, length);
			return FindNextInSelection();
		}

		public static bool FindNextInSelection()
		{
			TextEditorControl textArea = null;
			while (textArea == null) {
				SearchResultMatch result = find.FindNext(textSelection.Offset, textSelection.Length);
				if (result == null) {
					if (!_foundAtLeastOneItem) {
						ShowNotFoundMessage();
					}
					find.Reset();
					lastResult = null;
					_foundAtLeastOneItem = false;
					return false;
				} else {
					textArea = OpenTextArea(result.FileName);
					if (textArea != null) {
						_foundAtLeastOneItem = true;
						if (lastResult != null  && lastResult.FileName == result.FileName &&
						    textArea.ActiveTextAreaControl.Caret.Offset != lastResult.Offset + lastResult.Length) {
						}
						int startPos = Math.Min(textArea.Document.TextLength, Math.Max(0, result.Offset));
						int endPos   = Math.Min(textArea.Document.TextLength, startPos + result.Length);
						SearchReplaceUtilities.SelectText(textArea, startPos, endPos);
						lastResult = result;
					}
				}
			}
			return true;
		}
		
		static void ShowNotFoundMessage()
		{
            //if (monitor != null && monitor.IsCancelled)
            //    return;
            //if (monitor != null) monitor.ShowingDialog = true;
            //MessageBox.Show(WorkbenchSingleton.MainForm,
            //                ResourceService.GetString("Dialog.NewProject.SearchReplace.SearchStringNotFound"),
            //                ResourceService.GetString("Dialog.NewProject.SearchReplace.SearchStringNotFound.Title"),
            //                MessageBoxButtons.OK,
            //                MessageBoxIcon.Information);
            //if (monitor != null) monitor.ShowingDialog = false;
		}
		
		static TextEditorControl OpenTextArea(string fileName)
		{
		    return FrmMdiParent.Instance.GetActiveSqlTextEditor();
            //ITextEditorControlProvider textEditorProvider = null;
            //if (fileName != null) {
            //    textEditorProvider = FileService.OpenFile(fileName) as ITextEditorControlProvider;
            //} else {
            //    textEditorProvider = WorkbenchSingleton.Workbench.ActiveViewContent as ITextEditorControlProvider;
            //}
			
            //if (textEditorProvider != null) {
            //    return textEditorProvider.TextEditorControl;
            //}
			//return null;
		}
		
		static void ClearSelection()
		{
            FrmMdiParent.Instance.GetActiveSqlTextEditor().ActiveTextAreaControl.TextArea.SelectionManager.ClearSelection();
            //if (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow != null) {
            //    ITextEditorControlProvider provider = WorkbenchSingleton.Workbench.ActiveViewContent as ITextEditorControlProvider;
            //    if (provider != null) {
            //        provider.TextEditorControl.ActiveTextAreaControl.TextArea.SelectionManager.ClearSelection();
            //    }
            //}
		}
	}
}
