using System.Text.Json.Serialization;

namespace mewo.Data
{
    public enum AllowedRole
    {
        User,
        Admin
    }

    public class UserRole
    {
        public string Username { get; set; }

        public AllowedRole Role { get; set; }
    }
}