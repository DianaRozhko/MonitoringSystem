using System;
using System.Collections.Generic;

namespace BLL.DTO
{
    public class DataDTO
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int SensorId { get; set; }
        public double Value { get; set; }
        public string MeasurementType { get; set; }

        public string SensorName { get; set; }

        public ICollection<int> ReportIds { get; set; } = new List<int>();
    }
}
