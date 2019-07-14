using System;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetInitiativeDescriptionForMultiAccount
{
    public class GetInitiativeDescriptionRequest : IRequest<GetInitiativeDescriptionViewModel>
    {
        public Guid MultiAccountId { get; set; }
    }
}
