using Asp.Versioning;
using Ecommerce.Core.DTOs.Auth;
using Ecommerce.Core.Interfaces.Services;
using Ecommerce.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController(
        IValidator<RegisterDto> registerValidator,
        IValidator<LoginDto> loginValidator,
        UserManager<ApplicationUser> userManager,
        IAuthService authService
        ) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var validationResult = await registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => $"{x.PropertyName} => {x.ErrorMessage}").ToList();
                return BadRequest(new { Errors = errors });
            }

            var result = await authService.Register(registerDto);
            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Message });
            return Ok(result.Data);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var validationResult = await loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => $"{x.PropertyName} => {x.ErrorMessage}").ToList();
                return BadRequest(new { Errors = errors });
            }

            var result = await authService.Login(loginDto);
            if (result.IsSuccess)
                return Ok(result.Data);

            return Unauthorized(new { Error = result.Message });
        }
    }
}