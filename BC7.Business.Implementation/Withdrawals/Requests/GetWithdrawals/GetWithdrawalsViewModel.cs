using System.Collections.Generic;
using BC7.Business.Models;

namespace BC7.Business.Implementation.Withdrawals.Requests.GetWithdrawals
{
    public class GetWithdrawalsViewModel
    {
        public int WithdrawalsTotalCount { get; set; }
        public IEnumerable<WithdrawalModel> Withdrawals { get; set; }
    }
}
