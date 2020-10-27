using Conteque.Shoppingendly.Modules.UserAccess.Domain.Shared;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users;

namespace Conteque.Shoppingendly.Modules.UserAccess.UnitTests.Domain.Base
{
    public static class TestDataGenerator
    {
        public static User CreateUser(IUniqueUserChecker uniqueUser)
        {
            var registration = UserRegistration.RegisterNewUser(
                "user@email.com",
                "password",
                uniqueUser,
                "confirmLink");
            
            registration.Confirm("login", uniqueUser);
            
            var user = registration.CreateUser(
                "login",
                uniqueUser);

            return user;
        }
    }
}