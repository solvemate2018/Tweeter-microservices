using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using ProfileService.Models;
using ProfileService.ServiceCommunications;
using System.Security.Claims;
using TweetsService.ServiceCommunications;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProfileService.Controllers
{
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly Services.ProfileService profileService;

        private readonly EventsEmitter eventsEmitter;

        private readonly EventsReceiver eventsReceiver;

        public ProfilesController(Services.ProfileService profileService, EventsEmitter eventsEmitter, EventsReceiver eventsReceiver)
        {
            this.profileService = profileService;
            this.eventsEmitter = eventsEmitter;
            this.eventsReceiver = eventsReceiver;
        }

        [HttpGet]
        [Route("/cors")]
        public async Task<IActionResult> Cors()
        {
            return Ok();
        }

        [HttpGet]
        [Route("/profiles")]
        [Authorize]
        public async Task<IActionResult> RequestUserInfo()
        {
            var user = await profileService.GetUserById(RetrieveAuthenticatedUserID());
            return Ok(user);
        }

        [HttpGet]
        [Route("/profiles/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile(int id)
        {
            var user = await profileService.GetUserById(id);
            return Ok(user);
        }

        [HttpPut]
        [Route("/profiles")]
        [Authorize]
        public async Task<IActionResult> UpdateBio([FromBody] UpdateBioModel profileBio)
        {
            var updatedUser = await profileService.UpdateProfileBio(RetrieveAuthenticatedUserID(), profileBio.UpdatedBio);
            return Ok(updatedUser);
        }

        [HttpPost]
        [Route("/profiles/followers/{foollowId}")]
        [Authorize] 
        public async Task<IActionResult> Follow(int foollowId)
        {
            await profileService.Follow(RetrieveAuthenticatedUserID(), foollowId);
            //eventsEmitter.AnnounceUserWasFollowed(RetrieveAuthenticatedUserID(), foollowId);
            return Ok(ModelState);
        }

        [HttpDelete]
        [Route("/profiles/followers/{unfollowId}")]
        [Authorize]
        public async Task<IActionResult> Unfollow(int unfollowId)
        {
            await profileService.Unfollow(RetrieveAuthenticatedUserID(), unfollowId);
            //eventsEmitter.AnnounceUserWasUnfollowed(RetrieveAuthenticatedUserID(), unfollowId);
            return Ok(ModelState);
        }

        [HttpGet]
        [Route("/profiles/followers/{id}")]
        [Authorize]
        public async Task<IActionResult> GetFollowers(int id)
        {
            var followers = await profileService.GetFollowers(id);
            return Ok(followers);
        }

        [HttpGet]
        [Route("/profiles/following/{id}")]
        [Authorize]
        public async Task<IActionResult> GetFollowing(int id)
        {
            var following = await profileService.GetFollowing(id);
            return Ok(following);
        }


        private int RetrieveAuthenticatedUserID()
        {
            var user = HttpContext.User;
            return Int32.Parse(user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
        }
    }
}
