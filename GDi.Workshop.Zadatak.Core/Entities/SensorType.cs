using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDi.Workshop.Zadatak.Core.Entities
{
    public class SensorType
    {
        public long Id { get; set; }
        public int FromInterval { get; set; }
        public int ToInterval { get; set; }
        public string Name { get; set; }

        public List<Sensor> Sensors { get; set; }
    }
}
