using System;
using System.IO;
using System.Reflection;
using log4net;

namespace Utilities.InfragisticsUtilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class InfragisticsHelpers
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Load Infragistics style from embedded resource.
        /// </summary>
        /// <param name="resourceName">Embedded resource name.</param>
        /// <param name="assembly">Assembly where the resources exist.</param>
        /// <returns><code>true</code> if the operation is successful or <code>false</code> otherwise.</returns>
        public static bool LoadStyleFromResource(string resourceName, Assembly assembly)
        {
            if (string.IsNullOrWhiteSpace(resourceName) || string.IsNullOrEmpty(resourceName)) throw new ArgumentNullException("resourceName");

            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat("Loading style form file {0} ...", resourceName);
            }
            var s = assembly.GetManifestResourceStream(resourceName);
            if (s != null)
            {
                Infragistics.Win.AppStyling.StyleManager.Load(s);
                return true;
            }
            else
            {
                _log.ErrorFormat("Could not load resource {0}. Stream is null.", resourceName);
                return false;
            }
        }

        /// <summary>
        /// Load Infragistics style from byte array.
        /// </summary>
        /// <param name="byteArray">Byte array containing the style.</param>
        public static void LoadStyle(byte[] byteArray)
        {
            if (byteArray == null) throw new ArgumentNullException("byteArray");
            _log.Debug("Loading style ....");
            using (var memStream = new MemoryStream(byteArray))
            {
                Infragistics.Win.AppStyling.StyleManager.Load(memStream);
                _log.Debug("Style loaded.");
            }
        }
    }
}
