using System.Windows.Forms;

namespace SqlEditor.DatabaseExplorer
{
    public class DatabaseExplorerImageList
    {
        private static DatabaseExplorerImageList _instance;
        public ImageList ImageList { get; set; }

        public static DatabaseExplorerImageList Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseExplorerImageList();
                }
                return _instance;
            }
        }
    }
}