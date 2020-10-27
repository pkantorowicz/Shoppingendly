using Conteque.Liberey.Domain.BusinessRules;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Shared;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations.Rules
{
    public class UserEmailMustBeUniqueRule : IBusinessRule
    {
        private readonly IUniqueUserChecker _uniqueUserChecker;
        private readonly string _email;

        internal UserEmailMustBeUniqueRule(IUniqueUserChecker uniqueUserChecker, string email)
        {
            _uniqueUserChecker = uniqueUserChecker;
            _email = email;
        }

        public bool IsBroken() => _uniqueUserChecker.CheckIfUserEmailIsUnique(_email) > 0;

        public string Message => "User email must be unique";
    }
}