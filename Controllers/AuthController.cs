using Microsoft.AspNetCore.Mvc;
using MemoriaAPI.Models.DTO;
using MemoriaAPI.Service;


namespace MemoriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            _logger.LogInformation("🔐 Intento de login para el usuario '{Username}'", request.NombreUsuario);

            var token = _authService.Authenticate(request.NombreUsuario, request.Contraseña);

            if (token == null)
            {
                _logger.LogWarning("❌ Login fallido para el usuario '{Username}'", request.NombreUsuario);
                return Unauthorized("Credenciales inválidas");
            }

            _logger.LogInformation("✅ Login exitoso para el usuario '{Username}'", request.NombreUsuario);

            return Ok(new { token });
        }
    }
}
