using System;
using System.Collections.Generic;
using BC7.Common.Extensions;
using BC7.Infrastructure.CustomExceptions;

namespace BC7.Domain
{
    public class UserMultiAccount
    {
        public Guid Id { get; set; }
        public Guid UserAccountDataId { get; set; }
        public virtual UserAccountData UserAccountData { get; set; }

        // TODO: Change it to SponsorId
        public Guid? UserMultiAccountInvitingId { get; set; }
        public virtual UserMultiAccount UserMultiAccountInviting { get; set; }

        public string MultiAccountName { get; set; }
        public string RefLink { get; set; }
        public bool IsMainAccount { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<MatrixPosition> MatrixPositions { get; set; }

        private UserMultiAccount()
        {
        }

        public UserMultiAccount(Guid id, Guid userAccountDataId, Guid? userMultiAccountInvitingId, string multiAccountName)
        {
            ValidateDomain(id, userAccountDataId, multiAccountName);

            Id = id;
            UserAccountDataId = userAccountDataId;
            UserMultiAccountInvitingId = userMultiAccountInvitingId;
            MultiAccountName = multiAccountName;
            CreatedAt = DateTime.UtcNow;
            RefLink = null;
            IsMainAccount = false;
        }

        private static void ValidateDomain(Guid id, Guid userAccountDataId, string multiAccountName)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException("Invalid ID.");
            }
            if (userAccountDataId == Guid.Empty)
            {
                throw new DomainException("Invalid userAccountDataId.");
            }
            if (multiAccountName.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid multiAccountName.");
            }
        }

        public void SetAsMainAccount()
        {
            if (IsMainAccount)
            {
                throw new DomainException("This account has been already set as a main account");
            }

            IsMainAccount = true;
        }

        public void SetReflink(string reflink)
        {
            if (reflink.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid reflink.");
            }

            RefLink = reflink;
        }

        public void ChangeSponsor(Guid sponsorId)
        {
            UserMultiAccountInvitingId = sponsorId;
        }
    }
}
