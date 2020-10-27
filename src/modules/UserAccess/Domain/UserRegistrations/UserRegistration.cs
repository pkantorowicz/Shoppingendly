using System;
using Conteque.Liberey.Domain.Core.Aggregates;
using Conteque.Liberey.Domain.Events.DomainEvents;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Shared;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Daos;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Events;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Rules;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations
{
    public class UserRegistration : AggregateRoot
    {
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public UserRegistrationStatus Status { get; private set; }
        public DateTime ConfirmedDate { get; private set; }

        private UserRegistration()
        {
        }
        
        public static UserRegistration RegisterNewUser(
            string email,
            string password,
            IUniqueUserChecker uniqueUserChecker,
            string confirmLink)
        {
            CheckRule(new UserEmailMustBeUniqueRule(uniqueUserChecker, email));

            var userRegistration = new UserRegistration();
            var newUserRegistered = new NewUserRegisteredDomainEvent(
                Guid.NewGuid(),
                email,
                password,
                DateTime.UtcNow,
                confirmLink);

            userRegistration.Apply(newUserRegistered);
            userRegistration.AddDomainEvent(newUserRegistered);

            return userRegistration;
        }

        public static UserRegistration CreateFromDao(
            UserRegistrationDao userRegistrationDao)
        {
            var userRegistration = new UserRegistration();
            var userRegistrationFetched = new UserRegistrationFetchedDomainEvent(
                userRegistrationDao.Id,
                userRegistrationDao.Email,
                userRegistrationDao.Password,
                userRegistrationDao.RegistrationDate,
                UserRegistrationStatus.Of(userRegistrationDao.Status),
                userRegistrationDao.ConfirmedDate);

            userRegistration.Apply(userRegistrationFetched);

            return userRegistration;
        }

        public User CreateUser(
            string login, 
            IUniqueUserChecker uniqueUserChecker)
        {
            CheckRule(new UserCannotBeCreatedWhenRegistrationIsNotConfirmedRule(Status));

            return User.CreateFromUserRegistration(
                Id,
                Email,
                Password,
                login);
        }

        public UserRegistrationSnapshot GetSnapshot()
        {
            return new UserRegistrationSnapshot(
                new UserRegistrationId(Id), 
                Email, 
                Password, 
                RegistrationDate, 
                Status,
                ConfirmedDate);
        }
        
        public void Confirm(string login, IUniqueUserChecker uniqueUserChecker)
        {
            CheckRule(new UserLoginMustBeUniqueRule(uniqueUserChecker, login));
            CheckRule(new UserRegistrationCannotBeConfirmedMoreThanOnceRule(Status));
            CheckRule(new UserRegistrationCannotBeConfirmedAfterExpirationRule(Status));

            var userRegistrationConfirmed =
                new UserRegistrationConfirmedDomainEvent(
                    Id,
                    UserRegistrationStatus.Confirmed,
                    login,
                    DateTime.UtcNow);

            Apply(userRegistrationConfirmed);
            AddDomainEvent(userRegistrationConfirmed);
        }

        public void Expire()
        {
            CheckRule(new UserRegistrationCannotBeExpiredMoreThanOnceRule(Status));

            var userRegistrationExpired =
                new UserRegistrationExpiredDomainEvent(
                    Id,
                    UserRegistrationStatus.Expired);

            Apply(userRegistrationExpired);
            AddDomainEvent(userRegistrationExpired);
        }

        protected override void Apply(IDomainEvent @event)
        {
            When((dynamic) @event);
        }

        private void When(NewUserRegisteredDomainEvent @event)
        {
            Id = @event.UserRegistrationId;
            Password = @event.Password;
            Email = @event.Email;
            RegistrationDate = @event.RegistrationDate;
            Status = UserRegistrationStatus.WaitingForConfirmation;
        }

        private void When(UserRegistrationFetchedDomainEvent @event)
        {
            Id = @event.UserRegistrationId;
            Password = @event.Password;
            Email = @event.Email;
            RegistrationDate = @event.RegistrationDate;
            Status = @event.Status;
            ConfirmedDate = @event.ConfirmedDate;
        }

        private void When(UserRegistrationConfirmedDomainEvent @event)
        {
            Status = @event.Status;
            ConfirmedDate = @event.ConfirmedDate;
        }

        private void When(UserRegistrationExpiredDomainEvent @event)
        {
            Status = @event.Status;
        }
    }
}