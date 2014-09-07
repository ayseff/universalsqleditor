// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision$</version>
// </file>

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using SqlEditor.SearchAndReplace.Engine.SearchStrategy;

namespace SqlEditor.SearchAndReplace.Engine.DocumentIterator
{
	public class AllOpenDocumentIterator : IDocumentIterator
	{
		int  startIndex = -1;
		int  curIndex   = -1;
		bool resetted   = true;
		
		public AllOpenDocumentIterator()
		{
			Reset();
		}
		
		public string CurrentFileName {
			get
			{
			    var frmSqlWorksheet = FrmMdiParent.Instance.GetActiveWorksheet();
                if (frmSqlWorksheet != null)
                {
                    return frmSqlWorksheet.WorksheetFile ?? frmSqlWorksheet.Title;
                }
				return null;
			}
		}
		
		public ProvidedDocumentInformation Current {
			get {
                TextEditorControl textEditor = FrmMdiParent.Instance.GetActiveSqlTextEditor();
                if (textEditor != null)
                {
                    IDocument document = textEditor.Document;
                    return new ProvidedDocumentInformation(document,
                        CurrentFileName,
                        textEditor.ActiveTextAreaControl);
                }
				return null;
			}
		}
		
		void GetCurIndex()
		{
		    var activeWorksheet = FrmMdiParent.Instance.GetActiveWorksheet();
		    var worksheets = FrmMdiParent.Instance.GetAllWorksheets();
		    int viewCount = worksheets.Length;
			if (curIndex == -1 || curIndex >= viewCount) {
				for (int i = 0; i < viewCount; ++i) {
                    if (activeWorksheet == worksheets[i])
                    {
						curIndex = i;
						return;
					}
				}
				curIndex = -1;
			}
		}
		
		public bool MoveForward() 
		{
			GetCurIndex();
			if (curIndex < 0) {
				return false;
			}
			
			if (resetted) {
				resetted = false;
				return true;
			}

            curIndex = (curIndex + 1) % FrmMdiParent.Instance.GetAllWorksheets().Length;
			if (curIndex == startIndex) {
				return false;
			}
			return true;
		}
		
		public bool MoveBackward()
		{
			GetCurIndex();
			if (curIndex < 0) {
				return false;
			}
			if (resetted) {
				resetted = false;
				return true;
			}
			
			if (curIndex == 0) {
                curIndex = FrmMdiParent.Instance.GetAllWorksheets().Length - 1;
			}
			
			if (curIndex > 0) {
				--curIndex;
				return true;
			}
			return false;
		}
		
		public void Reset() 
		{
			curIndex = -1;
			GetCurIndex();
			startIndex = curIndex;
			resetted = true;
		}
	}
}
