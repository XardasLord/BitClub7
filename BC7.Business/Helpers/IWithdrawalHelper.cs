namespace BC7.Business.Helpers
{
    public interface IWithdrawalHelper
    {
        decimal CalculateAmountToWithdraw(int matrixLevel);
        decimal CalculateAmountToWithdraw(decimal amount);
    }
}