using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.EF.Impl;

namespace DAL.EF.Interfaces
{
    public interface IDataRepository
    {
        // CRUD-операції
        Task<Data> GetByIdAsync(int id); // Отримати Data за ID
        Task<IEnumerable<Data>> GetAllAsync(); // Отримати всі Data
        Task AddAsync(Data data); // Додати новий запис
        Task UpdateAsync(Data data); // Оновити існуючий запис
        Task DeleteAsync(int id); // Видалити запис

        // Додаткові методи
        Task<IEnumerable<Data>> GetBySensorIdAsync(int sensorId); // Отримати Data за SensorId
        Task<IEnumerable<Data>> GetByDateRangeAsync(DateTime startDate, DateTime endDate); // Дані за датами
    }
}
