using System;

namespace Geex.Shared._ShouldMigrateToLib
{
    public class UserFriendlyException : Exception
    {
        /// <summary>
        /// Additional information about the exception.
        /// </summary>
        public string Details { get; private set; }
        public UserFriendlyException(string message, string detailTemplate, params object[] formatParams) : base(message)
        {
            this.Details = string.Format(detailTemplate, formatParams);
        }
    }
}