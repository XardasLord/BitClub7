namespace BC7.Business.Implementation.Users.Requests.GetInitiativeDescriptionForMultiAccount
{
    public class GetInitiativeDescriptionViewModel
    {
        public string Initiative { get; set; }

        /// <summary>
        /// Login
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Reflink
        /// </summary>
        public string ProjectCode { get; set; }
    }
}