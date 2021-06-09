using Geex.Common.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Geex.Core.Exceptions
{
    public class ExceptionType : GeexExceptionType
    {
        public static ExceptionType UserAlreadyExists = new ExceptionType(nameof(UserAlreadyExists), nameof(UserAlreadyExists), nameof(UserAlreadyExists), LogLevel.Information);
        public static ExceptionType UserFriendly { get; } = new(nameof(UserFriendly), nameof(UserFriendly), nameof(UserFriendly), LogLevel.Information);
        public static ExceptionType NotFound { get; } = new(nameof(NotFound), nameof(NotFound), nameof(NotFound), LogLevel.Warning);
        public ExceptionType([NotNull] string name, [NotNull] string value, [NotNull] string message, LogLevel logLevel = LogLevel.Warning) : base(name, value, message, logLevel)
        {
        }
    }
}
