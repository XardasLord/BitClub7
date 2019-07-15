using System;
using BC7.Common.Extensions;
using BC7.Domain.Enums;
using BC7.Infrastructure.CustomExceptions;

namespace BC7.Domain
{
    public class Article
    {
        public Guid Id { get; private set; }
        public Guid CreatorId { get; private set; }
        public virtual UserAccountData Creator { get; private set; }
        public string Title { get; private set; }
        public string Text { get; private set; }
        public ArticleType ArticleType { get; set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public Article(Guid id, Guid creatorId, string title, string text, ArticleType articleType)
        {
            ValidateDomain(id, creatorId, title, text);

            Id = id;
            CreatorId = creatorId;
            Title = title;
            Text = text;
            ArticleType = articleType;
            CreatedAt = DateTimeOffset.Now;
        }

        private static void ValidateDomain(Guid id, Guid creatorId, string title, string text)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException("Invalid Article ID.");
            }
            if (creatorId == Guid.Empty)
            {
                throw new DomainException("Invalid article creator ID");
            }
            if (title.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid article title");
            }
            if (text.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid article text");
            }
        }

        public void UpdateInformation(string newTitle, string newText)
        {
            if (newTitle.IsNullOrWhiteSpace())
            {
                throw new DomainException("Article's title cannot be null or empty");
            }
            if (newText.IsNullOrWhiteSpace())
            {
                throw new DomainException("Article's text cannot be null or empty");
            }

            Title = newTitle;
            Text = newText;
            // TODO: ModifiedAt...
        }
    }
}
