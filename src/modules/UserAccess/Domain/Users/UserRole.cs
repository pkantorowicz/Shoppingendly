using Conteque.Liberey.Domain.Core.ValueObjects;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.Users
{
    public class UserRole : ValueObject
    {
        public static UserRole Admin => new UserRole(nameof(Admin));
        public static UserRole SuperUser => new UserRole(nameof(SuperUser));
        public static UserRole User => new UserRole(nameof(User));
        
        public string Name { get; }

        private UserRole(string name)
        {
            Name = name;
        }

        public static UserRole Of(string name)
        {
            return new UserRole(name);
        }
    }
}