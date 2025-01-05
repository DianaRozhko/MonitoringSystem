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
    public class ScientistService : IScientistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScientistService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ScientistDTO>> GetAllScientistsAsync()
        {
            var scientists = await _unitOfWork.ScientistRepository.GetAllScientistsAsync();
            return _mapper.Map<IEnumerable<ScientistDTO>>(scientists);
        }

        public async Task<ScientistDTO> GetScientistByIdAsync(int id)
        {
            var scientist = await _unitOfWork.ScientistRepository.GetByIdAsync(id);
            if (scientist == null)
                throw new KeyNotFoundException($"Scientist with ID {id} not found.");

            return _mapper.Map<ScientistDTO>(scientist);
        }

        public async Task AddScientistAsync(ScientistDTO scientistDto)
        {
            var scientist = _mapper.Map<Scientist>(scientistDto);
            await _unitOfWork.ScientistRepository.AddAsync(scientist);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateScientistAsync(ScientistDTO scientistDto)
        {
            var scientist = _mapper.Map<Scientist>(scientistDto);
            await _unitOfWork.ScientistRepository.UpdateAsync(scientist);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteScientistAsync(int id)
        {
            await _unitOfWork.ScientistRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
