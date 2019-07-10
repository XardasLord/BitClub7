using System.Threading.Tasks;
using BC7.Domain;
using BC7.Infrastructure.Implementation.Hangfire;
using BC7.Repository;
using Hangfire.Console;
using Hangfire.Server;
using Newtonsoft.Json;

namespace BC7.Business.Implementation.Withdrawals.Jobs
{
    public class CommitWithdrawalJob : IJob<Withdrawal>
    {
        private readonly IWithdrawalRepository _withdrawalRepository;

        public CommitWithdrawalJob(IWithdrawalRepository withdrawalRepository)
        {
            _withdrawalRepository = withdrawalRepository;
        }

        public async Task Execute(Withdrawal withdrawal, PerformContext context)
        {
            context.WriteLine($"CommitWithdrawalJob started with data - {JsonConvert.SerializeObject(withdrawal)}");

            await _withdrawalRepository.CreateAsync(withdrawal);

            context.WriteLine("CommitWithdrawalJob completed.");
        }
    }
}