using InventoryProducer.Models;
using InventoryProducer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace InventoryProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ProducerService _producerService;

        public InventoryController(ProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpPost]
        public async Task<IActionResult> Produce([FromBody] InventoryUpdateRequest request)
        {
            var message = JsonSerializer.Serialize(request);
            await _producerService.ProduceAsync("inventory-updates", message);
            return Ok("Inventory Updated Successfully...");
        }
    }
}
