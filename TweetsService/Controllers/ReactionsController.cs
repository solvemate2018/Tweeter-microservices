using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TweetsService.Models;
using TweetsService.Models.DTOs;
using TweetsService.Services;

namespace TweetsService.Controllers
{
    [ApiController]
    [Route("reaction")]
    public class ReactionsController : ControllerBase
    {
        private readonly ReactionService reactionService;

        public ReactionsController(ReactionService reactionService)
        {
            this.reactionService = reactionService;
        }

        [HttpPost]
        public async Task<IActionResult> PostReaction([FromBody] CreateReactionDTO reaction)
        {
            var createdReaction = await reactionService.ReactOnTweet(RetrieveAuthenticatedUserID(), reaction.TweetId, ReactionType.Like);
            return Ok(createdReaction);
        }

        [HttpDelete]
        [Route("{reactionId}/tweet/{tweetId}")]
        public async Task<IActionResult> DeleteReaction(int reactionId, int tweetId)
        {
            await reactionService.UnreactATweet(reactionId, tweetId);
            return Ok(ModelState);
        }

        [HttpGet]
        [Route("find-by-tweet-id/{tweetId}")]
        public async Task<IActionResult> GetReactionsByTweetId(int tweetId)
        {
            var searchedReactions = await reactionService.GetReactionsByTweetId(tweetId);
            return Ok(searchedReactions);
        }

        private int RetrieveAuthenticatedUserID()
        {
            var user = HttpContext.User;
            return Int32.Parse(user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
        }
    }
}
