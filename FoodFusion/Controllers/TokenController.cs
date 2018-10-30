using System;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Login functionality
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>The newly created JWT Token</returns>
        /// <response code="200">Returns the newly created JWT Token</response>
        /// <response code="400">The credentials are not valid</response> 
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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
