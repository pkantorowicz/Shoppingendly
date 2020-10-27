using System;
using Conteque.Liberey.Domain.MediatR.DomainEvents;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Events
{
    public class UserCreatedDomainEvent : DomainEventBaseMediatR
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string Password { get; }
        public string Login { get; }
        public bool IsActive { get; }

        public UserCreatedDomainEvent(
            Guid userId, 
            string email,
            string password,
            string login,
            bool isActive)
        {
            UserId = userId;
            Email = email;
            Password = password;
            Login = login;
            IsActive = isActive;
        }
    }
}