using Microsoft.Extensions.Configuration;

namespace MemoriaAPI.Services
{
    public interface IConfiguracionService
    {
        string GetBaseUrl();
    }

    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfiguration _configuration;

        public ConfiguracionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetBaseUrl()
        {
            return _configuration["ApiConfiguration:BaseUrl"];
        }
    }
}