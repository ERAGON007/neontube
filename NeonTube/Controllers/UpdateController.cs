using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NeonTube.Services;
using Telegram.Bot.Types;

namespace NeonTube.Controllers
{
    [Route("api/[controller]")]
    public class UpdateController : Controller
    {
        private readonly IUpdateService _updateService;

        public UpdateController(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        // POST api/update
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            await _updateService.HandleUpdate(update);
            return Ok();
        }
    }
}
