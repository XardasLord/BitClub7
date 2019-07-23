using System;
using BC7.Business.Helpers;

namespace BC7.Business.Implementation.Helpers
{
    public class WithdrawalHelper : IWithdrawalHelper
    {
        private const decimal Lvl0 = 0.01M;
        private const decimal Lvl1 = 0.03M;
        private const decimal Lvl2 = 0.06M;
        private const decimal Lvl3 = 0.12M;
        private const decimal Lvl4 = 0.25M;
        private const decimal Lvl5 = 0.50M;
        private const decimal Lvl6 = 1.00M;
        private const decimal AdminFee = 0.095M;

        public decimal CalculateAmountToWithdraw(int matrixLevel)
        {

            switch (matrixLevel)
            {
                case 0:
                    return Lvl0 - (AdminFee * Lvl0);
                case 1:                       
                    return Lvl1 - (AdminFee * Lvl1);
                case 2:                       
                    return Lvl2 - (AdminFee * Lvl2);
                case 3:                       
                    return Lvl3 - (AdminFee * Lvl3);
                case 4:                       
                    return Lvl4 - (AdminFee * Lvl4);
                case 5:                       
                    return Lvl5 - (AdminFee * Lvl5);
                case 6:                       
                    return Lvl6 - (AdminFee * Lvl6);
                default:
                    throw new ArgumentOutOfRangeException($"Invalid matrixLevel value: {matrixLevel}");
            }
        }

        public decimal CalculateAmountToWithdraw(decimal amount)
        {
            return amount - (AdminFee * amount);
        }
    }
}