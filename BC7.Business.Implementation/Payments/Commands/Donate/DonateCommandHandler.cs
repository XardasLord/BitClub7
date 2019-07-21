using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Infrastructure.Payments;
using BC7.Infrastructure.Payments.BodyModels;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.Donate
{
    public class DonateCommandHandler : IRequestHandler<DonateCommand, DonateViewModel>
    {
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;
        private readonly IBitBayPayFacade _bitBayPayFacade;

        public DonateCommandHandler(IUserMultiAccountRepository userMultiAccountRepository, IPaymentHistoryRepository paymentHistoryRepository, IBitBayPayFacade bitBayPayFacade)
        {
            _userMultiAccountRepository = userMultiAccountRepository;
            _paymentHistoryRepository = paymentHistoryRepository;
            _bitBayPayFacade = bitBayPayFacade;
        }

        public async Task<DonateViewModel> Handle(DonateCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            await ValidateCommand(command);

            var orderId = command.UserMultiAccountId;
            var paymentResponse = await _bitBayPayFacade.CreatePayment(orderId, command.Amount);

            ValidateResponse(paymentResponse);

            var paymentHistory = new PaymentHistory(
                id: Guid.NewGuid(),
                paymentId: paymentResponse.Data.PaymentId,
                orderId: orderId,
                amountToPay: command.Amount,
                paymentFor: PaymentForHelper.ProjectDonation
            );
            await _paymentHistoryRepository.CreateAsync(paymentHistory);

            return new DonateViewModel
            {
                PaymentUrl = paymentResponse.Data.Url
            };
        }

        private async Task ValidateCommand(DonateCommand command)
        {
            var multiAccount = await _userMultiAccountRepository.GetAsync(command.UserMultiAccountId);
            if (multiAccount is null)
            {
                throw new AccountNotFoundException("Multi account with given ID was not found");
            }
        }

        private static void ValidateResponse(CreatePaymentResponse response)
        {
            if (response.Status != "Ok")
            {
                throw new ValidationException("Status of the BitBayPay API call has failed");
            }
        }
    }
}