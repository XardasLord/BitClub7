using System;
using BC7.Business.Models;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetMultiAccounts
{
    public class GetMultiAccountsRequest : IRequest<UserMultiAccountModel>
    {
        public Guid UserAccountId { get; set; }
    }
}
