using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using log4net;

namespace Utilities.Email
{
    public class EmailSender
    {
        private static ILog _log = LogManager.GetLogger(typeof (EmailSender));
        protected static ILog Log
        {
            get
            {
                if (_log == null)
                {
                    _log = LogManager.GetLogger(typeof(EmailSender));
                }
                return _log;
            }
        }

        public static bool Send(MailMessage message,  string userName, string password)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message", "Message is null.");
            }

            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat("Sending email to {0} ...", string.Join(",", message.To.Select(a => a.Address)));
            }

            try
            {
                var domainName = GetDomainName(userName);
                if (domainName == "yahoo.com")
                {
                    return SendUsingYahoo(message, userName, password);
                }
                else if (domainName == "gmail.com")
                {
                    return SendUsingGoogle(message, userName, password);
                }
                else
                {
                    throw new InvalidOperationException("Cannot send email from addresses other than gmail or yahoo.");
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error sending email to {0}.", message.To);
                Log.Error(ex.Message, ex);
            }
            return false;
        }

        private static bool SendUsingGoogle(MailMessage message, string userName, string password)
        {
            try
            {
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(userName, password)
                };
                smtp.Send(message);

                if (Log.IsDebugEnabled)
                {
                    Log.Debug("Email sent successfully.");
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error sending email to {0}.", string.Join(",", message.To.Select(a => a.Address)));
                Log.Error(ex.Message, ex);
            }
            return false;
        }

        /// <summary>
        /// Send an electronic message using the Collaboration Data Objects (CDO).
        /// </summary>
        /// <remarks>http://support.microsoft.com/kb/310212</remarks>
        private static bool SendUsingYahoo(MailMessage m, string userName, string password)
        {
            try
            {
                CDO.Message message = new CDO.Message();
                CDO.IConfiguration configuration = message.Configuration;
                ADODB.Fields fields = configuration.Fields;

                // Set configuration.
                // sendusing:               cdoSendUsingPort, value 2, for sending the message using the network.
                // smtpauthenticate:     Specifies the mechanism used when authenticating to an SMTP service over the network.
                //                                  Possible values are:
                //                                  - cdoAnonymous, value 0. Do not authenticate.
                //                                  - cdoBasic, value 1. Use basic clear-text authentication. (Hint: This requires the use of "sendusername" and "sendpassword" fields)
                //                                  - cdoNTLM, value 2. The current process security context is used to authenticate with the service.

                ADODB.Field field = fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"];
                field.Value = "smtp.mail.yahoo.com";

                field = fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"];
                field.Value = 465;

                field = fields["http://schemas.microsoft.com/cdo/configuration/sendusing"];
                field.Value = CDO.CdoSendUsing.cdoSendUsingPort;

                field = fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"];
                field.Value = CDO.CdoProtocolsAuthentication.cdoBasic;

                field = fields["http://schemas.microsoft.com/cdo/configuration/sendusername"];
                field.Value = userName;

                field = fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"];
                field.Value = password;

                field = fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"];
                field.Value = "true";

                fields.Update();

                message.From = userName;
                message.To = string.Join(",", m.To.Select(a => a.Address));
                message.Subject = m.Subject;
                if (m.IsBodyHtml)
                {
                    message.HTMLBody = m.Body;
                }
                else
                {
                    message.TextBody = m.Body;
                }
                
                message.Send();
                if (Log.IsDebugEnabled)
                {
                    Log.Debug("Email sent successfully.");
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error sending email to {0}.", string.Join(",", m.To.Select(a => a.Address)));
                Log.Error(ex.Message, ex);
            }
            return false;
        }

        private static string GetDomainName(string emailAddress)
        {
            int atIndex = emailAddress.IndexOf('@');
            if (atIndex == -1)
            {
                throw new ArgumentException("Not a valid email address", "emailAddress");
            }
            if (emailAddress.IndexOf('<') > -1 && emailAddress.IndexOf('>') > -1)
            {
                return emailAddress.Substring(atIndex + 1, emailAddress.IndexOf('>') - atIndex).ToLower();
            }
            else
            {
                return emailAddress.Substring(atIndex + 1).ToLower();
            }
        }
    }
}
