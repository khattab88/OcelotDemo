using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authSvc;

        public AuthController(IAuthService authSvc)
        {
            _authSvc = authSvc;
        }

        [HttpPost(Name = "authenticate")]
        public IActionResult Authenticate([FromBody] UserCredentials credentials)
        {
            var jwt = _authSvc.Authenticate(credentials.UserName, credentials.Password);

            if (string.IsNullOrEmpty(jwt))
            {
                return Unauthorized();
            }

            return Ok(jwt);
        }
    }
}
