using System;
using BC7.Common.Extensions;
using BC7.Infrastructure.CustomExceptions;

namespace BC7.Domain
{
    public class Ticket
    {
        public Guid Id { get; private set; }
        public int Number { get; private set; }
        public string FullTicketNumber { get; private set; }
        public string Email { get; private set; }
        public string Subject { get; private set; }
        public string Text { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public Ticket(Guid id, string email, string subject, string text)
        {
            ValidateDomain(id, email, subject, text);
            
            Id = id;
            Email = email;
            Subject = subject;
            Text = text;
            CreatedAt = DateTimeOffset.Now;
            //FullTicketNumber // TODO: Ticket number `ticket-000001`
        }

        private static void ValidateDomain(Guid id, string email, string subject, string text)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException("Invalid ID");
            }
            if (email.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid email");
            }
            if (subject.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid subject");
            }
            if (text.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid text");
            }
        }
    }
}
