namespace Geex.Core.Authorization.Events
{
    public record PermissionChangedEvent(string SubId, AppPermission[] Permissions)
    {
        public string SubId { get; init; } = SubId;
        public AppPermission[] Permissions { get; init; } = Permissions;
    }
}