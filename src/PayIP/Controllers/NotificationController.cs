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
        public async Task<IActionResult> Add([FromBody] NotificationRequestModel model)
        {
            _logger.LogInformation($"Iniciando o processo de envio de notificação para {model.Mensagens.Count} usuarios");

            if (model.Mensagens.Any())
            {
                return Ok(await _notificationService.SendNotifications(model.Mensagens));
            }

            _logger.LogError("Requisição não pode estar vazia");
            return BadRequest();

        }

    }
}
