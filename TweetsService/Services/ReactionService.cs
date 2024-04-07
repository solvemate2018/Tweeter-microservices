using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TweetsService.Data;
using TweetsService.Models;

namespace TweetsService.Services
{
    public class ReactionService
    {
        private readonly TweeterServiceDbContext _dbContext;

        public ReactionService(TweeterServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Reaction> ReactOnTweet(int userId, int tweetId, ReactionType reactionType)
        {
            Reaction reaction = new Reaction(tweetId, userId, reactionType);

            reaction.CreatedAt = DateTime.UtcNow;

            reaction.Id = _dbContext.Reactions.Max(r => r.Id) + 1;

            var dbReaction = await _dbContext.Reactions.AddAsync(reaction);

            await _dbContext.SaveChangesAsync();

            return dbReaction.Entity;
        }

        public async Task UnreactATweet(int reactionId, int tweetId)
        {
            var dbReaction = await _dbContext.Reactions.Where(r => r.Id == reactionId && r.TweetId == tweetId)
                          .FirstOrDefaultAsync();

            _dbContext.Reactions.Remove(dbReaction);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Reaction>> GetReactionsByTweetId(int tweetId)
        {
            var reactions = await _dbContext.Reactions.Where(reaction => reaction.TweetId.Equals(tweetId)).ToListAsync();

            return reactions;
        }
    }
}
