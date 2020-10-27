using System;
using System.Linq;
using Conteque.Liberey.Domain.Adapters;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Daos;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.Users
{
    public class UserAdapter : IAdapter<User, UserDao>
    {
        public UserDao AdaptDomainObject(User user)
        {
            return new UserDao
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                Login = user.Login,
                IsActive = user.IsActive,
                UserRoles = user.UserRoles
                    .Select(ur => ur.Name).ToList(),
                CreatedAt = DateTime.UtcNow
            };
        }

        public User AdaptDataAccessObject(UserDao userDao)
        {
            return User.CreateFromDao(userDao);
        }
    }
}