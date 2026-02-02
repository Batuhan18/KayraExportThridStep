using KayraExportThridStep.Auth.Application.Dtos;
using KayraExportThridStep.Auth.Application.Services;
using KayraExportThridStep.Auth.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KayraExportThirdStep.Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // Validasyon kontrolleri
            if (dto == null)
            {
                return BadRequest(new { Message = "Geçersiz veri" });
            }

            // Kullanıcı adı kontrolü
            var existingUser = await _userManager.FindByNameAsync(dto.Username);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "Bu kullanıcı adı zaten kullanılıyor" });
            }

            // Email kontrolü
            var existingEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingEmail != null)
            {
                return BadRequest(new { Message = "Bu email adresi zaten kullanılıyor" });
            }

            // YENİ kullanıcı oluştur (ÖNCEKİ DEĞİL!)
            var user = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            // Kullanıcıyı kaydet
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Message = "Kayıt başarısız",
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            // Varsayılan role ekle
            await _userManager.AddToRoleAsync(user, "User");

            return Ok(new
            {
                Message = "Kayıt başarılı",
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // Validasyon
            if (dto == null || string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest(new { Message = "Kullanıcı adı ve şifre gerekli" });
            }

            // Kullanıcıyı bul
            var user = await _userManager.FindByNameAsync(dto.Username);

            if (user == null || !user.IsActive)
            {
                return Unauthorized(new { Message = "Kullanıcı adı veya şifre hatalı" });
            }

            // Şifreyi kontrol et
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { Message = "Kullanıcı adı veya şifre hatalı" });
            }

            // Rolleri al
            var roles = await _userManager.GetRolesAsync(user);

            // Token oluştur
            var token = _jwtTokenService.GenerateToken(user, roles);

            return Ok(new
            {
                Token = token,
                Username = user.UserName,
                Email = user.Email,
                Roles = roles,
                ExpiresAt = DateTime.Now.AddMinutes(60)
            });
        }
    }
}