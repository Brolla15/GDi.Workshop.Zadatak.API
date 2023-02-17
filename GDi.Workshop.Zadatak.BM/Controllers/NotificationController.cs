using GDi.Workshop.Zadatak.BM.Models;
using GDi.Workshop.Zadatak.Core.Entities;
using GDi.Workshop.Zadatak.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GDi.Workshop.Zadatak.BM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly WorkshopDbContext _dbContext;

        public NotificationController(WorkshopDbContext dbContext)
        {

            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Notification>>> GetNotifications()
        {
            var notification = await _dbContext.Notifications.ToListAsync();

            return Ok(notification);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorModel>> GetNotification(long Id)
        {
            var notification = await _dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == Id);
            if (notification is null)
                return BadRequest("Notification doesn't exist");

            return Ok(new NotificationModel(notification.Id,notification.Date,notification.Value,notification.SensorId));
        }

        [HttpPost("add-notification")]
        public async Task<ActionResult<NotificationModel>> AddNotification([FromBody] NotificationModel notificationModel)
        {
            var sensor = await _dbContext.Sensors.FirstOrDefaultAsync(x => x.Id == notificationModel.SensorId);
            if(sensor is null)
            {
                return BadRequest("Sensor doesn't exist");
            }

            var notification = new Notification
            {
                SensorId= notificationModel.SensorId,
                Value= notificationModel.Value,
                Date= notificationModel.Date
            };
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();

            return Ok(new NotificationModel(notification.Id, notification.Date,notification.Value,notification.SensorId));

        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<SensorModel>> DeleteSensor(long id)
        {
            var delObj = _dbContext.Notifications.Where(x => x.Id == id).SingleOrDefault();
            if (delObj == null)
                BadRequest("Sensor doesn't exist");

            _dbContext.Notifications.Remove(delObj);
            await _dbContext.SaveChangesAsync();
            return Ok();
            throw new NotImplementedException();

        }
        [HttpPut]
        public async Task<ActionResult<SensorModel>> UpdateSensor([FromBody] NotificationModel notificationModel)
        {
            var notification = await _dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == notificationModel.Id);
            if (notification == null)
                return BadRequest("Notification doesn't exist");

            notification.SensorId = notificationModel.SensorId;
            notification.Value = notificationModel.Value;
            notification.Date = notificationModel.Date;

            await _dbContext.SaveChangesAsync();

            return Ok(notificationModel);
            
        }
    }
}
