using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository,ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            var user = await userRepository.AuthenticateAsync(loginRequest.Username, loginRequest.Password);

            //check if user is authenticated
            if(user is null)
            {
                return BadRequest("Username or Password is incorrect");
            }

            //Generate JWT token
            var token = await tokenHandler.CreateTokenAsync(user);
            return Ok(token);



        }
    }
}
