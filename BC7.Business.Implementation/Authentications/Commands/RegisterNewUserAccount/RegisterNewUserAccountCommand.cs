using System;
using MediatR;

namespace BC7.Business.Implementation.Authentications.Commands.RegisterNewUserAccount
{
    public class RegisterNewUserAccountCommand : IRequest<Guid>
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string BtcWalletAddress { get; set; }
    }
}
