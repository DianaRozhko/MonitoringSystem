using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositories.Interfaces
{
    public interface IScientistRepository
    {
        void AddScientist(Scientist scientist);
        void RemoveScientist(int id);
        Scientist GetScientistById(int id);
        Scientist GetScientistByUsername(string username);
        IEnumerable<Scientist> GetAllScientists();
        void UpdateScientist(Scientist scientist);
    }
}
