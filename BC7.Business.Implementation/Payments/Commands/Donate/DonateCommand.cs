﻿using System;
using BC7.Business.Models;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.Donate
{
    public class DonateCommand : IRequest<DonateViewModel>
    {
        public Guid? UserMultiAccountId { get; set; }
        public LoggedUserModel RequestedUserAccount { get; set; }
		public decimal Amount { get; set; }
    }
}
