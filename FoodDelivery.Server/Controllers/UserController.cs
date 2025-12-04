using FoodDelivery.Server.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController (UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register (RegisterRequest req)
        {
            try
            {
                var uid = await _userService.RegisterUser(
                    req.Name,
                    req.Email,
                    req.Password
                );
                return Ok(new { message = "User registered Successfully", uid = uid });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginRequest req)
        {
            var result = await _userService.LoginUser(req.Email, req.Password);
            if(result == null){
                return BadRequest(new {message = "Invalid Email and Password"});
            }
            return Ok(result);
        }
    }

    public class RegisterRequest
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = ""; 
    }

    public class LoginRequest
    {
        public string Email { get; set;} = "";
        public string Password {get; set;} = "";
    }
}