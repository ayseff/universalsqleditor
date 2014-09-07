using System;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace SqlEditor.SearchAndReplace.Engine.SearchStrategy
{
    public class SearchResultMatch
    {
        ProvidedDocumentInformation providedDocumentInformation;
        int offset;
        int length;

        public string FileName
        {
            get
            {
                return providedDocumentInformation.FileName;
            }
        }

        public ProvidedDocumentInformation ProvidedDocumentInformation
        {
            set
            {
                providedDocumentInformation = value;
            }
        }

        public int Offset
        {
            get
            {
                return offset;
            }
        }

        public int Length
        {
            get
            {
                return length;
            }
        }

        public virtual string TransformReplacePattern(string pattern)
        {
            return pattern;
        }

        public IDocument CreateDocument()
        {
            return providedDocumentInformation.CreateDocument();
        }

        public SearchResultMatch(int offset, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            this.offset = offset;
            this.length = length;
        }

        public virtual TextLocation GetStartPosition(IDocument document)
        {
            return document.OffsetToPosition(Math.Min(Offset, document.TextLength));
        }

        public virtual TextLocation GetEndPosition(IDocument document)
        {
            return document.OffsetToPosition(Math.Min(Offset + Length, document.TextLength));
        }

        /// <summary>
        /// Gets a special text to display, or null to display the line's content.
        /// </summary>
        public virtual string DisplayText
        {
            get
            {
                return null;
            }
        }

        public override string ToString()
        {
            return String.Format("[{3}: FileName={0}, Offset={1}, Length={2}]",
                FileName, Offset, Length,
                GetType().Name);
        }
    }
}