using System;

namespace BC7.Business.Models
{
	public class DonationForFoundationModel
	{
		public Guid DonatedUserMultiAccountId { get; set; }
		public decimal Amount { get; set; }
	}
}