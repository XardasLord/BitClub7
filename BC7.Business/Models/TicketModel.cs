using System;

namespace BC7.Business.Models
{
    public class TicketModel
    {
        public Guid Id { get; set; }
        public string TicketNumber { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
    }
}
