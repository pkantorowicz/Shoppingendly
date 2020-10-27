using System;
using Conteque.Liberey.Domain.MediatR.DomainEvents;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Events
{
    public class UserRegistrationExpiredDomainEvent : DomainEventBaseMediatR
    {
        public UserRegistrationExpiredDomainEvent(
            Guid userRegistrationId,
            UserRegistrationStatus status)
        {
            UserRegistrationId = userRegistrationId;
            Status = status;
        }

        public Guid UserRegistrationId { get; }
        public UserRegistrationStatus Status { get; }
    }
}