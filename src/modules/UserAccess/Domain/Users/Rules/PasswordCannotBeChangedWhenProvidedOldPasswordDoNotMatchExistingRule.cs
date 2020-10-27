using Conteque.Liberey.Domain.BusinessRules;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.Users.Rules
{
    public class PasswordCannotBeChangedWhenProvidedOldPasswordDoNotMatchExistingRule : IBusinessRule
    {
        private readonly string _existingPassword;
        private readonly string _providedOldPassword;

        public PasswordCannotBeChangedWhenProvidedOldPasswordDoNotMatchExistingRule(
            string existingPassword,
            string providedOldPassword)
        {
            _existingPassword = existingPassword;
            _providedOldPassword = providedOldPassword;
        }

        public bool IsBroken() => _existingPassword != _providedOldPassword;

        public string Message => "Provided old password must be the same as existing.";
    }
}