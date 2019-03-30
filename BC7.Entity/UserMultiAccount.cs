using System;
using BC7.Entity.AbstractBase;

namespace BC7.Entity
{
    public class UserMultiAccount : BaseEntity
    {
        public Guid UserAccountDataId { get; set; }
        public virtual UserAccountData UserAccountData { get; set; }

        public virtual Guid? UserMultiAccountInvitingId { get; set; }
        public virtual UserMultiAccount UserMultiAccountInviting { get; set; }

        public int AccountNumber { get; set; }
        public string RefLink { get; set; }
    }
}
