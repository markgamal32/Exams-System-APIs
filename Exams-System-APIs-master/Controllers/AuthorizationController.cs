using LMSAPIProject.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LMSAPIProject.DTO;
using System.Security.Claims;
using LMSAPIProject.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Internal;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using LMSAPIProject.ViewModel;

namespace LMSAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private IUserRepository userRepository;
        public AuthorizationController(IUserRepository userRepository)
        {

            this.userRepository = userRepository;

        }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(logInDto userDTO)
        {
            if (userDTO != null && userRepository.Find(userDTO.email, userDTO.password))
            {
                try
                {
                    // generate token
                    var user = userRepository.GetUserByEmailAndPassword(userDTO.email, userDTO.password);
                    string userRole = (user.IsAdmin) ? "admin" : "student";
                    var userData = new List<Claim>
                    {
                        new Claim(CustomClaims.Email, userDTO.email),
                        new Claim(CustomClaims.Name, user.Name),
                        new Claim(CustomClaims.Role , userRole)
                    };

                    var sKey = "welcome to my account in angular exam project";
                    var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(sKey));
                    var signCre = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        claims: userData,
                        signingCredentials: signCre,
                        expires: DateTime.Now.AddDays(1));

                    var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

                    var response = new LoginResponseViewModel()
                    {
                        token = stringToken
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate user data and perform business logic

            User user = new() { Email = userDTO.email, Password = userDTO.password, Name = userDTO.Name, Age = userDTO.age, };

            userRepository.Insert(user);

            return Ok(new { message = "success" });
        }

        //[HttpPost]
        //[Authorize]
        //public IActionResult SignOut()
        //{
        //    HttpContext.Authentication.SignOutAsync("MyAuthenticationScheme");
        //    return RedirectToAction("Index", "Home");
        //}

    }

}
