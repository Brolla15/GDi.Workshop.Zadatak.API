using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDi.Workshop.Zadatak.Core.Entities
{
    public class Sensor
    {
        public long Id { get; set; }

        public string SerialNumber { get; set; }
        public long Value { get; set; }
        public SensorType SensorType { get; set; }
        public long SensorTypeId { get; set; }
    }
}
