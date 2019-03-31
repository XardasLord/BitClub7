using System;
using System.Collections.Generic;
using BC7.Business.Models;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetMultiAccounts
{
    public class GetMultiAccountsRequest : IRequest<IEnumerable<UserMultiAccountModel>>
    {
        public Guid UserAccountId { get; set; }
    }
}
