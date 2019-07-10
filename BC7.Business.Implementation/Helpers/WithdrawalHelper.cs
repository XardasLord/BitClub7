using System;
using BC7.Business.Helpers;

namespace BC7.Business.Implementation.Helpers
{
    public class WithdrawalHelper : IWithdrawalHelper
    {
        public decimal CalculateAmountToWithdraw(int matrixLevel)
        {
            var lvl0 = 0.01M;
            var lvl1 = 0.03M;
            var lvl2 = 0.06M;
            var lvl3 = 0.12M;
            var lvl4 = 0.25M;
            var lvl5 = 0.50M;
            var lvl6 = 1.00M;
            var adminFee = 0.095M;

            switch (matrixLevel)
            {
                case 0:
                    return lvl0 - (adminFee * lvl0);
                case 1:
                    return lvl1 - (adminFee * lvl1);
                case 2:
                    return lvl2 - (adminFee * lvl2);
                case 3:
                    return lvl3 - (adminFee * lvl3);
                case 4:
                    return lvl4 - (adminFee * lvl4);
                case 5:
                    return lvl5 - (adminFee * lvl5);
                case 6:
                    return lvl6 - (adminFee * lvl6);
                default:
                    throw new ArgumentOutOfRangeException($"Invalid matrixLevel value: {matrixLevel}");
            }
        }
    }
}