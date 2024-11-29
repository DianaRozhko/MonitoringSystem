using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF.Impl;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL.Entities
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public ICollection<Data> Data { get; set; }

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

