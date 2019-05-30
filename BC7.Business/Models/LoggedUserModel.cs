using System;

namespace BC7.Business.Models
{
    public class LoggedUserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public LoggedUserModel(Guid id, string email, string role)
        {
            Id = id;
            Email = email;
            Role = role;
        }
    }
}
