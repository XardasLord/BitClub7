using System;
using BC7.Business.Helpers;
using BC7.Business.Models;

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
                    return Lvl0;
                case 1:
                    return Lvl1;
                case 2:                       
                    return Lvl2;
                case 3:                       
                    return Lvl3;
                case 4:                       
                    return Lvl4;
                case 5:                       
                    return Lvl5;
                case 6:                       
                    return Lvl6;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid matrixLevel value: {matrixLevel}");
            }
        }

        public decimal CalculateAmountToWithdraw(decimal amount)
        {
            return amount;
        }

        public decimal CalculateAmountToWithdraw(decimal amount, AffiliateProgramType affiliateProgramType)
        {
            switch (affiliateProgramType)
            {
                case AffiliateProgramType.DirectDonate:
                    return amount * 0.8M;
                case AffiliateProgramType.AffiliateLineA:
                    return amount * 0.1M;
                case AffiliateProgramType.AffiliateLineB:
                    return amount * 0.05M;
                case AffiliateProgramType.Bc7DonateFee:
                    return amount * 0.05M;
                case AffiliateProgramType.Bc7ConstFee:
                    return amount * 0.095M;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid AffiliateProgramType value: {affiliateProgramType}");
            }
        }
    }
}