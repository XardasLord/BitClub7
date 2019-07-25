using System;

namespace BC7.Business.Implementation.Withdrawals.Jobs.JobModels
{
    public class InitWithdrawalModel
    {
        public Guid MatrixPositionId { get; set; }
        public string WithdrawalFor { get; set; }      
    }
}