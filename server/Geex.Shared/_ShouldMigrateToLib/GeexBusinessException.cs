using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Shared._ShouldMigrateToLib.Abstractions;

using Microsoft.Extensions.Logging;


namespace Geex.Shared._ShouldMigrateToLib
{
    public class GeexBusinessException : Exception
    {
        public LogLevel LogLevel { get; }
        public string LogMessage { get; }
        public GeexBusinessException(GeexExceptionType exceptionType, Exception? innerException = default, string? message = default) : base(message, innerException)
        {
            ExceptionName = exceptionType.Name;
            ExceptionCode = exceptionType.Value;
            LogMessage = message ?? exceptionType.DefaultLogMessage;
            LogLevel = exceptionType.LogLevel;
        }

        public string ExceptionCode { get; set; }

        public string ExceptionName { get; set; }
    }

    public class GeexUserFriendlyException : GeexBusinessException
    {
        public GeexUserFriendlyException(string message, Exception? innerException = default) : base(GeexExceptionType.UserFriendly, innerException, message)
        {
        }
    }

    /// <summary>
    /// inherit this enumeration to customise your own business exceptions
    /// </summary>
    public class GeexExceptionType : Enumeration<GeexExceptionType, string>
    {
        protected GeexExceptionType(string name, string code, string defaultLogMessage, LogLevel logLevel = LogLevel.Warning) : base(name, code)
        {
            DefaultLogMessage = defaultLogMessage;
            LogLevel = logLevel;
        }

        public string DefaultLogMessage { get; }
        public LogLevel LogLevel { get; }

        public static GeexExceptionType UserFriendly { get; } = new(nameof(UserFriendly), nameof(UserFriendly), nameof(UserFriendly), LogLevel.Information);
        public static GeexExceptionType NotFound { get; } = new(nameof(NotFound), nameof(NotFound), nameof(NotFound), LogLevel.Warning);
    }
}
