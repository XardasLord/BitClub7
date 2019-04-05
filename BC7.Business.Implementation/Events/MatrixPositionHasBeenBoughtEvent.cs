using System;
using MediatR;

namespace BC7.Business.Implementation.Events
{
    public class MatrixPositionHasBeenBoughtEvent : INotification
    {
        public Guid MatrixPositionId { get; set; }
    }
}
