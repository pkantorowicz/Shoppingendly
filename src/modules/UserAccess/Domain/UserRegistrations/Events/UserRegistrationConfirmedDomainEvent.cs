using System;
using Conteque.Liberey.Domain.MediatR.DomainEvents;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Events
{
    public class UserRegistrationConfirmedDomainEvent : DomainEventBaseMediatR
    {
        public UserRegistrationConfirmedDomainEvent(
            Guid userRegistrationId, 
            UserRegistrationStatus status, 
            string login,
            DateTime confirmedDate)
        {
            UserRegistrationId = userRegistrationId;
            Status = status;
            Login = login;
            ConfirmedDate = confirmedDate;
        }

        public Guid UserRegistrationId { get; }
        public UserRegistrationStatus Status { get; }
        public string Login { get; }
        public DateTime ConfirmedDate { get; }
    }
}