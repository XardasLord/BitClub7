using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Database;
using MediatR;

namespace BC7.Business.Implementation.Authentications.Commands.RegisterNewUserAccount
{
    public class RegisterNewUserAccountCommandHandler : IRequestHandler<RegisterNewUserAccountCommand, Guid>
    {
        private readonly IBitClub7Context _context;

        public RegisterNewUserAccountCommandHandler(IBitClub7Context context)
        {
            _context = context;
        }

        public Task<Guid> Handle(RegisterNewUserAccountCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}