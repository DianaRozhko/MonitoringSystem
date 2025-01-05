using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IScientistService
    {
        Task<IEnumerable<ScientistDTO>> GetAllScientistsAsync();
        Task<ScientistDTO> GetScientistByIdAsync(int id);
        Task AddScientistAsync(ScientistDTO scientistDto);
        Task UpdateScientistAsync(ScientistDTO scientistDto);
        Task DeleteScientistAsync(int id);
    }
}
