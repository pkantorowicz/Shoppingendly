namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.Shared
{
    public interface IUniqueUserChecker
    {
        int CheckIfUserEmailIsUnique(string email);
        int CheckIfUserLoginIsUnique(string login);
    }
}