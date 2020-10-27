using System.Threading.Tasks;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Shared;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Events;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Rules;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Events;
using Conteque.Shoppingendly.Modules.UserAccess.UnitTests.Domain.Base;
using FluentAssertions;
using Moq;
using Xunit;

namespace Conteque.Shoppingendly.Modules.UserAccess.UnitTests.Domain.UserRegistrations
{
    public class UserRegistrationTests : TestBase, IAsyncLifetime
    {
        private const string Login = "login";

        private Mock<IUniqueUserChecker> _uniqueUserCheckerMock;

        public async Task InitializeAsync()
        {
            _uniqueUserCheckerMock = new Mock<IUniqueUserChecker>();

            await Task.CompletedTask;
        }

        [Fact]
        public void NewUserRegistration_WithUniqueEmail_IsSuccessful()
        {
            // Arrange

            // Act
            var userRegistration =
                UserRegistration.RegisterNewUser(
                    "user@email.com",
                    "password",
                    _uniqueUserCheckerMock.Object,
                    "confirmLink");

            // Assert
            var newUserRegisteredDomainEvent =
                AssertPublishedDomainEvent<NewUserRegisteredDomainEvent>(userRegistration);
            newUserRegisteredDomainEvent.UserRegistrationId.Should().Be(userRegistration.Id);
        }

        [Fact]
        public void NewUserRegistration_WithoutUniqueEmail_BreaksUserLoginMustBeUniqueRule()
        {
            // Arrange
            _uniqueUserCheckerMock
                .Setup(usc => usc.CheckIfUserEmailIsUnique("user@email.com"))
                .Returns(1);

            // Assert
            AssertBrokenRule<UserEmailMustBeUniqueRule>(()
                => UserRegistration.RegisterNewUser(
                    "user@email.com",
                    "password",
                    _uniqueUserCheckerMock.Object,
                    "confirmLink"));
        }

        [Fact]
        public void ConfirmingUserRegistration_WhenWaitingForConfirmation_IsSuccessful()
        {
            var registration = UserRegistration.RegisterNewUser(
                "user@email.com",
                "password",
                _uniqueUserCheckerMock.Object,
                "confirmLink");

            registration.Confirm(Login, _uniqueUserCheckerMock.Object);

            var userRegistrationConfirmedDomainEvent =
                AssertPublishedDomainEvent<UserRegistrationConfirmedDomainEvent>(registration);
            userRegistrationConfirmedDomainEvent.UserRegistrationId.Should().Be(registration.Id);
        }

        [Fact]
        public void UserRegistration_WhenIsConfirmed_CannotBeConfirmedAgain()
        {
            var registration = UserRegistration.RegisterNewUser(
                "user@email.com",
                "password",
                _uniqueUserCheckerMock.Object,
                "confirmLink");

            registration.Confirm(Login, _uniqueUserCheckerMock.Object);

            AssertBrokenRule<UserRegistrationCannotBeConfirmedMoreThanOnceRule>(() =>
            {
                registration.Confirm(Login, _uniqueUserCheckerMock.Object);
            });
        }

        [Fact]
        public void UserRegistration_WhenIsExpired_CannotBeConfirmed()
        {
            var registration = UserRegistration.RegisterNewUser(
                "user@email.com",
                "password",
                _uniqueUserCheckerMock.Object,
                "confirmLink");

            registration.Expire();

            AssertBrokenRule<UserRegistrationCannotBeConfirmedAfterExpirationRule>(() =>
            {
                registration.Confirm(Login, _uniqueUserCheckerMock.Object);
            });
        }

        [Fact]
        public void UserRegistration_WhenWaitingForConfirmation_MustHasUniqueLogin()
        {
            var registration = UserRegistration.RegisterNewUser(
                "user@email.com",
                "password",
                _uniqueUserCheckerMock.Object,
                "confirmLink");

            _uniqueUserCheckerMock
                .Setup(usc => usc.CheckIfUserLoginIsUnique(Login))
                .Returns(1);

            AssertBrokenRule<UserLoginMustBeUniqueRule>(
                () => registration.Confirm(Login, _uniqueUserCheckerMock.Object));
        }

        [Fact]
        public void ExpiringUserRegistration_WhenWaitingForConfirmation_IsSuccessful()
        {
            var registration = UserRegistration.RegisterNewUser(
                "user@email.com",
                "password",
                _uniqueUserCheckerMock.Object,
                "confirmLink");

            registration.Expire();

            var userRegistrationExpired = AssertPublishedDomainEvent<UserRegistrationExpiredDomainEvent>(registration);

            userRegistrationExpired.UserRegistrationId.Should().Be(registration.Id);
        }

        [Fact]
        public void UserRegistration_WhenIsExpired_CannotBeExpiredAgain()
        {
            var registration = UserRegistration.RegisterNewUser(
                "user@email.com",
                "password",
                _uniqueUserCheckerMock.Object,
                "confirmLink");

            registration.Expire();

            AssertBrokenRule<UserRegistrationCannotBeExpiredMoreThanOnceRule>(() => { registration.Expire(); });
        }

        [Fact]
        public void CreateUser_WhenRegistrationIsConfirmed_IsSuccessful()
        {
            var registration = UserRegistration.RegisterNewUser(
                "user@email.com",
                "password",
                _uniqueUserCheckerMock.Object,
                "confirmLink");

            registration.Confirm(Login, _uniqueUserCheckerMock.Object);

            var user = registration.CreateUser(
                Login,
                _uniqueUserCheckerMock.Object);

            var userCreated = AssertPublishedDomainEvent<UserCreatedDomainEvent>(user);

            user.Id.Should().Be(registration.Id);
            userCreated.UserId.Should().Be(registration.Id);
        }

        [Fact]
        public void UserCreation_WhenRegistrationIsNotConfirmed_IsNotPossible()
        {
            var registration = UserRegistration.RegisterNewUser(
                "user@email.com",
                "password",
                _uniqueUserCheckerMock.Object,
                "confirmLink");

            AssertBrokenRule<UserCannotBeCreatedWhenRegistrationIsNotConfirmedRule>(
                () => registration.CreateUser(Login, _uniqueUserCheckerMock.Object));
        }

        public async Task DisposeAsync()
        {
            _uniqueUserCheckerMock = null;

            await Task.CompletedTask;
        }
    }
}