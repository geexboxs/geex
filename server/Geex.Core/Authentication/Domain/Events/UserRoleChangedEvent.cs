using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Geex.Core.Authentication.Domain.Events
{
    public record UserRoleChangedEvent(string UserId, List<string> Roles):INotification
    {
        public string UserId { get; init; } = UserId;
        public List<string> Roles { get; init; } = Roles;
    }
}
