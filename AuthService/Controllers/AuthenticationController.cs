using Microsoft.AspNetCore.Mvc;
using GatewayService.ServiceCommunications;
using GatewayService.Models;
using GatewayService.Services;
using Microsoft.AspNetCore.Authorization;

namespace GatewayService.Controllers
{
    [ApiController]
    
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly EventsEmitter eventsEmitter;

        private readonly IConfiguration _configuration;

        private readonly IUserService _userService;

        public AuthenticationController(IConfiguration configuration, IUserService userService, EventsEmitter eventsEmitter)
        {
            this.eventsEmitter = eventsEmitter;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.AuthenticateUser(login.Email, login.Password);
            if (user == null)
            {
                return BadRequest("Invalid email or password");
            }

            var token = JwtUtils.GenerateJwtToken(user, _configuration);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                SameSite = SameSiteMode.Lax,
                Secure = false
            };

            Response.Cookies.Append("jwt", token, cookieOptions);

            return Ok(new { message = token });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.RegisterUser(register.Email, register.Password, register.FirstName, register.LastName);
            if (user == null)
            {
                return BadRequest("Registration failed");
            }

            eventsEmitter.AnnounceUserRegistered(user.Id, user.FirstName, user.LastName);

            return Ok(new { message = "Registration successful" });

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwt", new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax });
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
