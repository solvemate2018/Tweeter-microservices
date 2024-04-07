using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TweetsService.Data;
using TweetsService.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TweetsService.Services
{
    public class TweetsService
    {
        private readonly TweeterServiceDbContext _dbContext;

        public TweetsService(TweeterServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tweet> PostTweet(int userId, string content, List<string> tags)
        {
            var tweet = new Tweet(userId, content, tags);

            tweet.CreatedAt = DateTime.UtcNow;
            tweet.UpdatedAt = DateTime.UtcNow;
            var dbTweet = await _dbContext.Tweets.AddAsync(tweet);

            await _dbContext.SaveChangesAsync();

            return dbTweet.Entity;
        }

        public async Task<Tweet> EditTweet(int tweetId, string content, List<string> tags)
        {
            var dbTweet = await _dbContext.Tweets.FindAsync(tweetId);

            dbTweet.Content = content;
            dbTweet.Tags = tags;
            dbTweet.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return dbTweet;
        }

        public async Task DeleteTweet(int tweetId)
        {
            var dbTweet = await _dbContext.Tweets.FindAsync(tweetId);

            _dbContext.Tweets.Remove(dbTweet);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Tweet>> GetTimeline()
        {
            var tweets = await _dbContext.Tweets.Include(tweet => tweet.Comments).Include(tweet => tweet.Reactions).OrderByDescending(tweet => tweet.CreatedAt).ToListAsync();

            return tweets;
        }

        public async Task<Tweet> GetTweetById(int tweetId)
        {
            var dbTweet = await _dbContext.Tweets.Include(tweet => tweet.Comments).Include(tweet => tweet.Reactions).FirstOrDefaultAsync(t => t.Id == tweetId);

            return dbTweet;
        }

        public async Task<List<Tweet>> GetTweetsByIds(List<int> tweetIds)
        {
            var dbTweets = await _dbContext.Tweets.Include(tweet => tweet.Comments).Include(tweet => tweet.Reactions).Where(t => tweetIds.Contains(t.Id)).ToListAsync();

            return dbTweets;
        }

        public async Task<List<Tweet>> GetTweetsByUserId(List<int> userIds)
        {
            var dbTweets = await _dbContext.Tweets.Include(tweet => tweet.Comments).Include(tweet => tweet.Reactions).Where(t => userIds.Contains(t.UserId)).ToListAsync();

            return dbTweets;
        }
    }
}
