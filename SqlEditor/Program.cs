using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Infragistics.Win.AppStyling;
using SqlEditor.Properties;
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
                //LoadDefaultStyle();
                //Application.Run(FrmSplash.Instance);
                Application.Run(FrmMdiParent.Instance);
            }
            catch (Exception ex)
            {
                _log.Error("UNHANDLED exception occurred.");
                _log.Error(ex.Message, ex);
                throw;
            }
        }

        private static void LoadDefaultStyle()
        {
            var styleName = string.IsNullOrEmpty(Settings.Default.FrmMdiParent_Style) ? "Office 2010 Blue" : Settings.Default.FrmMdiParent_Style;
            _log.DebugFormat("Loading style {0} ...", styleName);
            try
            {
                var styleData = GetStyleData(styleName);
                using (var memStream = new MemoryStream(styleData))
                {
                    if (memStream.Length > 0)
                    {
                        StyleManager.Load(memStream);
                        _log.DebugFormat("Style {0} loaded successfully.", styleName);
                    }
                    else
                    {
                        _log.ErrorFormat("Style data is empty.");
                    }
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error loading style {0}.", styleName);
                _log.Error(ex.Message, ex);
            }
        }

        private static byte[] GetStyleData(string buttonName)
        {
            _log.DebugFormat("Getting style data for {0} ....", buttonName);
            switch (buttonName)
            {
                case "Office 2007 Blue":
                    return Resources.Office2007Blue;
                case "Office 2007 Silver":
                    return Resources.Office2007Silver;
                case "Office 2007 Black":
                    return Resources.Office2007Black;
                case "Office 2010 Blue":
                    return Resources.Office2010Blue;
                case "Office 2010 Silver":
                    return Resources.Office2010Silver;
                case "Office 2010 Black":
                    return Resources.Office2010Black;
                case "Metro":
                    return Resources.Metro;
                case "Aero":
                    return Resources.Aero;
                case "Windows7":
                    return Resources.Windows7;
                default:
                    throw new Exception("Could not find style data for " + buttonName);
            }
        }
    }
}
