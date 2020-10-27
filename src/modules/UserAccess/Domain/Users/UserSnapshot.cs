using System.Collections.Generic;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.Users
{
    public class UserSnapshot
    {
        public UserId UserId { get; }
        public string Email { get; }
        public string Password { get; }
        public string Login { get; }
        public bool IsActive { get; }
        public List<UserRole> UserRoles { get; }

        public UserSnapshot(
            UserId userId,
            string email,
            string password,
            string login,
            bool isActive,
            List<UserRole> userRoles)
        {
            UserId = userId;
            Email = email;
            Password = password;
            Login = login;
            IsActive = isActive;
            UserRoles = userRoles;
        }
    }
}