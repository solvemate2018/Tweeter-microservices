using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TweetsService.Models
{
    [Table("Tweets")]
    public class Tweet
    {
        public int Id { get; set; }

        [Required]
        [Comment("Leading to Profile Service")]
        public int UserId { get; set; }

        [Required]
        public string Content { get; set; }

        public List<string> Tags { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Reaction> Reactions { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Tweet(int userId, string content, List<string> tags)
        {
            UserId = userId;
            Content = content;
            Tags = tags;
            Comments = new List<Comment>();
            Reactions = new List<Reaction>();
        }
    }
}
