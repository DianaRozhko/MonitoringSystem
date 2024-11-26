using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Impl
{
    public class ScientistRepository : IScientistRepository
    {
        private readonly List<Scientist> _scientists = new List<Scientist>();
        private DatabaseContext context;

        public ScientistRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public void AddScientist(Scientist scientist)
        {
            if (_scientists.Any(s => s.Username == scientist.Username))
            {
                throw new System.Exception("A scientist with this username already exists.");
            }
            _scientists.Add(scientist);
        }

        public void RemoveScientist(int id)
        {
            var scientist = GetScientistById(id);
            if (scientist != null)
            {
                _scientists.Remove(scientist);
            }
            else
            {
                throw new System.Exception("Scientist not found.");
            }
        }

        public Scientist GetScientistById(int id)
        {
            return _scientists.FirstOrDefault(s => s.Id == id);
        }

        public Scientist GetScientistByUsername(string username)
        {
            return _scientists.FirstOrDefault(s => s.Username == username);
        }

        public IEnumerable<Scientist> GetAllScientists()
        {
            return _scientists;
        }

        public void UpdateScientist(Scientist updatedScientist)
        {
            var existingScientist = GetScientistById(updatedScientist.Id);
            if (existingScientist != null)
            {
                existingScientist.Name = updatedScientist.Name;
                existingScientist.Password = updatedScientist.Password;
            }
            else
            {
                throw new System.Exception("Scientist not found.");
            }
        }
    }
}
