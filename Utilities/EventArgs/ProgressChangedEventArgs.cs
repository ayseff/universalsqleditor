#region



#endregion

using System.Globalization;

namespace Utilities.EventArgs
{
    public class ProgressChangedEventArgs : System.EventArgs
    {
        private readonly int _progressPercentage;

        public ProgressChangedEventArgs(int progressPercentage)
        {
            _progressPercentage = progressPercentage;
        }

        public int ProgressPercentage
        {
            get { return _progressPercentage; }
        }

        public override string ToString()
        {
            return ProgressPercentage.ToString(CultureInfo.InvariantCulture);
        }
    }
}