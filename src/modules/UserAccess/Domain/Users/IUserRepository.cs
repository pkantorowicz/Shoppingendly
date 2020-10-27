using System;
using Conteque.Liberey.Domain.Repositories;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.Users
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}