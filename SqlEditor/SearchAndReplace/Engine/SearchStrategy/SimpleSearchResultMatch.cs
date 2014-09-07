using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace SqlEditor.SearchAndReplace.Engine.SearchStrategy
{
    public class SimpleSearchResultMatch : SearchResultMatch
    {
        TextLocation position;

        public override TextLocation GetStartPosition(IDocument doc)
        {
            return position;
        }

        public override TextLocation GetEndPosition(IDocument doc)
        {
            return position;
        }

        string displayText;

        public override string DisplayText
        {
            get
            {
                return displayText;
            }
        }

        public SimpleSearchResultMatch(string displayText, TextLocation position)
            : base(0, 0)
        {
            this.position = position;
            this.displayText = displayText;
        }
    }
}