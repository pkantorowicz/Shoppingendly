using System;
using System.Threading.Tasks;
using Conteque.Liberey.Domain.Adapters;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Shared;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Daos;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Events;
using Conteque.Shoppingendly.Modules.UserAccess.UnitTests.Domain.Base;
using FluentAssertions;
using Moq;
using Xunit;

namespace Conteque.Shoppingendly.Modules.UserAccess.UnitTests.Domain.UserRegistrations
{
    public class UserRegistrationAdapterTests : TestBase, IAsyncLifetime
    {
        private IAdapter<UserRegistration, UserRegistrationDao> _userRegistrationAdapter;
        private Mock<IUniqueUserChecker> _uniqueUserCheckerMock;

        public async Task InitializeAsync()
        {
            _userRegistrationAdapter = new UserRegistrationAdapter();
            _uniqueUserCheckerMock = new Mock<IUniqueUserChecker>();

            await Task.CompletedTask;
        }

        [Fact]
        public void UserRegistrationAdapter_MappingToDataAccessObject_IsSuccessful()
        {
            var userRegistration =
                UserRegistration.RegisterNewUser(
                    "user@email.com",
                    "password",
                    _uniqueUserCheckerMock.Object,
                    "confirmLink");

            var userRegistrationDao = _userRegistrationAdapter.AdaptDomainObject(userRegistration);

            userRegistrationDao.Id.Should().Be(userRegistration.Id);
            userRegistrationDao.Email.Should().Be(userRegistration.Email);
            userRegistrationDao.Password.Should().Be(userRegistration.Password);
            userRegistrationDao.RegistrationDate.Should().Be(userRegistration.RegistrationDate);
            userRegistrationDao.Status.Should().Be(userRegistration.Status.Code);
            userRegistrationDao.ConfirmedDate.Should().Be(userRegistrationDao.ConfirmedDate);
        }

        [Fact]
        public void UserRegistrationAdapter_MappingToDomainObject_IsSuccessful()
        {
            var userRegistrationDao = new UserRegistrationDao
            {
                Id = Guid.NewGuid(),
                Email = "user@email.com",
                Password = "password",
                RegistrationDate = new DateTime(2020, 06, 10),
                Status = "Confirmed",
                ConfirmedDate = new DateTime(2020, 10, 14)
            };

            var userRegistration = _userRegistrationAdapter.AdaptDataAccessObject(userRegistrationDao);

            userRegistration.Id.Should().Be(userRegistrationDao.Id);
            userRegistration.Email.Should().Be(userRegistrationDao.Email);
            userRegistration.Password.Should().Be(userRegistrationDao.Password);
            userRegistration.RegistrationDate.Should().Be(userRegistrationDao.RegistrationDate);
            userRegistration.Status.Should().Be(UserRegistrationStatus.Of(userRegistrationDao.Status));
            userRegistration.ConfirmedDate.Should().Be(userRegistrationDao.ConfirmedDate);

            AssertDomainEventNotPublished<UserRegistrationFetchedDomainEvent>(userRegistration);
        }

        public async Task DisposeAsync()
        {
            _userRegistrationAdapter = null;
            _uniqueUserCheckerMock = null;

            await Task.CompletedTask;
        }
    }
}