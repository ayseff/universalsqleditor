using System;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using log4net.Config;

namespace SqlEditor
{
    static class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                XmlConfigurator.Configure();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(FrmMdiParent.Instance);
            }
            catch (Exception ex)
            {
                _log.Error("UNHANDLED exception occurred.");
                _log.Error(ex.Message, ex);
                Utilities.Logging.Log4NetHelper.FlushAllAppenders();
                throw;
            }
        }
        
    }
}
