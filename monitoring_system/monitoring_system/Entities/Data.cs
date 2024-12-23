using DAL.Entities;

namespace DAL.EF.Impl
{
    public class Data
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int SensorId { get; set; } // Зовнішній ключ
        public double Value { get; set; }
        public string MeasurementType { get; set; }

        // Навігаційна властивість
        public Sensor Sensor { get; set; }

        public ICollection<Report> Reports { get; set; }
    }
}