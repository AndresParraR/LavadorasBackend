using Microsoft.AspNetCore.Mvc;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Lavadoras.Application.Services.Operator;
using Lavadoras.Application.Common.JWT;
using Lavadoras.API.Results;
using Lavadoras.Domain.Entities;
using Lavadoras.API.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lavadoras.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OperatorController : ControllerBase
{
    private readonly IOperatorService _operatorService;
    private readonly IMapper _mapper;
    private readonly IJwtToken _jwtToken;

    public OperatorController(IOperatorService operatorService, IMapper mapper, IJwtToken jwtToken) {
        _operatorService = operatorService;
        _mapper = mapper;
        _jwtToken = jwtToken;
    }

    [HttpPost("Create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GenericResult<User>))]
    public async Task<IActionResult> Create([FromBody] CreateOperatorRequest request)
    {
        if (ModelState.IsValid)
        {
            var token = HttpContext.GetTokenAsync("Bearer", "access_token");
            var jToken = _jwtToken.decodeJwtToken(token.Result.ToString());
            var currentUserId = int.Parse(jToken.Claims.First(claim => claim.Type == "sub").Value);
            var user = _mapper.Map<User>(request);
            var newUser = await _operatorService.Create(user, currentUserId);
            if (newUser != null)
            {
                var result = new GenericResult<User>
                {
                    Success = true,
                    Response = newUser
                };
                return Ok(result);

            }

            return Unauthorized();
        }

        return BadRequest(ModelState);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GenericResult<List<User>>))]
    public async Task<IActionResult> Get()
    {
        var operators = await _operatorService.GetAll();
        var result = new GenericResult<List<User>>
        {
            Success = true,
            Response = operators
        };
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GenericResult<User>))]
    public async Task<IActionResult> Update(int id, [FromBody] CreateOperatorRequest request)
    {
        if (ModelState.IsValid)
        {
            var user = _mapper.Map<User>(request);
            var token = HttpContext.GetTokenAsync("Bearer", "access_token");
            var jToken = _jwtToken.decodeJwtToken(token.Result.ToString());
            var currentUserId = int.Parse(jToken.Claims.First(claim => claim.Type == "sub").Value);
            var newOperator  = await _operatorService.Update(id, user, currentUserId);
            var result = new GenericResult<User>
            {
                Success = true,
                Response = newOperator
            };
            return Ok(result);
        }

        return BadRequest(ModelState);
    }
}
