using System;

namespace Conteque.Shoppingendly.Modules.UserAccess.Domain.UserRegistrations
{
    public class UserRegistrationSnapshot
    {
        public UserRegistrationId UserRegistrationId { get; }
        public string Email { get; }
        public string Password { get; }
        public DateTime RegistrationDate { get; }
        public UserRegistrationStatus Status { get; }
        public DateTime ConfirmedDate { get; }

        public UserRegistrationSnapshot(
            UserRegistrationId userRegistrationId,
            string email,
            string password,
            DateTime registrationDate,
            UserRegistrationStatus status,
            DateTime confirmedDate)
        {
            UserRegistrationId = userRegistrationId;
            Email = email;
            Password = password;
            RegistrationDate = registrationDate;
            Status = status;
            ConfirmedDate = confirmedDate;
        }
    }
}