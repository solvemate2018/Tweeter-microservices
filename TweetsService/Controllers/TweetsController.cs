using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TweetsService.Models;
using TweetsService.Models.DTOs;

namespace TweetsService.Controllers
{
    [ApiController]
    [Route("tweet")]
    public class TweetsController : ControllerBase
    {
        private readonly Services.TweetsService tweetsService;

        public TweetsController(Services.TweetsService tweetsService)
        {
            this.tweetsService = tweetsService;
        }

        [HttpGet]
        [Route("/cors")]
        public async Task<IActionResult> Cors()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostTweet([FromBody] CreateTweetDTO tweet)
        {
            var createdTweet = await tweetsService.PostTweet(RetrieveAuthenticatedUserID(), tweet.Content, tweet.Tags);
            return Ok(createdTweet);
        }

        [HttpPut]
        [Route("{tweetId}")]
        public async Task<IActionResult> UpdateTweet(int tweetId, [FromBody] CreateTweetDTO tweet)
        {
            var updatedTweet = await tweetsService.EditTweet(tweetId, tweet.Content, tweet.Tags);
            return Ok(updatedTweet);
        }

        [HttpDelete]
        [Route("{tweetId}")]
        public async Task<IActionResult> DeleteTweet(int tweetId)
        {
            await tweetsService.DeleteTweet(tweetId);
            return Ok(ModelState);
        }

        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> GetUserTweets()
        {
            var searchedTweet = await tweetsService.GetTweetsByUserId(new List<int>() { RetrieveAuthenticatedUserID() });
            return Ok(searchedTweet);
        }

        [HttpGet]
        [Route("timeline")]
        public async Task<IActionResult> GetTimeline()
        {
            var timelineTweets = await tweetsService.GetTimeline();
            return Ok(timelineTweets);
        }

        [HttpGet]
        [Route("{tweetId}")]
        public async Task<IActionResult> GetTweetById(int tweetId)
        {
            var searchedTweet = await tweetsService.GetTweetById(tweetId);
            return Ok(searchedTweet);
        }

        [HttpPost]
        [Route("find-by-ids")]
        public async Task<IActionResult> GetTweetsByIds([FromBody]List<int> tweetIds)
        {
            var searchedTweets = await tweetsService.GetTweetsByIds(tweetIds);
            return Ok(searchedTweets);
        }

        [HttpPost]
        [Route("find-by-user-ids")]
        public async Task<IActionResult> GetTweetsByUserIds([FromBody] List<int> userIds)
        {
            var searchedTweets = await tweetsService.GetTweetsByUserId(userIds);
            return Ok(searchedTweets);
        }

        private int RetrieveAuthenticatedUserID()
        {
            var user = HttpContext.User;
            return Int32.Parse(user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
        }
    }
}
