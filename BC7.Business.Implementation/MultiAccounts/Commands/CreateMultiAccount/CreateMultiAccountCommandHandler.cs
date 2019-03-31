using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BC7.Business.Implementation.MultiAccounts.Commands.CreateMultiAccount
{
    public    class CreateMultiAccountCommandHandler : IRequestHandler<CreateMultiAccountCommand, Guid>

    {
        public CreateMultiAccountCommandHandler()
        {
        }

        public Task<Guid> Handle(CreateMultiAccountCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
