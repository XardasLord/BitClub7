﻿using System;
using System.Collections.Generic;

namespace BC7.Entity
{
    public class UserAccountData
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string BtcWalletAddress { get; set; }
        public string Role { get; set; }
        public bool IsMembershipFeePaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<UserMultiAccount> UserMultiAccounts { get; set; }
    }
}
