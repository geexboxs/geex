using System;

using Microsoft.Extensions.Logging;

namespace Geex.Shared._ShouldMigrateToLib
{
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException(string message) : base(message)
        {
        }
    }

    public class GeexException : Exception
    {
        public EventId EventId { get; }

        public GeexException(string message, EventId eventId) : base(message)
        {
            EventId = eventId;
        }
    }
}