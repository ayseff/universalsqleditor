using System;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace Utilities.Logging
{
    public class Log4NetHelper
    {
        /// <summary>
        /// Configures log4net logger based on default configuration in app.config.
        /// </summary>
        public static void ConfigureLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Flushes all appenders.
        /// </summary>
        public static void FlushAllAppenders()
        {
            foreach (var appender in LogManager.GetRepository().GetAppenders())
            {
                var buffer = appender as BufferingAppenderSkeleton;
                if (buffer != null)
                {
                    buffer.Flush();
                }
            }
        }

        /// <summary>
        /// Gets the log level for logger.
        /// </summary>
        /// <param name="logger">Logger for which to get the log level.</param>
        /// <returns>Logger log level.</returns>
        public static Level GetLoggerLogLevel(ILog logger)
        {
            return ((log4net.Repository.Hierarchy.Logger)logger.Logger).Level;
        }

        /// <summary>
        /// Sets logger log level.
        /// </summary>
        /// <param name="logger">Logger for which to set the log level.</param>
        /// <param name="logLevel">log level.</param>
        public static void SetLoggerLogLevel(ILog logger, Level logLevel)
        {
            ((log4net.Repository.Hierarchy.Logger)logger.Logger).Level = logLevel;
        }

        /// <summary>
        /// Gets log level for the entire repository.
        /// </summary>
        /// <returns>Log level of the repository.</returns>
        public static Level GetLogLevel()
        {
            return LogManager.GetRepository().Threshold;
        }

        /// <summary>
        /// Sets log level for the entire repository.
        /// </summary>
        /// <param name="logLevel">Log level of the repository.</param>
        public static void SetLogLevel(Level logLevel)
        {
            LogManager.GetRepository().Threshold = logLevel;
        }

        public static string GetFileName(ILog logger)
        {
            if (logger == null || logger.Logger == null) throw new ArgumentNullException("logger");
            return LogManager.GetRepository().GetAppenders().OfType<FileAppender>().Where(p => p.Name == logger.Logger.Name).Select(a => a.File).FirstOrDefault();
        }

        public static void SetFileName(ILog logger, string fileFullPath)
        {
            if (logger == null || logger.Logger == null) throw new ArgumentNullException("logger");
            var rootRep = LogManager.GetRepository();
            foreach (var iApp in rootRep.GetAppenders())
            {
                if (iApp.Name == logger.Logger.Name && iApp is FileAppender)
                {
                    FileAppender fApp = (FileAppender)iApp;
                    fApp.File = fileFullPath;
                    fApp.ActivateOptions();
                }
            }
        }
    }
}
