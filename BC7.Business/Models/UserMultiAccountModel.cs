using System;

namespace BC7.Business.Models
{
    public class UserMultiAccountModel
    {
        public Guid Id { get; set; }
        public string MultiAccountName { get; set; }
        public string Reflink { get; set; }
        // TODO: Matrix positions?
        // TODO: UserMultiAccountInviting?
    }
}
