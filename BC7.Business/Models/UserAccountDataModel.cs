using System;

namespace BC7.Business.Models
{
    public class UserAccountDataModel
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string Login { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string ZipCode { get; private set; }
        public string Country { get; private set; }
        public string BtcWalletAddress { get; private set; }
        public string Role { get; private set; }
        public string InitiativeDescription { get; private set; }
        public string Avatar { get; private set; }
        public bool IsMembershipFeePaid { get; private set; }
        public int MultiAccountsTotalCount { get; set; }
    }
}