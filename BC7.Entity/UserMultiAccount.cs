using System;
using System.Collections.Generic;

namespace BC7.Entity
{
    public class UserMultiAccount
    {
        public Guid Id { get; set; }
        public Guid UserAccountDataId { get; set; }
        public virtual UserAccountData UserAccountData { get; set; }

        public virtual Guid? UserMultiAccountInvitingId { get; set; }
        public virtual UserMultiAccount UserMultiAccountInviting { get; set; }

        public string MultiAccountName { get; set; }
        public string RefLink { get; set; }
        public bool IsMainAccount { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<MatrixPosition> MatrixPositions { get; set; }
    }
}
