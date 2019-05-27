using System;

namespace BC7.Business.Models
{
    public class ArticleModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Creator { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
