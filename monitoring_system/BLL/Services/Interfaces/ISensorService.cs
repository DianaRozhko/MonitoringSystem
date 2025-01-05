using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ISensorService
    {
        Task<IEnumerable<SensorDTO>> GetAllSensorsAsync();
        Task<SensorDTO> GetSensorByIdAsync(int id);
        Task AddSensorAsync(SensorDTO sensorDto);
        Task UpdateSensorAsync(SensorDTO sensorDto);
        Task DeleteSensorAsync(int id);
    }
}
