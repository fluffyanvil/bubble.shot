using System;
using MetroLog;

namespace PhotoStorm.Core.Portable.Logging
{
    public class Logger : ILogger
    {
        private readonly MetroLog.ILogger _logger = LogManagerFactory.DefaultLogManager.GetLogger(typeof(Logger));
        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(message, exception);
        }
    }
}