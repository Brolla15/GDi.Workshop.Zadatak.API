using GDi.Workshop.Zadatak.BM.Models;
using GDi.Workshop.Zadatak.Core.Entities;
using GDi.Workshop.Zadatak.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace GDi.Workshop.Zadatak.BM.Controllers
{
    [Route("api/sensors")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly WorkshopDbContext _dbContext;
        private readonly HttpClient _httpClient;

        public SensorController(WorkshopDbContext dbContext, HttpClient httpClient) {
        
            _dbContext = dbContext;
            _httpClient = httpClient;
        }
        [HttpPost("check/{sensorId}")]
        public async Task<ActionResult> Check(int sensorId)
        {
            var processId = "Process_1q7ccjj";
            var url = $"http://localhost:8080/engine-rest/process-definition/key/{processId}/start";
            var bodyData = new { variables = new {}, businessKey = sensorId.ToString() };
            var resp = await _httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(bodyData), Encoding.UTF8, "application/json"));
            
            var content=await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception($"DeleteConfigurationResponse exception, status code: {resp.StatusCode}, content :{content}");
            }
            return this.Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<Sensor>>> GetSensors()
        {
            var sensorsTemp = await _dbContext.Sensors.Include(x=>x.SensorType)
                .Select(x => new SensorModel(x.Id, x.SerialNumber, x.Value, x.SensorTypeId)).ToListAsync();


            return Ok(sensorsTemp);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorModel>> GetSensor(long id)
        {
            var sensor = await _dbContext.Sensors.FirstOrDefaultAsync(x => x.Id == id);
            if (sensor is null)
                return BadRequest("Sensor doesn't exist");

            return Ok(new SensorModel(sensor.Id, sensor.SerialNumber, sensor.Value, sensor.SensorTypeId));
        }
        private void SendMail(long sensorId)
        {
            string to = "bmatokanovic@gmail.com";
            string from = "bmatokanovic@gmail.com";
            MailMessage message=new MailMessage(from,to);
            message.Subject = "Sensor is out of intervals";
            message.Body = @"Sensor with id: " + sensorId + " is out of Intervals";
            SmtpClient client=new SmtpClient();
            client.UseDefaultCredentials = true;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in SendMail():{0}",
                    ex.ToString());
            }
        }
        [HttpPost("add-sensor")]
        public async Task<ActionResult<SensorModel>> AddSensor([FromBody] SensorModel sensorModel)
        {
            var sensorType = await _dbContext.SensorTypes.FirstOrDefaultAsync(x => x.Id == sensorModel.SensorTypeId);
            if (sensorType == null)
            {
                return this.StatusCode(204, "Sensor type does not exist");
            }

            var sensor = new Sensor
            {
                SerialNumber = sensorModel.SerialNumber,
                Value = sensorModel.Value,
                SensorType = sensorType
            };
            if(sensor.SensorType.FromInterval>sensor.Value && sensor.SensorType.ToInterval < sensor.Value)
            {
                SendMail(sensor.Id);
            }
            _dbContext.Sensors.Add(sensor);
            await _dbContext.SaveChangesAsync();

            return Ok(new SensorModel(sensor.Id, sensor.SerialNumber, sensor.Value, sensor.SensorType.Id));

        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<SensorModel>> DeleteSensor(long id)
        {
            var delObj = _dbContext.Sensors.Where(x => x.Id == id).SingleOrDefault();
            if (delObj == null)
                BadRequest("Sensor doesn't exist");

            _dbContext.Sensors.Remove(delObj);
            await _dbContext.SaveChangesAsync();
            return Ok();
            throw new NotImplementedException();

        }
        
        [HttpPut]
        public async Task<ActionResult<SensorModel>> UpdateSensor([FromBody] SensorModel sensorModel)
        {
            var sensor = await _dbContext.Sensors.FirstOrDefaultAsync(x => x.Id == sensorModel.Id);
            if (sensor == null)
                return BadRequest("Sensor doesn't exist");

            var sensorType = await _dbContext.SensorTypes.Where(x => x.Id == sensorModel.SensorTypeId).FirstOrDefaultAsync();

            if (sensorType == null)
            {
                return this.StatusCode(204, "Sensor type does not exist");
            }
            sensor.SerialNumber = sensorModel.SerialNumber;
            sensor.Value = sensorModel.Value;
            //sensor.SensorType = sensorType;
            sensor.SensorTypeId = sensorModel.SensorTypeId;

            await _dbContext.SaveChangesAsync();

            return Ok(sensorModel);
            //var sensorAllowedFrom = sensorType.FromInterval;
            //var sensorAllowedTo = sensorType.ToInterval;
            //if (sensor.Value > sensorAllowedFrom && sensor.Value < sensorAllowedTo)
            //{

            //    sensor.SerialNumber = sensorModel.SerialNumber;
            //    sensor.Value = sensorModel.Value;
            //    sensor.SensorType = sensorType;
            //    sensor.SensorTypeId = sensorModel.SensorTypeId;
            //    await _dbContext.SaveChangesAsync();

            //    return Ok(sensorModel);
            //}
            //else
            //{
            //    return BadRequest("Sensor is not in allowed interval");
            //}
        }

        //[HttpGet("get-sensors-joined")]
        //public async Task<ActionResult<SensorModel>> GetSensor2()
        //{
        //    var sensor = await _dbContext.Sensors.Join(_dbContext.SensorTypes, s => s.SensorType.Id, t => t.Id, (s, t) => new { s.SerialNumber, t.Name }).ToListAsync();

        //    return Ok(sensor);
        //}
    }
}
