using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IDataService
    {
        Task<IEnumerable<DataDTO>> GetAllDataAsync();
        Task<DataDTO> GetDataByIdAsync(int id);
        Task AddDataAsync(DataDTO dataDto);
        Task UpdateDataAsync(DataDTO dataDto);
        Task DeleteDataAsync(int id);

        Task<IEnumerable<DataDTO>> GetDataBySensorIdAsync(int sensorId);
        Task<IEnumerable<DataDTO>> GetDataByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
