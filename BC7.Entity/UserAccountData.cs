using BC7.Entity.AbstractBase;

namespace BC7.Entity
{
    public class UserAccountData : BaseEntity
    {
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
    }
}
