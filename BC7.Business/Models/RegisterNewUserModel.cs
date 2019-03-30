namespace BC7.Business.Models
{
    public class RegisterNewUserModel
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string BtcWalletAddress { get; set; }
        public string InvitingUserLogin { get; set; }
    }
}
