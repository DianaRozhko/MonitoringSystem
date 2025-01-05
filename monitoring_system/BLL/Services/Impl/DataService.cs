using AutoMapper;
using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Impl
{
    public class DataService : IDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DataService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<DataDTO>> GetAllDataAsync()
        {
            var data = await _unitOfWork.DataRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DataDTO>>(data);
        }

        public async Task<DataDTO> GetDataByIdAsync(int id)
        {
            var data = await _unitOfWork.DataRepository.GetByIdAsync(id);
            if (data == null)
                throw new KeyNotFoundException($"Data with ID {id} not found.");

            return _mapper.Map<DataDTO>(data);
        }

        public async Task AddDataAsync(DataDTO dataDto)
        {
            var data = _mapper.Map<Data>(dataDto);
            await _unitOfWork.DataRepository.AddAsync(data);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateDataAsync(DataDTO dataDto)
        {
            var data = _mapper.Map<Data>(dataDto);
            await _unitOfWork.DataRepository.UpdateAsync(data);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteDataAsync(int id)
        {
            await _unitOfWork.DataRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<DataDTO>> GetDataBySensorIdAsync(int sensorId)
        {
            var data = await _unitOfWork.DataRepository.GetBySensorIdAsync(sensorId);
            return _mapper.Map<IEnumerable<DataDTO>>(data);
        }

        public async Task<IEnumerable<DataDTO>> GetDataByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var data = await _unitOfWork.DataRepository.GetByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<DataDTO>>(data);
        }
    }
}
