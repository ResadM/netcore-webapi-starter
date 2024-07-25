using Business.Manager;
using DataModel.HttpResponse;
using DataModel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly AccountManager _accountManager;

        public AccountController(AccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var registerResult = await _accountManager.RegisterUser(registerDto);
            var response = new CustomHttpResponse();
            if (!registerResult.status && registerResult.identityErrors != null)
            {
                foreach (var error in registerResult.identityErrors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            else if (!registerResult.status)
            {
                response.Status = 500;
                response.Message = registerResult.errorMessage;
                return StatusCode(500, response);
            }
            else
            {
                response.Status = 201;
                response.Message = "User created";
                return StatusCode(201, response);
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken cancelToken)
        {
            var userLogin = await _accountManager.LoginUser(loginDto);
            if (userLogin.Errors.Count > 0)
            {
                var response = new CustomHttpResponse();
                response.Status = 401;
                response.Message = userLogin.Errors[0];
                return StatusCode((int)HttpStatusCode.Unauthorized,response);
            }
            return userLogin.Data;
        }

    }
}
