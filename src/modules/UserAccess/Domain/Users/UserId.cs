using System;
using Conteque.Liberey.Domain.Core.Aggregates;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.Users
{
    public class UserId : AggregateId<User>
    {
        public UserId(Guid value)
            : base(value)
        {
        }
    }
}