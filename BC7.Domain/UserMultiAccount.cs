using System;
using System.Collections.Generic;
using BC7.Common.Extensions;
using BC7.Infrastructure.CustomExceptions;

namespace BC7.Domain
{
    public class UserMultiAccount
    {
        public Guid Id { get; private set; }
        public Guid UserAccountDataId { get; private set; }
        public virtual UserAccountData UserAccountData { get; private set; }

        // TODO: Change it to SponsorId
        public Guid? SponsorId { get; private set; }
        public virtual UserMultiAccount Sponsor { get; private set; }

        public string MultiAccountName { get; private set; }
        public string RefLink { get; private set; }
        public bool IsMainAccount { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public virtual ICollection<MatrixPosition> MatrixPositions { get; private set; }

        private UserMultiAccount()
        {
        }

        public UserMultiAccount(Guid id, Guid userAccountDataId, Guid? sponsorId, string multiAccountName)
        {
            ValidateDomain(id, userAccountDataId, multiAccountName);

            Id = id;
            UserAccountDataId = userAccountDataId;
            SponsorId = sponsorId;
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

            if (!RefLink.IsNullOrWhiteSpace())
            {
                throw new DomainException("This account has a reflink already set.");
            }

            RefLink = reflink;
        }

        public void ChangeSponsor(Guid sponsorId)
        {
            SponsorId = sponsorId;
        }
    }
}
