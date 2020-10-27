using System;
using Conteque.Liberey.Domain.Core.Aggregates;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations
{
    public class UserRegistrationId : AggregateId<UserRegistration>
    {
        public UserRegistrationId(Guid value)
            : base(value)
        {
        }
    }
}