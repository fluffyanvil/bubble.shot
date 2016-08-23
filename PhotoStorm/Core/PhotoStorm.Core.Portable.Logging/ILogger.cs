using System;

namespace PhotoStorm.Core.Portable.Logging
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message, Exception exception);
    }
}