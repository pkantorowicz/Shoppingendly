using System.Threading.Tasks;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Shared;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Events;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Rules;
using Conteque.Shoppingendly.Modules.UserAccess.UnitTests.Domain.Base;
using FluentAssertions;
using Moq;
using Xunit;

namespace Conteque.Shoppingendly.Modules.UserAccess.UnitTests.Domain.Users
{
    public class UserTests : TestBase, IAsyncLifetime
    {
        private Mock<IUniqueUserChecker> _uniqueUserCheckerMock;
        
        public async Task InitializeAsync()
        {
            _uniqueUserCheckerMock = new Mock<IUniqueUserChecker>();
            
            await Task.CompletedTask;
        }

        [Fact]
        public void UserChangingPassword_WhenOldPasswordDoNotMatch_CannotBeChanged()
        {
            var user = TestDataGenerator.CreateUser(_uniqueUserCheckerMock.Object);

            AssertBrokenRule<PasswordCannotBeChangedWhenProvidedOldPasswordDoNotMatchExistingRule>(() =>
                user.SetNewPassword("oldPassword", "newPassword"));
        }

        [Fact]
        public void UserChangingPassword_WhenOldPasswordIsMatched_IsSuccessful()
        {
            const string newPassword = "newPassword";
            var user = TestDataGenerator.CreateUser(_uniqueUserCheckerMock.Object);
            
            user.SetNewPassword(user.Password, newPassword);

            AssertPublishedDomainEvent<NewPasswordHasBeenSetDomainEvent>(user);

            user.Password.Should().BeEquivalentTo(newPassword);
        }
        
        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}