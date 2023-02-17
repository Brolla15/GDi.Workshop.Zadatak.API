using GDi.Workshop.Zadatak.BM.Models.Requests;
using GDi.Workshop.Zadatak.BM.Models.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GDi.Workshop.Zadatak.BM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRealTimeController : ControllerBase
    {
        private IHubContext<AppHub> _hub;
        

        public SignalRealTimeController(IHubContext<AppHub> hub)
        {
            _hub = hub;
        }

        [HttpPost("notify")]
        public async Task<ActionResult> Notify([FromBody] NotifyRequest request)
        {
            await _hub.Clients.All.SendAsync("camundaMessageHub", request);
            return this.Ok();
        }

        

        [HttpGet]
        public async Task<ActionResult> CheckFunctionality()
        {
            var sensorFunctionality = false;
            Random random = new Random();
            if (random.NextDouble()<0.9)
            {
                sensorFunctionality = true;
            }
            return this.Ok(sensorFunctionality);
        }
    }
}
