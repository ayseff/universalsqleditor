namespace SqlEditor
{
    public class CloseAction
    {
        private static CloseAction _instance;
        private OnCloseSaveAction _onCloseSaveAction = OnCloseSaveAction.Save;

        public OnCloseSaveAction OnCloseSaveAction
        {
            get { return _onCloseSaveAction; }
            set { _onCloseSaveAction = value; }
        }

        public static CloseAction Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CloseAction();
                }
                return _instance;
            }
            
        }
    }
}