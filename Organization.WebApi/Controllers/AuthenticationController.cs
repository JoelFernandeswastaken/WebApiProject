using ErrorOr;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using Organization.Application.Common.DTO.Request;
using Organization.Application.UserModule.Commands.RefreshToken;
using Organization.Application.UserModule.Commands.RegisterUser;
using Organization.Application.UserModule.Queries.GetUserByEmail;
using Organization.Application.UserModule.Queries.LoginUser;
using System.ComponentModel.DataAnnotations;

namespace Organization.Presentation.Api.Controllers
{
    [ApiController]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [AllowAnonymous]
    public class AuthenticationController : BaseAPIController
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        public AuthenticationController(ISender sender, IMapper mapper)
        {
            _sender = sender;   
            _mapper = mapper;   
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            var registerUserCommand = _mapper.Map<RegisterUserCommand>(request);
            var result = await _sender.Send(registerUserCommand);
            return result.Match(
                p => Ok(p),
                error => Problem(error)); 
        }
        [HttpPost]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(GetUserByEmailRequest request)
        {
            var getUserByEmailQuery = _mapper.Map<GetUserByEmailQuery>(request);
            var result = await _sender.Send(getUserByEmailQuery);
            return result.Match(
                p => Ok(p),
                error => Problem(error));
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser(LoginUserRequest request)
        {
            var LoginUserQuery = _mapper.Map<LoginUserQuery>(request);
            var result = await _sender.Send(LoginUserQuery);
            return result.Match(
                p => Ok(p),
                errors => Problem(errors)
            );
        }
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var refreshTokenCommand = _mapper.Map<RefreshTokenCommand>(request);
            var result = await _sender.Send(refreshTokenCommand);
            return result.Match(
                p => Ok(p),
                errors => Problem(errors)
            );
        }
    }
}
