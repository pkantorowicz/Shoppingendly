using System;
using Conteque.Liberey.Domain.MediatR.DomainEvents;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Events
{
    public class NewUserRegisteredDomainEvent : DomainEventBaseMediatR
    {
        public Guid UserRegistrationId { get; }
        public string Email { get; }
        public string Password { get; }
        public DateTime RegistrationDate { get; }
        public string ConfirmLink { get; }

        public NewUserRegisteredDomainEvent(
            Guid userRegistrationId,
            string email,
            string password,
            DateTime registrationDate,
            string confirmLink)
        {
            UserRegistrationId = userRegistrationId;
            Email = email;
            Password = password;
            RegistrationDate = registrationDate;
            ConfirmLink = confirmLink;
        }
    }
}