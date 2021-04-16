using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

namespace Geex.Shared
{
    public class GeexExceptionType : Geex.Shared._ShouldMigrateToLib.GeexExceptionType
    {
        public static GeexExceptionType UserAlreadyExists = new GeexExceptionType(nameof(UserAlreadyExists), nameof(UserAlreadyExists), nameof(UserAlreadyExists), LogLevel.Information);

        public GeexExceptionType([NotNull] string name, [NotNull] string value, [NotNull] string message, LogLevel logLevel = LogLevel.Warning) : base(name, value, message, logLevel)
        {
        }
    }
}
