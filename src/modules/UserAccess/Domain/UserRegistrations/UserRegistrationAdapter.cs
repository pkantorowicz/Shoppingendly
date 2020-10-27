using Conteque.Liberey.Domain.Adapters;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Daos;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations
{
    public class UserRegistrationAdapter : IAdapter<UserRegistration, UserRegistrationDao>
    {
        public UserRegistrationDao AdaptDomainObject(UserRegistration userRegistration)
        {
            return new UserRegistrationDao
            {
                Id = userRegistration.Id,
                Email = userRegistration.Email,
                Password = userRegistration.Password,
                ConfirmedDate = userRegistration.ConfirmedDate,
                Status = userRegistration.Status.Code,
                RegistrationDate = userRegistration.RegistrationDate
            };
        }

        public UserRegistration AdaptDataAccessObject(UserRegistrationDao userRegistrationDao)
        {
            return UserRegistration.CreateFromDao(userRegistrationDao);
        }
    }
}