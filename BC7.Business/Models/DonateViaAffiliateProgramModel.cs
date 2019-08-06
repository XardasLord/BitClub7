using System;

namespace BC7.Business.Models
{
	public class DonateViaAffiliateProgramModel
	{
		public Guid UserMultiAccountId { get; set; }
		public decimal Amount { get; set; }
	}
}