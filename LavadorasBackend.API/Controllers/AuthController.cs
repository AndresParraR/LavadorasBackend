using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Lavadoras.Application.Services.Auth;
using Lavadoras.Application.Common.JWT;
using Lavadoras.Domain.Entities;
using Lavadoras.API.Results;
using Lavadoras.API.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lavadoras.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtToken _jwtToken;

        public AuthController(IAuthService authService, IJwtToken jwtToken) {
            _authService = authService;
            _jwtToken = jwtToken;
        }

        [HttpPost("Login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GenericResult<User>))]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (ModelState.IsValid)
            {

                var user = await _authService.Login(request.Username, request.Password);
                if (user != null)
                {
                    var result = new GenericResult<User>
                    {
                        Success = true,
                        Response = user
                    };
                    return Ok(result);

                }

                return Unauthorized();
            }

            return BadRequest(ModelState);
        }

        [HttpPost("Login2FA")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GenericResult<User>))]
        public async Task<IActionResult> Login2FA([FromBody] Login2FARequest request)
        {
            if (ModelState.IsValid)
            {

                var user = await _authService.Login2FA(request.Username, request.CodeVerification);
                if (user != null)
                {
                    var userWithToken = await _authService.GenerateToken(user);
                    var result = new GenericResult<User>
                    {
                        Success = true,
                        Response = userWithToken
                    };
                    return Ok(result);

                }

                return Unauthorized();
            }

            return BadRequest(ModelState);
        }

        [HttpPost("ForgotPassword")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        public async Task<IActionResult> ForgotPassword(string username)
        {
            if (ModelState.IsValid)
            {

                var result = await _authService.ForgotPassword(username);
                return Ok(true);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("ResendCodeVerification")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        public async Task<IActionResult> ResendCodeVerification(string username)
        {
            if (ModelState.IsValid)
            {

                var result = await _authService.ResendCodeVerification(username);
                return Ok(true);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("SignOut")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        public async Task<IActionResult> SignOut()
        {
            if (ModelState.IsValid)
            {
                var token = HttpContext.GetTokenAsync("Bearer", "access_token");
                var jToken = _jwtToken.decodeJwtToken(token.Result.ToString());
                var currentUserId = int.Parse(jToken.Claims.First(claim => claim.Type == "sub").Value);
                var result = await _authService.SignOut(currentUserId);
                return Ok(true);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.ChangePassword(request.Username, request.CurrentPassword, request.NewPassword);
                return Ok(true);
            }

            return BadRequest(ModelState);
        }
    }
}
