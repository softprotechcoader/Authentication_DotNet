using LoginRegistrationAPI.Models.DTO;
using LoginRegistrationAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace LoginRegistrationAPI.Controllers
{
    [Route("api/[controller]")] //Specifies the route template for the controller. [controller] is a placeholder for the controller’s name, which in this case is AuthController.
    [ApiController]//Indicates that this class is an API controller, which provides automatic model validation and other features.
    public class AuthController : ControllerBase//Defines a public class named AuthController that inherits from ControllerBase, which provides basic functionalities for handling HTTP requests.
    {
        private readonly UserManager<IdentityUser> userManager;//Declares a private, read-only field to hold the UserManager instance.
        private readonly ItokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ItokenRepository tokenRepository)// Constructor that initializes the userManager field with the provided UserManager instance.
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        //POST:/api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registerRequestDto)//Defines an asynchronous method that handles user registration. It takes a RegistrationRequestDTO object from the request body
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);//Attempts to create the user with the specified password.
            if (identityResult.Succeeded)//Checks if the user creation was successful.
            {
                // Add roles to this User
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was Registered! Please Login");
                    }
                }
                return BadRequest("Something went wrong");
            }
            return BadRequest("User registration failed");
        }

        //POST:/api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDto)//Defines an asynchronous method that handles user login. It takes a LoginRequestDTO object from the request body.
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);//Attempts to find a user by their email.
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    //Get Roles For this user
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //create Token

                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                }
            }
            return BadRequest("Username or password incorrect");
        }

        //POST:/api/Auth/Logout
        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()//Defines a method that handles user logout.
        {
            // Invalidate the token here (implementation depends on your token handling logic)
            return Ok("User logged out successfully");
        }
    }
}

