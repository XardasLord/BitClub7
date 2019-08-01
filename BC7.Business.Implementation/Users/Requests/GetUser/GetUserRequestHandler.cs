using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Helpers;
using BC7.Business.Models;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetUser
{
    public class GetUserRequestHandler : IRequestHandler<GetUserRequest, UserAccountDataModel>
    {
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IWithdrawalRepository _withdrawalRepository;
        private readonly IUserMultiAccountHelper _multiAccountHelper;
        private readonly IMapper _mapper;

        public GetUserRequestHandler(
            IUserAccountDataRepository userAccountDataRepository, 
            IWithdrawalRepository withdrawalRepository, 
            IUserMultiAccountHelper multiAccountHelper,
            IMapper mapper)
        {
            _userAccountDataRepository = userAccountDataRepository;
            _withdrawalRepository = withdrawalRepository;
            _multiAccountHelper = multiAccountHelper;
            _mapper = mapper;
        }

        public async Task<UserAccountDataModel> Handle(GetUserRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidateRequest(request);

            var user = await _userAccountDataRepository.GetAsync(request.UserId);

            var userModel = _mapper.Map<UserAccountDataModel>(user);

            // More details for the home page
            var userWithdrawals = await _withdrawalRepository.GetAllAsync(user.UserMultiAccounts.Select(x => x.Id));
            var accountsWhereRequestedUserIsSponsor = await _multiAccountHelper.GetAllWhereMultiAccountsAreSponsors(user.UserMultiAccounts.Select(x => x.Id));

            userModel.EarnedBtc = userWithdrawals.Sum(x => x.Amount);
            userModel.InvitedAccountsTotalCount = accountsWhereRequestedUserIsSponsor.Count;
            userModel.AccountsInMatrixTotalCount = user.UserMultiAccounts.Count(x => x.RefLink != null);

            return userModel;
        }

        private static void ValidateRequest(GetUserRequest request)
        {
            if (request is null)
            {
                throw new ValidationException("Request has not been provided.");
            }
            if (request.UserId == Guid.Empty)
            {
                throw new ValidationException("UserId cannot be empty.");
            }
        }
    }
}