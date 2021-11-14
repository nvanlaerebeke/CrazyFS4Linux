using System;
using Serilog;
using Serilog.Events;

namespace CrazyFS.Log
{
    public interface ILogProvider
    {
        ILogger Get();
    }

    public class LogProvider : ILogProvider
    {
        private static ILogger _logger;
        private static readonly object _lock = new();
        private static LogEventLevel _logLevel = LogEventLevel.Debug;

        public static LogEventLevel LogLevel
        {
            get => _logLevel;
            set
            {
                _logLevel = value;
                _logger = null;
            }
        }

        public ILogger Get()
        {
            if (_logger != null) return _logger;
            
            lock (_lock)
            {
                if (_logger != null) return _logger;

                var conf = new LoggerConfiguration().WriteTo.Console();
                switch (LogLevel)
                {
                    case LogEventLevel.Debug:
                        conf.MinimumLevel.Debug();
                        break;
                    case LogEventLevel.Error:
                        conf.MinimumLevel.Error();
                        break;
                    case LogEventLevel.Fatal:
                        conf.MinimumLevel.Fatal();
                        break;
                    case LogEventLevel.Information:
                        conf.MinimumLevel.Information();
                        break;
                    case LogEventLevel.Verbose:
                        conf.MinimumLevel.Verbose();
                        break;
                    case LogEventLevel.Warning:
                        conf.MinimumLevel.Warning();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _logger = conf.CreateLogger();
            }

            return _logger;
        }
    }
}