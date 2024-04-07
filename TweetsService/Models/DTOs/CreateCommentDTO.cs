namespace TweetsService.Models.DTOs
{
    public class CreateCommentDTO
    {
        public int TweetId { get; set; }
        public string Content { get; set; }
    }
}
