using System;
using MediatR;

namespace BC7.Business.Implementation.Events
{
    public class MatrixPositionBoughtEvent : INotification
    {
        public Guid MatrixPositionId { get; set; }
    }
}
