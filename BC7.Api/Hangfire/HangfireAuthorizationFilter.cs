using BC7.Security;
using Hangfire.Dashboard;

namespace BC7.Api.Hangfire
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            if (httpContext.User.IsInRole(UserRolesHelper.Admin))
            {
                return true;
            }

            return false;
        }
    }
}
