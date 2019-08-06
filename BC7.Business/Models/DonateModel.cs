using System;

namespace BC7.Business.Models
{
	public class DonateModel
	{
		public Guid? UserMultiAccountId { get; set; }
		public decimal Amount { get; set; }
	}
}