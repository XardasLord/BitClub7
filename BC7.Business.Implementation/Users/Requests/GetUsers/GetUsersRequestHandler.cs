using System.Collections.Generic;
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
        private readonly IMapper _mapper;

        public GetUsersRequestHandler(IUserAccountDataRepository userAccountDataRepository, IMapper mapper)
        {
            _userAccountDataRepository = userAccountDataRepository;
            _mapper = mapper;
        }

        public async Task<GetUsersViewModel> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var users = await _userAccountDataRepository.GetAllAsync();

            var userModels = _mapper.Map<List<UserAccountDataModel>>(users);

            return new GetUsersViewModel
            {
                UserAccountsTotalCount = userModels.Count,
                UserAccounts = userModels
            };
        }
    }
}