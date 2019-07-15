using System.Collections.Generic;
using BC7.Business.Models;

namespace BC7.Business.Implementation.Users.Requests.GetUsers
{
    public class GetUsersViewModel
    {
        public int UserAccountsTotalCount { get; set; }
        public IEnumerable<UserAccountDataModel> UserAccounts { get; set; }
    }
}