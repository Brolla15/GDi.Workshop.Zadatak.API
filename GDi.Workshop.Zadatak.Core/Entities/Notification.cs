using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDi.Workshop.Zadatak.Core.Entities
{
    public class Notification
    {
        public long Id { get; set; }
        public long SensorId { get; set; }
        public DateTime Date { get; set; }
        public Sensor Sensor { get; set; }
        public long Value { get; set; }
    }
}
