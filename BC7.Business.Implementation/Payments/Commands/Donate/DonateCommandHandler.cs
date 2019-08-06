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
		private readonly Guid _rootId = Guid.Parse("441C799C-E2B7-4F1C-B141-DB3C6C1AF034"); // TODO: Move this to the settings

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

			var orderId = command.RequestedUserAccount.Id;

			// If donation is for foundation (command.UserMultiAccountId is null) then we set orderId (id of the user who makes payment) to the requested user ID
			var isDonationForFoundation = command.UserMultiAccountId.HasValue == false;
			var userPaymentForId = isDonationForFoundation ? _rootId : command.UserMultiAccountId.Value;

			var paymentResponse = await _bitBayPayFacade.CreatePayment(orderId, command.Amount);

            ValidateResponse(paymentResponse);

            var paymentHistory = new PaymentHistory(
                id: Guid.NewGuid(),
                paymentId: paymentResponse.Data.PaymentId,
                orderId: orderId,
				userPaymentForId: userPaymentForId,
				amountToPay: command.Amount,
                paymentFor: command.UserMultiAccountId.HasValue ? PaymentForHelper.ProjectDonation : PaymentForHelper.DonationForFoundation
            );
            await _paymentHistoryRepository.CreateAsync(paymentHistory);

            return new DonateViewModel
            {
                PaymentUrl = paymentResponse.Data.Url
            };
        }

        private async Task ValidateCommand(DonateCommand command)
        {
            if (!command.UserMultiAccountId.HasValue)
            {
                return;
            }

            var multiAccount = await _userMultiAccountRepository.GetAsync(command.UserMultiAccountId.Value);
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