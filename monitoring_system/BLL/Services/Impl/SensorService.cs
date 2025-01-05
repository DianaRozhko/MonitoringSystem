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
    public class SensorService : ISensorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SensorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<SensorDTO>> GetAllSensorsAsync()
        {
            var sensors = await _unitOfWork.SensorRepository.GetAllSensorsAsync();
            return _mapper.Map<IEnumerable<SensorDTO>>(sensors);
        }

        public async Task<SensorDTO> GetSensorByIdAsync(int id)
        {
            var sensor = await _unitOfWork.SensorRepository.GetSensorByIdAsync(id);
            if (sensor == null)
                throw new KeyNotFoundException($"Sensor with ID {id} not found.");

            return _mapper.Map<SensorDTO>(sensor);
        }

        public async Task AddSensorAsync(SensorDTO sensorDto)
        {
            var sensor = _mapper.Map<Sensor>(sensorDto);
            await _unitOfWork.SensorRepository.AddAsync(sensor);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateSensorAsync(SensorDTO sensorDto)
        {
            var sensor = _mapper.Map<Sensor>(sensorDto);
            await _unitOfWork.SensorRepository.UpdateAsync(sensor);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteSensorAsync(int id)
        {
            await _unitOfWork.SensorRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
