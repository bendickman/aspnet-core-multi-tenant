using Microsoft.AspNetCore.Mvc;
using MultiTenant.Api.Requests;
using MultiTenant.Api.Responses;
using MultiTenant.Core.Interfaces;

namespace MultiTenant.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/identity/")]
    [Produces("application/json")]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(
            IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(AuthFailedResponse), 400)]
        [Route("register")]
        public async Task<IActionResult> Register(
            [FromBody] UserRegistrationRequest request)
        {
            var authResponse = await _identityService
                .RegisterAsync(request.Email, request.Password);

            if (!authResponse.IsSuccess)
            {
                return BadRequest(AuthFailedResponse
                    .Error(authResponse.Errors));
            }

            return Ok(AuthSuccessResponse
                .Success(authResponse.Token, authResponse.RefreshToken));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(AuthFailedResponse), 400)]
        [Route("login")]
        public async Task<IActionResult> Login(
            [FromBody] UserRegistrationRequest request)
        {
            var authResponse = await _identityService
                .LoginAsync(request.Email, request.Password);

            if (!authResponse.IsSuccess)
            {
                return BadRequest(AuthFailedResponse
                    .Error(authResponse.Errors));
            }

            return Ok(AuthSuccessResponse
                .Success(authResponse.Token, authResponse.RefreshToken));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(AuthFailedResponse), 400)]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshTokenRequest request)
        {
            var authResponse = await _identityService
                .RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authResponse.IsSuccess)
            {
                return BadRequest(AuthFailedResponse
                    .Error(authResponse.Errors));
            }

            return Ok(AuthSuccessResponse
                .Success(authResponse.Token, authResponse.RefreshToken));
        }
    }
}
