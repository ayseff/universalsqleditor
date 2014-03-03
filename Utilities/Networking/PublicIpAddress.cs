using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using Utilities.Text;
using log4net;

namespace Utilities.Networking
{
    /// <summary>
    /// Class to represent a public facing IP address of a machine.
    /// </summary>
    public class PublicIpAddress
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Regex _validIpRegex =
            new Regex(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Determines whether an IP address is valid or not.
        /// </summary>
        /// <param name="address">IP address to validate.</param>
        /// <returns><code>true</code> if the IP address is valid of <code>false</code> if it is not.</returns>
        /// <exception cref="ArgumentNullException">If the address passed in is <code>null</code>.</exception>
        public static bool IsValidIpAddress(string address)
        {
            if (address == null) throw new ArgumentNullException("address");

            return _validIpRegex.IsMatch(address);
        }

        /// <summary>
        /// Retrieves public facing IP address of the machine running the application.
        /// </summary>
        /// <param name="retryCount">Number of retries (default is 3).</param>
        /// <param name="delay">Delay between requests in seconds.</param>
        /// <returns>Public IP address.</returns>
        public static string GetPublicIpAddress(int retryCount = 3, int delay = 1)
        {
            if (delay < 0)
            {
                throw new ArgumentException("Delay must be a positive number.");
            }

            _log.Debug("Getting public IP address ...");
            Exception exception = null;

            var list = new List<Func<string>>
                           {
                               GetPublicIpAddressFromDynDns,
                               GetPublicIpAddressFromFreeGeoIp,
                               GetPublicIpAddressFromGetIp
                           };


            for (var i = 0; i < retryCount; i++)
            {
                foreach (var func in list)
                {
                    try
                    {
                        var ipAddress = func.Invoke();
                        if (!ipAddress.IsNullEmptyOrWhitespace() && IsValidIpAddress(ipAddress))
                        {
                            return ipAddress;
                        }
                        else
                        {
                            _log.DebugFormat("Public IP address obtained {0} is not valid.", ipAddress ?? "NULL");
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.ErrorFormat("Error getting IP address using {0}.", func);
                        _log.Error(ex.Message);
                        exception = ex;
                    }
                }
                if (delay > 0)
                {
                    Thread.Sleep(delay*1000);
                }
            }
            throw exception ?? new Exception("Could not obtain IP address.");
        }

        private static string GetPublicIpAddressFromDynDns()
        {
            const string webAddress = "http://checkip.dyndns.org/";
            const string startString = "Current IP Address:";
            const string endString = "</body>";

            _log.Debug("Getting public IP address from DynDns ...");
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

            //Search for the IP in the html
            _log.Debug("Parsing response ...");
            var first = result.IndexOf(startString, StringComparison.Ordinal);
            if (first == -1)
            {
                const string message = "Error getting current IP address. Could not find start tag: " + startString;
                _log.ErrorFormat(message);
                _log.Error(result);
                throw new Exception(message);
            }
            first += startString.Length;

            var last = result.LastIndexOf(endString, StringComparison.Ordinal);
            if (last == -1)
            {
                const string message = "Error getting current IP address. Could not find end tag: " + endString;
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

        
        private static string GetPublicIpAddressFromFreeGeoIp()
        {
            const string url = "http://freegeoip.net/xml/";
            var wc = new WebClient {Proxy = null};
            XmlDocument doc;
            using (var ms = new MemoryStream(wc.DownloadData(url)))
            {
                doc = new XmlDocument();
                ms.Position = 0;
                doc.Load(ms);
            }
            var node = doc.SelectSingleNode("//Response/Ip");
            if (node == null)
            {
                throw new Exception("Could not find Ip node.");
            }
            return node.InnerText.Trim().ToUpper();
        }

        private static string GetPublicIpAddressFromGetIp()
        {
            const string url = "http://www.getip.com/";
            string result;
            var request = WebRequest.Create(url);
            _log.DebugFormat("Sending request to {0} ...", url);
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

            //Search for the IP in the html
            _log.Debug("Parsing response ...");
            const string startString = "<h1 id=\"current-ip\">Current IP: <em>";
            var first = result.IndexOf(startString, StringComparison.Ordinal);
            if (first == -1)
            {
                const string message = "Error getting current IP address. Could not find start tag: " + startString;
                _log.ErrorFormat(message);
                _log.Error(result);
                throw new Exception(message);
            }
            first += startString.Length;

            const string endString = "</em></h1>";
            var last = result.LastIndexOf(endString, StringComparison.Ordinal);
            if (last == -1)
            {
                const string message = "Error getting current IP address. Could not find end tag: " + endString;
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
