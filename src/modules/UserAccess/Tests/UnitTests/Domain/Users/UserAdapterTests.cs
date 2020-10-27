using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conteque.Liberey.Domain.Adapters;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Shared;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Daos;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Events;
using Conteque.Shoppingendly.Modules.UserAccess.UnitTests.Domain.Base;
using FluentAssertions;
using Moq;
using Xunit;

namespace Conteque.Shoppingendly.Modules.UserAccess.UnitTests.Domain.Users
{
    public class UserAdapterTests : TestBase, IAsyncLifetime
    {
        private IAdapter<User, UserDao> _userRegistrationAdapter;
        private Mock<IUniqueUserChecker> _uniqueUserCheckerMock;

        public async Task InitializeAsync()
        {
            _userRegistrationAdapter = new UserAdapter();
            _uniqueUserCheckerMock = new Mock<IUniqueUserChecker>();

            await Task.CompletedTask;
        }

        [Fact]
        public void UserAdapter_MappingToDataAccessObject_IsSuccessful()
        {
            var user = TestDataGenerator.CreateUser(_uniqueUserCheckerMock.Object);

            var userDao = _userRegistrationAdapter.AdaptDomainObject(user);

            userDao.Id.Should().Be(user.Id);
            userDao.Email.Should().Be(user.Email);
            userDao.Password.Should().Be(user.Password);
            userDao.Login.Should().Be(user.Login);
            userDao.IsActive.Should().BeTrue();
            userDao.UserRoles.Should().Equal(
                user.UserRoles.Select(ur => ur.Name));
        }

        [Fact]
        public void UserAdapter_MappingToDomainObject_IsSuccessful()
        {
            var userDao = new UserDao
            {
                Id = Guid.NewGuid(),
                Email = "user@email.com",
                Password = "password",
                Login = "login",
                IsActive = true,
                UserRoles = new List<string> {"User"}
            };

            var user = _userRegistrationAdapter.AdaptDataAccessObject(userDao);

            user.Id.Should().Be(userDao.Id);
            user.Email.Should().Be(userDao.Email);
            user.Password.Should().Be(userDao.Password);
            user.Login.Should().Be(userDao.Login);
            user.IsActive.Should().BeTrue();
            user.UserRoles.Should().Equal(
                userDao.UserRoles.Select(ur => UserRole.Of(ur)));

            AssertDomainEventNotPublished<UserFetchedDomainEvent>(user);
        }

        public async Task DisposeAsync()
        {
            _userRegistrationAdapter = null;
            _uniqueUserCheckerMock = null;
            await Task.CompletedTask;
        }
    }
}