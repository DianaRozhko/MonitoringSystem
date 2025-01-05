using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Data
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int SensorId { get; set; }   // Зовнішній ключ
        public double Value { get; set; }
        public string MeasurementType { get; set; }

        // Навігаційна властивість
        public Sensor Sensor { get; set; }

        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
