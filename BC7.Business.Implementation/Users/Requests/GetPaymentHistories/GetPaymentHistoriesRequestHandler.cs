using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Models;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetPaymentHistories
{
    public class GetPaymentHistoriesRequestHandler : IRequestHandler<GetPaymentHistoriesRequest, IEnumerable<PaymentHistoryModel>>
    {
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IMapper _mapper;

        public GetPaymentHistoriesRequestHandler(
            IPaymentHistoryRepository paymentHistoryRepository,
            IUserAccountDataRepository userAccountDataRepository,
            IMapper mapper)
        {
            _paymentHistoryRepository = paymentHistoryRepository;
            _userAccountDataRepository = userAccountDataRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentHistoryModel>> Handle(GetPaymentHistoriesRequest request, CancellationToken cancellationToken)
        {
            var user = await _userAccountDataRepository.GetAsync(request.UserAccountId);
            if (user is null)
            {
                throw new AccountNotFoundException("User with given ID does not exist");
            }

            var allPayments = new List<PaymentHistoryModel>();

            var userPayments = await _paymentHistoryRepository.GetPaymentsByUser(user.Id);
            var userPaymentModels = _mapper.Map<List<PaymentHistoryModel>>(userPayments);

            foreach (var userPaymentModel in userPaymentModels)
            {
                userPaymentModel.AccountName = user.Login;
            }

            allPayments.AddRange(userPaymentModels);

            foreach (var userUserMultiAccount in user.UserMultiAccounts)
            {
                var payment = await _paymentHistoryRepository.GetPaymentsByUser(userUserMultiAccount.Id);
                var paymentModels = _mapper.Map<List<PaymentHistoryModel>>(payment);

                foreach (var paymentModel in paymentModels)
                {
                    paymentModel.AccountName = userUserMultiAccount.MultiAccountName;
                }

                allPayments.AddRange(paymentModels);
            }

            return allPayments;
        }
    }
}