using System;
using Microsoft.AspNetCore.Mvc;
using Services.Authentication;
using IAuthenticationService = Services.Authentication.IAuthenticationService;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public TokenController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // POST: api/Token
        [HttpPost]
        public IActionResult Post([FromBody] LoginModel loginModel)
        {
            try
            {
                var token = _authenticationService.GetToken(loginModel);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid Credentials.");
            }
        }
    }
}
