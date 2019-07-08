using System;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace BC7.Infrastructure.Implementation.Hangfire.Attributes
{
    public class OneYearExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
    {
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromDays(365);
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromDays(365);
        }
    }
}