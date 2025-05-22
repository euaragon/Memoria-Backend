using Microsoft.AspNetCore.Mvc;
using MemoriaAPI.Services;

namespace MemoriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfiguracionController : ControllerBase
    {
        private readonly IConfiguracionService _configuracionService;

        public ConfiguracionController(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        [HttpGet]
        public IActionResult GetConfiguracion()
        {
            var baseUrl = _configuracionService.GetBaseUrl();
            return Ok(new { BaseUrl = baseUrl });
        }
    }
}
