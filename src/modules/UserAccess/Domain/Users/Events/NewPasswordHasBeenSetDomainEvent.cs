using System;
using Conteque.Liberey.Domain.MediatR.DomainEvents;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Events
{
    public class NewPasswordHasBeenSetDomainEvent : DomainEventBaseMediatR
    {
        public Guid UserId { get; }
        public string NewPassword { get; }
        public DateTime ChangedDate { get; }

        public NewPasswordHasBeenSetDomainEvent(
            Guid userId,
            string newPassword,
            DateTime changedDate)
        {
            UserId = userId;
            NewPassword = newPassword;
            ChangedDate = changedDate;
        }
    }
}