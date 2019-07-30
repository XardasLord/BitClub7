using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Models;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetUsers
{
    public class GetUsersRequestHandler : IRequestHandler<GetUsersRequest, GetUsersViewModel>
    {
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IMapper _mapper;

        public GetUsersRequestHandler(IUserAccountDataRepository userAccountDataRepository, IUserMultiAccountRepository userMultiAccountRepository, IMapper mapper)
        {
            _userAccountDataRepository = userAccountDataRepository;
            _userMultiAccountRepository = userMultiAccountRepository;
            _mapper = mapper;
        }

        public async Task<GetUsersViewModel> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var users = await _userAccountDataRepository.GetAllAsync();

            var userModels = _mapper.Map<List<UserAccountDataModel>>(users);

            // TODO: UGLY - TO REFACTOR
            foreach (var userAccount in users)
            {
                var mainMultiAccount = userAccount.UserMultiAccounts.First(x => x.IsMainAccount);

                if (!mainMultiAccount.SponsorId.HasValue)
                {
                    continue;
                }

                var mainAccountSponsor = await _userMultiAccountRepository.GetAsync(mainMultiAccount.SponsorId.Value);

                var userToSetSponsor = userModels.Single(x => x.Id == userAccount.Id);

                userToSetSponsor.MainAccountSponsor = mainAccountSponsor.MultiAccountName;
            }


            return new GetUsersViewModel
            {
                UserAccountsTotalCount = userModels.Count,
                UserAccounts = userModels
            };
        }
    }
}