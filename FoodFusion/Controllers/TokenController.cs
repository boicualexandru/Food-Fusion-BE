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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = _authenticationService.GetToken(loginModel);
                return Ok(token);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Login functionality
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns>The newly created JWT Token</returns>
        /// <response code="200">Returns the newly created JWT Token</response>
        /// <response code="400">The credentials are not valid</response> 
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = _authenticationService.RegisterAndGetToken(registerModel);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
