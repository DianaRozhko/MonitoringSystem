using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Impl
{
    public class SensorRepository : ISensorRepository
    {
        private readonly List<Sensor> _sensors = new()
        {
            new Sensor { Id = 1, Type = "Air Quality", Location = "Zone A", Status = "Active" },
            new Sensor { Id = 2, Type = "Radiation", Location = "Zone B", Status = "Active" }
        };

        public SensorRepository(DatabaseContext context)
        {
        }

        public List<Sensor> GetAllSensors()
        {
            return _sensors;
        }

        public Sensor GetSensorById(int id)
        {
            return _sensors.Find(sensor => sensor.Id == id);
        }
    }
}
