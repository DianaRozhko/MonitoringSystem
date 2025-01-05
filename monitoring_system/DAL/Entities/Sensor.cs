using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }

        // Дані, пов'язані з цим сенсором
        public ICollection<Data> Data { get; set; } = new List<Data>();

        // Якщо хочете «знімати» дані безпосередньо
        public Data CollectData()
        {
            // Симуляція зчитування даних із сенсора
            return new Data
            {
                SensorId = Id,
                Timestamp = DateTime.Now,
                Value = new Random().NextDouble() * 100,
                MeasurementType = Type
            };
        }
    }
}
