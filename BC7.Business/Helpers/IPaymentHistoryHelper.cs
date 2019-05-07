using System;
using System.Threading.Tasks;

namespace BC7.Business.Helpers
{
    public interface IPaymentHistoryHelper
    {
        Task<bool> DoesUserPaidForMatrixLevelAsync(int matrixLevelUpgrade, Guid userMultiAccountId);
    }
}
