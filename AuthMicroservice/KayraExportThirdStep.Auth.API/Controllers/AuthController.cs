using KayraExportThridStep.Auth.Application.Dtos;
using KayraExportThridStep.Auth.Application.Interfaces;
using KayraExportThridStep.Auth.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KayraExportThirdStep.Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly IUserService _userService;

        public AuthController(JwtTokenService jwtTokenService, IUserService userService)
        {
            _jwtTokenService = jwtTokenService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userService.ValidateUserAsync(dto.Username, dto.Password);

            if (user == null)
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı");
            }

            var token = _jwtTokenService.GenerateToken(user);

            return Ok(new
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                ExpiresAt = DateTime.Now.AddMinutes(60)
            });
        }
    }
}