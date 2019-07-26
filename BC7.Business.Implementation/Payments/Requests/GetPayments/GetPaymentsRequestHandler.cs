using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Models;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Payments.Requests.GetPayments
{
    public class GetPaymentsRequestHandler : IRequestHandler<GetPaymentsRequest, GetPaymentsViewModel>
    {
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;
        private readonly IMapper _mapper;

        public GetPaymentsRequestHandler(IPaymentHistoryRepository paymentHistoryRepository, IMapper mapper)
        {
            _paymentHistoryRepository = paymentHistoryRepository;
            _mapper = mapper;
        }

        public async Task<GetPaymentsViewModel> Handle(GetPaymentsRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            // TODO: Some pagination would be nice
            var payments = await _paymentHistoryRepository.GetAllAsync();

            var paymentModels = _mapper.Map<List<PaymentHistoryModel>>(payments);

            return new GetPaymentsViewModel
            {
                PaymentHistoryModel = paymentModels,
                TotalCount = paymentModels.Count
            };
        }
    }
}