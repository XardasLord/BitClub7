using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Helpers;
using BC7.Business.Models;
using BC7.Database;
using BC7.Infrastructure.CustomExceptions;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetMultiAccounts
{
    public class GetMultiAccountsRequestHandler : IRequestHandler<GetMultiAccountsRequest, IEnumerable<UserMultiAccountModel>>
    {
        private readonly IBitClub7Context _context;
        private readonly IMapper _mapper;
        private readonly IUserAccountDataHelper _userAccountDataHelper;

        public GetMultiAccountsRequestHandler(IBitClub7Context context, IMapper mapper, IUserAccountDataHelper userAccountDataHelper)
        {
            _context = context;
            _mapper = mapper;
            _userAccountDataHelper = userAccountDataHelper;
        }

        public async Task<IEnumerable<UserMultiAccountModel>> Handle(GetMultiAccountsRequest request, CancellationToken cancellationToken)
        {
            var userAccount = await _userAccountDataHelper.GetById(request.UserAccountId);
            if (userAccount == null)
            {
                throw new AccountNotFoundException("User with given ID does not exist");
            }

            var userMultiAccountModels = _mapper.Map<IEnumerable<UserMultiAccountModel>>(userAccount.UserMultiAccounts);

            return userMultiAccountModels;
        }
    }
}
