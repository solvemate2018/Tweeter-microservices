using Microsoft.AspNetCore.Mvc;

namespace ProfileService.Controllers
{
    [ApiController]
    [Route("profiles")]
    public class ProfilesController : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public IActionResult Register()
        {
            return Ok(ModelState);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login()
        {
            return Ok(ModelState);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateInfo()
        {
            return Ok(ModelState);
        }

        [HttpPost]
        [Route("follow/{id}")]
        public IActionResult Follow()
        {
            return Ok(ModelState);
        }

        [HttpDelete]
        [Route("unfollow/{id}")]
        public IActionResult Unfollow()
        {
            return Ok(ModelState);
        }

        [HttpGet]
        [Route("followers")]
        public IActionResult GetFollowers()
        {
            return Ok(ModelState);
        }

        [HttpGet]
        [Route("following")]
        public IActionResult GetFollowing()
        {
            return Ok(ModelState);
        }
    }
}
