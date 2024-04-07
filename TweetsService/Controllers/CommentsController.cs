using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TweetsService.Models;
using TweetsService.Models.DTOs;
using TweetsService.Services;

namespace TweetsService.Controllers
{
    [ApiController]
    [Route("comment")]
    public class CommentsController : ControllerBase
    {
        private readonly CommentService commentService;

        public CommentsController(CommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] CreateCommentDTO comment)
        {
            var createdComment = await commentService.CommentTweet(RetrieveAuthenticatedUserID(), comment.TweetId, comment.Content);
            return Ok(createdComment);
        }

        [HttpPut]
        [Route("{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] CreateCommentDTO comment)
        {
            var updatedComment = await commentService.EditComment(commentId, comment.Content);
            return Ok(updatedComment);
        }

        [HttpDelete]
        [Route("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            await commentService.DeleteComment(commentId);
            return Ok(ModelState);
        }

        [HttpGet]
        [Route("find-by-tweet-id/{tweetId}")]
        public async Task<IActionResult> GetCommentsByTweetId(int tweetId)
        {
            var searchedComments = await commentService.GetCommentsByTweetId(tweetId);
            return Ok(searchedComments);
        }

        private int RetrieveAuthenticatedUserID()
        {
            var user = HttpContext.User;
            return Int32.Parse(user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
        }
    }
}
