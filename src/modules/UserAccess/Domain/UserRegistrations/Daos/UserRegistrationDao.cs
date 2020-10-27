using System;
using Conteque.Liberey.DataAccess;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Daos
{
    public class UserRegistrationDao : IDaoIdentity<Guid>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; }
        public DateTime ConfirmedDate { get; set; }
    }
}