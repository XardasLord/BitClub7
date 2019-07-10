namespace BC7.Business.Models
{
    public class UpdateUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string BtcWalletAddress { get; set; }
        public string Role { get; set; }
        public string InitiativeDescription { get; set; }
    }
}
