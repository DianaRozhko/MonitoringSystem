using DAL.EF;
using DAL.EF.Interfaces;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.EF.Impl
{
    public class DataRepository : IDataRepository
    {
        private readonly DatabaseContext _context;

        public DataRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Data> GetByIdAsync(int id)
        {
            return await _context.Data
                .Include(d => d.Reports)
                .Include(d => d.Sensor)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Data>> GetAllAsync()
        {
            return await _context.Data
                .Include(d => d.Reports)
                .Include(d => d.Sensor)
                .ToListAsync();
        }

        public async Task AddAsync(Data data)
        {
            await _context.Data.AddAsync(data);
        }

        public async Task UpdateAsync(Data data)
        {
            _context.Data.Update(data);
        }

        public async Task DeleteAsync(int id)
        {
            var data = await GetByIdAsync(id);
            if (data != null)
            {
                _context.Data.Remove(data);
            }
        }

        public async Task<IEnumerable<Data>> GetBySensorIdAsync(int sensorId)
        {
            return await _context.Data
                .Include(d => d.Sensor)
                .Where(d => d.SensorId == sensorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Data>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Data
                .Include(d => d.Sensor)
                .Where(d => d.Timestamp >= startDate && d.Timestamp <= endDate)
                .ToListAsync();
        }
    }
}
