using System;
using System.IO;
using System.Net;
using System.Reflection;
using log4net;

namespace Utilities.Networking
{
    public class NetworkingHelper
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Retrieves public facing IP address of the machine running the application.
        /// </summary>
        /// <param name="webAddress">Web address from where to get the IP address (default: http://checkip.dyndns.org)</param>
        /// <param name="startString">String that is immediately before the IP address (default: 'Current IP Address:')</param>
        /// <param name="endString">String that is immediately after the IP address (default: &lt;/body&gt; ]])</param>
        /// <returns></returns>
        public static string GetPublicIpAddress(string webAddress = "http://checkip.dyndns.org/", string startString = "Current IP Address:", string endString = "</body>")
        {
            if (webAddress == null) throw new ArgumentNullException("webAddress");
            if (startString == null) throw new ArgumentNullException("startString");
            if (endString == null) throw new ArgumentNullException("endString");

            _log.Debug("Getting public IP address ...");
            string result;
            var request = WebRequest.Create(webAddress);
            _log.DebugFormat("Sending request to {0} ...", webAddress);
            using (var response = request.GetResponse())
            {
                var responseStream = response.GetResponseStream();
                if (responseStream == null)
                {
                    const string message = "Response stream is null";
                    _log.Error(message);
                    throw new Exception(message);
                }
                using (var stream = new StreamReader(responseStream))
                {
                    _log.Debug("Reading response ...");
                    result = stream.ReadToEnd();
                }
            }

            //Search for the ip in the html
            _log.Debug("Parsing response ...");
            var first = result.IndexOf(startString, StringComparison.Ordinal);
            if (first == -1)
            {
                string message = "Error getting current IP address. Could not find start tag: " + startString;
                _log.ErrorFormat(message);
                _log.Error(result);
                throw new Exception(message);
            }
            first += startString.Length;

            var last = result.LastIndexOf(endString, StringComparison.Ordinal);
            if (last == -1)
            {
                string message = "Error getting current IP address. Could not find end tag: " + endString;
                _log.ErrorFormat(message);
                _log.Error(result);
                throw new Exception(message);
            }
            else if (first >= last)
            {
                const string message = "Error getting current IP address. Start tag is after the end tag";
                _log.ErrorFormat(message);
                _log.Error(result);
                throw new Exception(message);
            }
            result = result.Substring(first, last - first).Trim().ToUpper();
            _log.DebugFormat("Obtained IP address {0}.", result);
            return result;
        }
    }
}
