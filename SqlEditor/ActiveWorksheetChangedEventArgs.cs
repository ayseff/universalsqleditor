using System;

namespace SqlEditor
{
    public class ActiveWorksheetChangedEventArgs : EventArgs
    {
        public FrmSqlWorksheet Worksheet { get; private set; }

        public ActiveWorksheetChangedEventArgs(FrmSqlWorksheet worksheet)
        {
            Worksheet = worksheet;
        }
    }
}