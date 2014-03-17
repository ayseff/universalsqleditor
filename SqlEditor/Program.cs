using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using HtmlAgilityPack;
using log4net;
using log4net.Config;
using Utilities.Forms.Dialogs;
using HtmlDocument = System.Windows.Forms.HtmlDocument;

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
                throw;
            }
        }
        
    }
}
