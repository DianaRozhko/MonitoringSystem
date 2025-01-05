using DAL.EF;
using DAL.EF.Interfaces;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.EF.Impl
{
    public class ScientistRepository : IScientistRepository
    {
        private readonly DatabaseContext _context;

        public ScientistRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Scientist>> GetAllScientistsAsync()
        {
            return await _context.Scientists
                .Include(s => s.Reports)
                .ToListAsync();
        }

        public async Task<Scientist> GetByIdAsync(int id)
        {
            return await _context.Scientists
                .Include(s => s.Reports)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Scientist> GetByUsernameAsync(string username)
        {
            return await _context.Scientists
                .Include(s => s.Reports)
                .FirstOrDefaultAsync(s => s.Username == username);
        }

        public async Task AddAsync(Scientist scientist)
        {
            await _context.Scientists.AddAsync(scientist);
        }

        public async Task UpdateAsync(Scientist scientist)
        {
            _context.Scientists.Update(scientist);
        }

        public async Task DeleteAsync(int id)
        {
            var scientist = await GetByIdAsync(id);
            if (scientist != null)
            {
                _context.Scientists.Remove(scientist);
            }
        }
    }
}
