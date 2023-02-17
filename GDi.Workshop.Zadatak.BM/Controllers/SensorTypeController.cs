using GDi.Workshop.Zadatak.BM.Models;
using GDi.Workshop.Zadatak.Core.Entities;
using GDi.Workshop.Zadatak.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace GDi.Workshop.Zadatak.BM.Controllers
{
    [Route("api/sensorType")]
    [ApiController]
    public class SensorTypeController : ControllerBase
    {
        private readonly WorkshopDbContext _dbContext;

        public SensorTypeController(WorkshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("get-sensorTypes")]
        public async Task<ActionResult<List<SensorTypesModel>>> GetSensorTypes()
        {
            var sensorTypes = await _dbContext.SensorTypes.Select(x => new SensorTypesModel(x.Id, x.FromInterval, x.ToInterval, x.Name)).ToListAsync();

            return Ok(sensorTypes);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorTypesModel>> GetSensorType(long id)
        {
            var sensorType = await _dbContext.SensorTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (sensorType is null)
                return BadRequest("Sensor type doesn't exist");

            return Ok(new SensorTypesModel(sensorType.Id,sensorType.FromInterval,sensorType.ToInterval,sensorType.Name));
        }
        [HttpPost]
        public async Task<ActionResult<SensorTypesModel>> AddSensorType([FromBody] SensorTypesModel sensorTypeModel)
        {
            var sensorType = new SensorType
            {
                Name = sensorTypeModel.Name,
                FromInterval = sensorTypeModel.FromInterval,
                ToInterval = sensorTypeModel.ToInterval
            };
            _dbContext.SensorTypes.Add(sensorType);

            await _dbContext.SaveChangesAsync();
            return this.Ok();
        }

        [HttpPut("modifySensorType")]
        public async Task<ActionResult<SensorTypesModel>> UpdateSensorType([FromBody] SensorTypesModel sensorTypeModel)
        {
            var sensorType = await _dbContext.SensorTypes.FirstOrDefaultAsync(x => x.Id == sensorTypeModel.Id);
            if (sensorType == null)
                return BadRequest("Sensor type doesn't exist");

            sensorType.Name = sensorTypeModel.Name;
            sensorType.FromInterval = sensorTypeModel.FromInterval;
            sensorType.ToInterval = sensorTypeModel.ToInterval;

            await _dbContext.SaveChangesAsync();

            return Ok(sensorTypeModel);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<SensorTypesModel>> DeleteSensorType(int id)
        {
            var sensors = _dbContext.Sensors.Where(x => x.SensorTypeId == id).ToList();
            if(sensors.Count > 0)
            {
                return BadRequest("Sensor type has sensors of it's type");
            }
            else
            {
                var delObj = _dbContext.SensorTypes.Where(x => x.Id == id).SingleOrDefault();
                if (delObj == null)
                    return BadRequest("Sensor type doesn't exist");

                _dbContext.SensorTypes.Remove(delObj);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpGet("dropdown")]
        public async Task<ActionResult<List<DropdownModel>>> GetSensorTypeDropdown()
        {
            var sensorsDropdown = await _dbContext.Sensors.Select(x => new DropdownModel(x.Id, x.SerialNumber)).ToListAsync();
            return Ok(sensorsDropdown);
        }
    }
}
