using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Impl
{
    public class DataRepository : IDataRepository
    {
        private readonly DatabaseContext _context;

        public DataRepository(DatabaseContext context)
        {
            _context = context;
        }

        // CRUD-операції

        public async Task<Data> GetByIdAsync(int id)
        {
            // Отримання Data за ID
            return await _context.Data
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Data>> GetAllAsync()
        {
            // Отримання всіх записів Data
            return await _context.Data.ToListAsync();
        }

        public async Task AddAsync(Data data)
        {
            // Додавання нового запису Data
            await _context.Data.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Data data)
        {
            // Оновлення існуючого запису Data
            _context.Data.Update(data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Видалення запису Data
            var data = await GetByIdAsync(id);
            if (data != null)
            {
                _context.Data.Remove(data);
                await _context.SaveChangesAsync();
            }
        }

        // Специфічні методи

        public async Task<IEnumerable<Data>> GetBySensorIdAsync(int sensorId)
        {
            // Отримання даних по SensorId
            return await _context.Data
                .Where(d => d.SensorId == sensorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Data>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            // Отримання даних по діапазону дат
            return await _context.Data
                .Where(d => d.Timestamp >= startDate && d.Timestamp <= endDate)
                .ToListAsync();
        }
    }
}
