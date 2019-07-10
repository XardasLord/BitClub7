using System;
using System.Collections.Generic;
using System.Linq;
using BC7.Common.Extensions;
using BC7.Infrastructure.CustomExceptions;
using BC7.Security;

namespace BC7.Domain
{
    public class UserAccountData
    {
        private readonly string[] _availableRoles = { UserRolesHelper.User, UserRolesHelper.Admin, UserRolesHelper.Root };

        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string Login { get; private set; }
        public string Salt { get; private set; }
        public string Hash { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string ZipCode { get; private set; }
        public string Country { get; private set; }
        public string BtcWalletAddress { get; private set; }
        public string Role { get; private set; }
        public string InitiativeDescription { get; private set; }
        public bool IsMembershipFeePaid { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public virtual ICollection<UserMultiAccount> UserMultiAccounts { get; private set; }

        private UserAccountData()
        {
        }

        public UserAccountData(Guid id, string email, string login, string firstName, string lastName, string street, string city, string zipCode, string country,
            string btcWalletAddress, string role)
        {
            ValidateDomain(id, email, login, firstName, lastName, street, city, zipCode, country, btcWalletAddress, role);

            Id = id;
            Email = email;
            Login = login;
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            Street = street;
            ZipCode = zipCode;
            Country = country;
            BtcWalletAddress = btcWalletAddress;
            CreatedAt = DateTime.UtcNow;
            Role = role;
            IsMembershipFeePaid = false;
            InitiativeDescription = null;
        }

        private static void ValidateDomain(Guid id, string email, string login, string firstName, string lastName, string street, string city, string zipCode, string country,
            string btcWalletAddress, string role)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException("Invalid ID.");
            }
            if (email.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid email.");
            }
            if (login.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid login.");
            }
            if (firstName.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid firstName.");
            }
            if (lastName.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid lastName.");
            }
            if (street.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid street.");
            }
            if (city.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid city.");
            }
            if (zipCode.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid zipCode.");
            }
            if (country.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid country.");
            }
            if (btcWalletAddress.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid btcWalletAddress.");
            }
            if (role.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid role.");
            }
        }

        public void SetPassword(string salt, string hash)
        {
            if (!Salt.IsNullOrWhiteSpace() || !Hash.IsNullOrWhiteSpace())
            {
                throw new DomainException("Cannot set salt/hash properties because they are already set");
            }

            Salt = salt;
            Hash = hash;
        }

        public void PaidMembershipFee()
        {
            if (IsMembershipFeePaid)
            {
                throw new DomainException("Membership's fee has been already paid by this account.");
            }

            IsMembershipFeePaid = true;
        }

        public void UpdateInformation(string firstName, string lastName, string street, string city, string zipCode, string country, string btcWalletAddress, string initiativeDescription)
        {
            if (firstName.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid firstName.");
            }
            if (lastName.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid lastName.");
            }
            if (street.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid street.");
            }
            if (city.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid city.");
            }
            if (zipCode.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid zipCode.");
            }
            if (country.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid country.");
            }
            if (btcWalletAddress.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid btcWalletAddress.");
            }

            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            ZipCode = zipCode;
            Country = country;
            BtcWalletAddress = btcWalletAddress;
            InitiativeDescription = initiativeDescription;
        }

        public void UpdateRole(string newRole)
        {
            if (!_availableRoles.Any(x => x.Contains(newRole)))
            {
                throw new DomainException($"Role - {newRole} - is invalid");
            }

            Role = newRole;
        }
    }
}
