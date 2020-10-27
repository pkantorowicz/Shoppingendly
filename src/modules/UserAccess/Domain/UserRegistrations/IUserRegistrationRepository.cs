using System;
using Conteque.Liberey.Domain.Repositories;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations
{
    public interface IUserRegistrationRepository : IRepository<UserRegistration, Guid>
    {
    }
}