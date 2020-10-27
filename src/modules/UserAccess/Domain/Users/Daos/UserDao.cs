using System;
using System.Collections.Generic;
using Conteque.Liberey.DataAccess;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Daos
{
    public class UserDao : IDaoIdentity<Guid>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> UserRoles { get; set; }

        public UserDao()
        {
            UserRoles = new List<string>();
        }
    }
}