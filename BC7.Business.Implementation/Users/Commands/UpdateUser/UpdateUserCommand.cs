using System;
using BC7.Business.Models;
using MediatR;

namespace BC7.Business.Implementation.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public Guid UserId { get; set; }
        public LoggedUserModel RequestedUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string BtcWalletAddress { get; set; }
        public string Role { get; set; }
    }
}