using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetsService.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int TweetId { get; set; }

        [Required]
        [Comment("Leading to Profile Service")]
        public int UserId {  get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Comment(int tweetId, int userId, string content)
        {
            TweetId = tweetId;
            UserId = userId;
            Content = content;
        }
    }
}
