using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetsService.Models
{
    public class Reaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public int TweetId { get; set; }

        [Required]
        [Comment("Leading to Profile Service")]
        [Key]
        public int UserId { get; set; }

        public ReactionType ReactionType { get; set; }

        public DateTime CreatedAt { get; set; }

        public Reaction(int tweetId, int userId, ReactionType reactionType)
        {
            TweetId = tweetId;
            UserId = userId;
            ReactionType = reactionType;
        }
    }
}
