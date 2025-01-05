using DAL.EF;
using DAL.EF.Interfaces;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.EF.Impl
{
    public class SensorRepository : ISensorRepository
    {
        private readonly DatabaseContext _context;

        public SensorRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sensor>> GetAllSensorsAsync()
        {
            return await _context.Sensors
                .Include(s => s.Data)
                .ToListAsync();
        }

        public async Task<Sensor> GetSensorByIdAsync(int id)
        {
            return await _context.Sensors
                .Include(s => s.Data)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Sensor sensor)
        {
            await _context.Sensors.AddAsync(sensor);
        }

        public async Task UpdateAsync(Sensor sensor)
        {
            _context.Sensors.Update(sensor);
        }

        public async Task DeleteAsync(int id)
        {
            var sensor = await GetSensorByIdAsync(id);
            if (sensor != null)
            {
                _context.Sensors.Remove(sensor);
            }
        }
    }
}
