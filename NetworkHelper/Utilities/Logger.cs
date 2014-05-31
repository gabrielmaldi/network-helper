using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;

namespace NetworkHelper.Utilities
{
    public class Logger
    {
        private static readonly Lazy<Logger> _Instance = new Lazy<Logger>(() => new Logger());

        private readonly ILog _logger;
        private readonly EventAppender _eventAppender;

        public event EventHandler<LoggedEventArgs> Logged
        {
            add
            {
                if (_eventAppender != null)
                {
                    _eventAppender.Logged += value;
                }
            }
            remove
            {
                if (_eventAppender != null)
                {
                    _eventAppender.Logged -= value;
                }
            }
        }

        private Logger()
        {
            XmlConfigurator.Configure();
            _logger = LogManager.GetLogger("Main");
            _eventAppender = LogManager.GetRepository().GetAppenders().OfType<EventAppender>().SingleOrDefault();
        }

        public static Logger Instance
        {
            get { return _Instance.Value; }
        }

        public void Log(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    _logger.Debug(message);
                    break;
                case LogLevel.Info:
                    _logger.Info(message);
                    break;
                case LogLevel.Warning:
                    _logger.Warn(message);
                    break;
                case LogLevel.Error:
                    _logger.Error(message);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(message);
                    break;
                default:
                    throw new InvalidEnumArgumentException("logLevel", (int)logLevel, typeof(LogLevel));
            }
        }

        public void Log(LogLevel logLevel, string format, params object[] args)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    _logger.DebugFormat(CultureInfo.InvariantCulture, format, args);
                    break;
                case LogLevel.Info:
                    _logger.InfoFormat(CultureInfo.InvariantCulture, format, args);
                    break;
                case LogLevel.Warning:
                    _logger.WarnFormat(CultureInfo.InvariantCulture, format, args);
                    break;
                case LogLevel.Error:
                    _logger.ErrorFormat(CultureInfo.InvariantCulture, format, args);
                    break;
                case LogLevel.Fatal:
                    _logger.FatalFormat(CultureInfo.InvariantCulture, format, args);
                    break;
                default:
                    throw new InvalidEnumArgumentException("logLevel", (int)logLevel, typeof(LogLevel));
            }
        }

        public void Log(LogLevel logLevel, string message, Exception exception)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    _logger.Debug(message, exception);
                    break;
                case LogLevel.Info:
                    _logger.Info(message, exception);
                    break;
                case LogLevel.Warning:
                    _logger.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    _logger.Error(message, exception);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(message, exception);
                    break;
                default:
                    throw new InvalidEnumArgumentException("logLevel", (int)logLevel, typeof(LogLevel));
            }
        }

        private class EventAppender : AppenderSkeleton
        {
            public event EventHandler<LoggedEventArgs> Logged;

            protected override void Append(LoggingEvent loggingEvent)
            {
                var handler = Logged;
                if (handler != null)
                {
                    handler(this, new LoggedEventArgs(LevelToLogLevel(loggingEvent.Level), RenderLoggingEvent(loggingEvent)));
                }
            }

            private static LogLevel LevelToLogLevel(Level level)
            {
                LogLevel result;

                if (level == Level.Debug)
                {
                    result = LogLevel.Debug;
                }
                else if (level == Level.Info)
                {
                    result = LogLevel.Info;
                }
                else if (level == Level.Warn)
                {
                    result = LogLevel.Warning;
                }
                else if (level == Level.Error)
                {
                    result = LogLevel.Error;
                }
                else if (level == Level.Fatal)
                {
                    result = LogLevel.Fatal;
                }
                else
                {
                    throw new InvalidEnumArgumentException("level", level.Value, typeof(Level));
                }

                return result;
            }
        }
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    public class LoggedEventArgs : EventArgs
    {
        public LogLevel LogLevel { get; private set; }
        public string Message { get; private set; }

        public LoggedEventArgs(LogLevel logLevel, string message)
        {
            LogLevel = logLevel;
            Message = message;
        }
    }
}