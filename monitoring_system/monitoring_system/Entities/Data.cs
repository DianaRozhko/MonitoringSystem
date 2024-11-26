using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositories.Impl
{
    public class Data
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int SensorId { get; set; }
        public double Value { get; set; }
        public string MeasurementType { get; set; }
        public object Sensor { get; internal set; }
        public ICollection<Report> Reports { get; set; }
    }
}
