using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Models;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Withdrawals.Requests.GetWithdrawals
{
    public class GetWithdrawalsRequestHandler : IRequestHandler<GetWithdrawalsRequest, GetWithdrawalsViewModel>
    {
        private readonly IWithdrawalRepository _withdrawalRepository;
        private readonly IMapper _mapper;

        public GetWithdrawalsRequestHandler(IWithdrawalRepository withdrawalRepository, IMapper mapper)
        {
            _withdrawalRepository = withdrawalRepository;
            _mapper = mapper;
        }

        public async Task<GetWithdrawalsViewModel> Handle(GetWithdrawalsRequest request, CancellationToken cancellationToken)
        {
            var withdrawals = await _withdrawalRepository.GetAllAsync();

            var withdrawalModels = _mapper.Map<List<WithdrawalModel>>(withdrawals);

            return new GetWithdrawalsViewModel
            {
                WithdrawalsTotalCount = withdrawalModels.Count,
                Withdrawals = withdrawalModels
            };
        }
    }
}