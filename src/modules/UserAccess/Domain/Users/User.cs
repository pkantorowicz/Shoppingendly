using System;
using System.Collections.Generic;
using System.Linq;
using Conteque.Liberey.Domain.Core.Aggregates;
using Conteque.Liberey.Domain.Events.DomainEvents;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Daos;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Events;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Rules;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.Users
{
    public class User : AggregateRoot
    {
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Login { get; private set; }
        public bool IsActive { get; private set; }
        public List<UserRole> UserRoles { get; private set; }

        private User()
        {
        }

        internal static User CreateFromUserRegistration(
            Guid userRegistrationId,
            string email,
            string password,
            string login
        )
        {
            var user = new User();
            var userCreated = new UserCreatedDomainEvent(
                userRegistrationId,
                password,
                email,
                login,
                true);

            user.Apply(userCreated);
            user.AddDomainEvent(userCreated);

            return user;
        }

        public static User CreateFromDao(UserDao userDao)
        {
            var user = new User();
            var userRoles = userDao.UserRoles.Select(urd => UserRole.Of(urd)).ToList();
            var userFetched = new UserFetchedDomainEvent(
                userDao.Id,
                userDao.Email,
                userDao.Password,
                userDao.Login,
                userDao.IsActive,
                userRoles);

            user.Apply(userFetched);

            return user;
        }

        public UserSnapshot GetSnapshot()
        {
            return new UserSnapshot(
                new UserId(Id),
                Email,
                Password,
                Login,
                IsActive,
                UserRoles);
        }

        public void SetNewPassword(string oldPassword, string newPassword)
        {
            CheckRule(new PasswordCannotBeChangedWhenProvidedOldPasswordDoNotMatchExistingRule(
                Password,
                oldPassword));

            var passwordChanged = new NewPasswordHasBeenSetDomainEvent(
                Id,
                newPassword,
                DateTime.UtcNow);

            Apply(passwordChanged);
            AddDomainEvent(passwordChanged);
        }

        protected override void Apply(IDomainEvent @event)
        {
            When((dynamic) @event);
        }

        private void When(UserCreatedDomainEvent @event)
        {
            Id = @event.UserId;
            Email = @event.Email;
            Password = @event.Password;
            Login = @event.Login;
            IsActive = @event.IsActive;

            UserRoles = new List<UserRole> {UserRole.User};
        }

        private void When(UserFetchedDomainEvent @event)
        {
            Id = @event.UserId;
            Email = @event.Email;
            Password = @event.Password;
            Login = @event.Login;
            IsActive = @event.IsActive;
            UserRoles = @event.UserRoles;
        }

        private void When(NewPasswordHasBeenSetDomainEvent @event)
        {
            Password = @event.NewPassword;
        }
    }
}