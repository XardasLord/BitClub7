using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Models;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetMultiAccounts
{
    public class GetMultiAccountsRequestHandler : IRequestHandler<GetMultiAccountsRequest, IEnumerable<UserMultiAccountModel>>
    {
        private readonly IMapper _mapper;
        private readonly IUserAccountDataRepository _userAccountDataRepository;

        public GetMultiAccountsRequestHandler(IMapper mapper, IUserAccountDataRepository userAccountDataRepository)
        {
            _mapper = mapper;
            _userAccountDataRepository = userAccountDataRepository;
        }

        public async Task<IEnumerable<UserMultiAccountModel>> Handle(GetMultiAccountsRequest request, CancellationToken cancellationToken)
        {
            var userAccount = await _userAccountDataRepository.GetAsync(request.UserAccountId);
            if (userAccount == null)
            {
                throw new AccountNotFoundException("User with given ID does not exist");
            }

            var userMultiAccountModels = _mapper.Map<IEnumerable<UserMultiAccountModel>>(userAccount.UserMultiAccounts);

            return userMultiAccountModels;
        }
    }
}
