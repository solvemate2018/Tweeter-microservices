using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TweetsService.Data;
using TweetsService.Models;

namespace TweetsService.Services
{
    public class CommentService
    {
        private readonly TweeterServiceDbContext _dbContext;

        public CommentService(TweeterServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Comment> CommentTweet(int userId, int tweetId, string content)
        {
            var dbTweet = await _dbContext.Tweets.FindAsync(tweetId);
            Comment comment = new Comment(tweetId, userId, content);

            comment.UpdatedAt = DateTime.UtcNow;
            comment.CreatedAt = DateTime.UtcNow;

            dbTweet.Comments.Add(comment);
            var dbComment = await _dbContext.Comments.AddAsync(comment);

            await _dbContext.SaveChangesAsync();

            return dbComment.Entity;
        }

        public async Task DeleteComment(int commentId)
        {
            var dbComment = await _dbContext.Comments.FindAsync(commentId);

            _dbContext.Comments.Remove(dbComment);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Comment> EditComment(int commentId, string content)
        {
            var dbComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            dbComment.Content = content;
            dbComment.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return dbComment;
        }

        public async Task<List<Comment>> GetCommentsByTweetId(int tweetId)
        {
            var comments = await _dbContext.Comments.Where(comment => comment.TweetId.Equals(tweetId)).ToListAsync();

            return comments;
        }
    }
}
