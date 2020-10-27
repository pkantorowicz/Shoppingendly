using System;
using System.Collections.Generic;
using Conteque.Liberey.Domain.Events.DomainEvents;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Events
{
    public class UserFetchedDomainEvent : DomainEventBase
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string Password { get; }
        public string Login { get; }
        public bool IsActive { get; }
        public List<UserRole> UserRoles { get; }

        public UserFetchedDomainEvent(
            Guid userId,
            string email, 
            string password,
            string login,
            bool isActive, 
            List<UserRole> userRoles)
        {
            UserId = userId;
            Email = email;
            Password = password;
            IsActive = isActive;
            Login = login;
            UserRoles = userRoles;
        }
    }
}