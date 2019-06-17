using System;
using BC7.Business.Models;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetUser
{
    public class GetUserRequest : IRequest<UserAccountDataModel>
    {
        public Guid UserId { get; set; }
    }
}
