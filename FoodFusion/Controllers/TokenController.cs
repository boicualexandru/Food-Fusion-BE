using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Authentication.Exceptions;
using Services.Authentication.Models;
using IAuthenticationService = Services.Authentication.IAuthenticationService;

namespace WebApi.Controllers
{
    /// <summary>
    /// Authentication and Registration functionality
    /// </summary>
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
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
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
            when (ex is UserNotFoundException || ex is InvalidPasswordException)
            {
                return BadRequest(ResponseMessages.Authentication.CredentialsNotValid);
            }
        }

        /// <summary>
        /// Register functionality
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns>The the JWT Token for newly created User</returns>
        /// <response code="200">Returns the JWT Token</response>
        /// <response code="400">The Register Model is not valid</response> 
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
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
            catch (EmailAlreadyExistsException)
            {
                var invalidModelState = new ModelStateDictionary();
                invalidModelState.AddModelError(
                    nameof(RegisterModel.Email), 
                    ResponseMessages.Authentication.EmailAlreadyExists);

                return BadRequest(invalidModelState);
            }
        }
    }
}
