using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Utilities.Text;
using log4net;

namespace Utilities.CLI
{
    public class CliHelper
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static Dictionary<string, List<string>> ProcessArguments(string[] args, string optString)
        {
            if (args == null) throw new ArgumentNullException("args");
            if (optString.IsNullEmptyOrWhitespace()) throw new ArgumentNullException("optString");

            _log.Debug("Processing arguments ...");
            var arguments = new Dictionary<string, List<string>>();
            var g = new Getopt("CliHelper", args, optString);
            int c;
            while ((c = g.getopt()) != -1)
            {
                if (_log.IsDebugEnabled)
                {
                    _log.Debug(string.Format("Found argument {0} with value {1}.", ((char) c), g.Optarg));
                }

                List<string> list;
                var argumentString = ((char) c).ToString(CultureInfo.InvariantCulture);
                if (!arguments.TryGetValue(argumentString, out list))
                {
                    list = new List<string>();
                    arguments.Add(argumentString, list);
                }
                list.Add(g.Optarg);
            }
            return arguments;
        }
    }
}
