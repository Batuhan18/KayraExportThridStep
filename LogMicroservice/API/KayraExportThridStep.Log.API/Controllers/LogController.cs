using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KayraExportThirdStep.Log.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private readonly IConfiguration _configuration;

        public LogController(ILogger<LogController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("info")]
        public IActionResult LogInfo([FromBody] LogRequest request)
        {
            _logger.LogInformation("{ServisAdi} - {Mesaj} {@Ozellikler}",
                request.ServiceName, request.Message, request.Properties);
            return Ok(new { Durum = "Kaydedildi", Seviye = "INFO" });
        }

        [HttpPost("warning")]
        public IActionResult LogWarning([FromBody] LogRequest request)
        {
            _logger.LogWarning("{ServisAdi} - {Mesaj} {@Ozellikler}",
                request.ServiceName, request.Message, request.Properties);
            return Ok(new { Durum = "Kaydedildi", Seviye = "WARNING" });
        }

        [HttpPost("error")]
        public IActionResult LogError([FromBody] LogRequest request)
        {
            _logger.LogError("{ServisAdi} - {Mesaj} - {Hata} {@Ozellikler}",
                request.ServiceName, request.Message, request.Exception, request.Properties);
            return Ok(new { Durum = "Kaydedildi", Seviye = "ERROR" });
        }

        [HttpPost("critical")]
        public IActionResult LogCritical([FromBody] LogRequest request)
        {
            _logger.LogCritical("{ServisAdi} - {Mesaj} - {Hata} {@Ozellikler}",
                request.ServiceName, request.Message, request.Exception, request.Properties);
            return Ok(new { Durum = "Kaydedildi", Seviye = "CRITICAL" });
        }

        //[HttpGet("test")]
        //public IActionResult TestLogs()
        //{
        //    _logger.LogInformation("Bu bir bilgilendirme logudur - Test edildi: {Zaman}", DateTime.Now);
        //    _logger.LogWarning("Bu bir uyarı logudur - Test edildi: {Zaman}", DateTime.Now);
        //    _logger.LogError("Bu bir hata logudur - Test edildi: {Zaman}", DateTime.Now);

        //    try
        //    {
        //        throw new Exception("Test hatası");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Test hatası yakalandı");
        //    }

        //    return Ok(new
        //    {
        //        Mesaj = "Test logları oluşturuldu",
        //        Zaman = DateTime.Now
        //    });
        //}

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                Durum = "Sağlıklı",
                Servis = "Log Mikroservisi",
                Zaman = DateTime.Now
            });
        }
    }

    public class LogRequest
    {
        public string ServiceName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
        public Dictionary<string, object>? Properties { get; set; }
    }
}