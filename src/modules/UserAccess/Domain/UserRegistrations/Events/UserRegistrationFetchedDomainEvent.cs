using System;
using Conteque.Liberey.Domain.Events.DomainEvents;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Events
{
    public class UserRegistrationFetchedDomainEvent : DomainEventBase
    {
        public Guid UserRegistrationId { get; }
        public string Email { get; }
        public string Password { get; }
        public DateTime RegistrationDate { get; }
        public UserRegistrationStatus Status { get; }
        public DateTime ConfirmedDate { get; }

        public UserRegistrationFetchedDomainEvent(
            Guid userRegistrationId,
            string email,
            string password,
            DateTime registrationDate,
            UserRegistrationStatus status,
            DateTime confirmedDate)
        {
            UserRegistrationId = userRegistrationId;
            Email = email;
            Password = password;
            RegistrationDate = registrationDate;
            Status = status;
            ConfirmedDate = confirmedDate;
        }
    }
}