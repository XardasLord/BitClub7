using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Models;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetMultiAccounts
{
    public class GetMultiAccountsRequestHandler : IRequestHandler<GetMultiAccountsRequest, UserMultiAccountModel>
    {
        public Task<UserMultiAccountModel> Handle(GetMultiAccountsRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
