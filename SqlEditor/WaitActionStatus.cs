using System.Windows.Forms;

namespace SqlEditor
{
    public class WaitActionStatus : ActionStatus
    {
        private readonly Cursor _previousCursor;

        public WaitActionStatus(string actionName)
            : base(actionName)
        {
            _previousCursor = Cursor.Current;            
            Cursor.Current = Cursors.WaitCursor;
            //Application.DoEvents();
        }

        public WaitActionStatus()
            : this(null)
        { }

        public override void Dispose()
        {            
            Cursor.Current = _previousCursor;
            base.Dispose();
        }
    }
}
