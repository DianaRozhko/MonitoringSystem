using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.EF.Interfaces
{
    public interface ISensorRepository
    {
        Task<IEnumerable<Sensor>> GetAllSensorsAsync();
        Task<Sensor> GetSensorByIdAsync(int id);
        Task AddAsync(Sensor sensor);
        Task UpdateAsync(Sensor sensor);
        Task DeleteAsync(int id);
    }
}
