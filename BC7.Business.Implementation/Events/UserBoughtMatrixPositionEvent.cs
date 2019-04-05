using System;
using MediatR;

namespace BC7.Business.Implementation.Events
{
    public class UserBoughtMatrixPositionEvent : INotification
    {
        public Guid MultiAccountId { get; set; }
    }
}
