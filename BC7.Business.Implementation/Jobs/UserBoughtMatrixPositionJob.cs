using System;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Infrastructure.Implementation.Hangfire;
using BC7.Repository;
using Hangfire.Console;
using Hangfire.Server;

namespace BC7.Business.Implementation.Jobs
{
    public class UserBoughtMatrixPositionJob : IJob<Guid>
    {
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IReflinkHelper _reflinkHelper;

        public UserBoughtMatrixPositionJob(IUserMultiAccountRepository userMultiAccountRepository, IReflinkHelper reflinkHelper)
        {
            _userMultiAccountRepository = userMultiAccountRepository;
            _reflinkHelper = reflinkHelper;
        }

        public async Task Execute(Guid multiAccountId, PerformContext context)
        {
            context.WriteLine($"UserBoughtMatrixPositionJob started with multiAccountId - {multiAccountId}");

            var multiAccount = await _userMultiAccountRepository.GetAsync(multiAccountId);

            multiAccount.SetReflink(_reflinkHelper.GenerateReflink());

            await _userMultiAccountRepository.UpdateAsync(multiAccount);

            context.WriteLine("UserBoughtMatrixPositionJob completed.");
        }
    }
}