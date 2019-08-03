using BC7.Business.Models;

namespace BC7.Business.Helpers
{
    public interface IWithdrawalHelper
    {
        decimal CalculateAmountToWithdraw(int matrixLevel);
        decimal CalculateAmountToWithdraw(decimal amount);
        decimal CalculateAmountToWithdraw(decimal amount, AffiliateProgramType affiliateProgramType);
    }
}