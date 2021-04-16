using MongoDB.Bson;

namespace Geex.Core.UserManagement
{
    public class UploadProfileInput
    {
        public ObjectId UserId { get; set; }
        public string? NewAvatar { get; set; }
    }
}