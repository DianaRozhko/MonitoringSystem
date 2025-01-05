using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.EF.Interfaces
{
    public interface IScientistRepository
    {
        Task<IEnumerable<Scientist>> GetAllScientistsAsync();
        Task<Scientist> GetByIdAsync(int id);
        Task<Scientist> GetByUsernameAsync(string username);
        Task AddAsync(Scientist scientist);
        Task UpdateAsync(Scientist scientist);
        Task DeleteAsync(int id);
    }
}
