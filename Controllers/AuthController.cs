using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tasks.ServiceLayer;
using Tasks.CommonUtility.Models;

namespace Tasks.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ITasksSL _iTasksSL;
        
        public AuthController(ITasksSL iTasksSL)
        {
            _iTasksSL = iTasksSL;
        }
        
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            RegisterUserResponse response = new RegisterUserResponse();
            try
            {
                response = await _iTasksSL.RegisterUser(request);

                if (!response.IsSuccess)
                {
                    return BadRequest(new { IsSucess = response.IsSuccess, Message = response.Message });
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return BadRequest(new { IsSucess = response.IsSuccess, Message = response.Message });
            }

            return Ok(new { IsSucess = response.IsSuccess, Message = response.Message });
        }


        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginUserRequest request)
        {
            LoginUserResponse response = new LoginUserResponse();

            try
            {
                response = await _iTasksSL.LoginUser(request);

                if (response.IsSuccess)
                {
                    //create claims after signIn
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email , response.user.UserName),
                        new Claim(ClaimTypes.PrimarySid, Convert.ToString(response.user.UserId)),
                        new Claim(ClaimTypes.Role , response.user.UserRole)
                    };

                    // create identity of claims
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // using this line cookie will get saved in browser
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                }
                else
                {
                    return BadRequest(new { IsSucess = response.IsSuccess, Message = response.Message });
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return BadRequest(new { IsSucess = response.IsSuccess, Message = response.Message });
            }

            return Ok(new { IsSucess = response.IsSuccess, Message = response.Message });
        }

    }
}
