using Microsoft.AspNetCore.Mvc;
using PayIP.Model;
using PayIP.Services.Interfaces;

namespace PayIP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationService _notificationService;

        public NotificationController(ILogger<NotificationController> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Add([FromBody] List<NotificationRequest> notificationRequest)
        {
            _logger.LogInformation($"Iniciando o processo de envio de notifica��o para {notificationRequest.Count} usuarios");

            if (notificationRequest.Any())
            {
                return Ok(await _notificationService.SendNotifications(notificationRequest));
            }

            _logger.LogError("Requisi��o n�o pode estar vazia");
            return BadRequest();

        }

    }
}
