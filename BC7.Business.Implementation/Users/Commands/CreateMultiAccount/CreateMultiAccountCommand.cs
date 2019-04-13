using System;
using MediatR;

namespace BC7.Business.Implementation.Users.Commands.CreateMultiAccount
{
    public class CreateMultiAccountCommand : IRequest<Guid>
    {
        public Guid UserAccountId { get; set; }
        public string SponsorReflink { get; set; }
    }
}
