using Conteque.Liberey.Domain.BusinessRules;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Shared;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Rules
{
    public class UserLoginMustBeUniqueRule : IBusinessRule
    {
        private readonly IUniqueUserChecker _uniqueUserChecker;
        private readonly string _login;

        internal UserLoginMustBeUniqueRule(
            IUniqueUserChecker uniqueUserChecker, 
            string login)
        {
            _uniqueUserChecker = uniqueUserChecker;
            _login = login;
        }

        public bool IsBroken() => _uniqueUserChecker.CheckIfUserLoginIsUnique(_login) > 0;

        public string Message => "User login must be unique";
    }
}