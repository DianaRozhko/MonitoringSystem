using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.EF.Interfaces
{
    public interface IDataRepository
    {
        Task<Data> GetByIdAsync(int id);
        Task<IEnumerable<Data>> GetAllAsync();
        Task AddAsync(Data data);
        Task UpdateAsync(Data data);
        Task DeleteAsync(int id);

        Task<IEnumerable<Data>> GetBySensorIdAsync(int sensorId);
        Task<IEnumerable<Data>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
