namespace SqlEditor
{
    public class ProgressInformation
    {
        public int PercentageProgress { get; set; }
        public string Action { get; set; }

        public ProgressInformation()
        {
            PercentageProgress = 0;
            Action = string.Empty;
        }

        public ProgressInformation(int percentageProgress, string action)
        {
            PercentageProgress = percentageProgress;
            Action = action;
        }
    }
}
