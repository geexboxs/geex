using System.Collections.Generic;

namespace Geex.Core.Users.Inputs
{
    public class AssignRoleInput
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}