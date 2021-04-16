using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Shared._ShouldMigrateToLib.Abstractions;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using Volo.Abp;

namespace Geex.Shared._ShouldMigrateToLib
{
    public class GeexBusinessException : BusinessException
    {
        public GeexBusinessException(GeexExceptionType exceptionType, string details = "", Exception? innerException = default) : base(exceptionType.Value, exceptionType.Message, details, innerException, exceptionType.LogLevel)
        {

        }
    }

    public class GeexUserFriendlyException : UserFriendlyException
    {
        public GeexUserFriendlyException(string message, string code = null, string details = null, Exception innerException = null, LogLevel logLevel = LogLevel.Information) : base(message, code, details, innerException, logLevel)
        {
        }
    }

    /// <summary>
    /// inherit this enumeration to customise your own business exceptions
    /// </summary>
    public abstract class GeexExceptionType : Enumeration<GeexExceptionType, string>
    {
        protected GeexExceptionType([NotNull] string name, string value, string message, LogLevel logLevel = LogLevel.Warning) : base(name, value)
        {
            Message = message;
            LogLevel = logLevel;
        }

        public string Message { get; }
        public LogLevel LogLevel { get; }
    }
}
