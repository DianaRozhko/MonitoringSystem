using DAL.EF.Interfaces;
using System;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ISensorRepository SensorRepository { get; }
        IDataRepository DataRepository { get; }
        IScientistRepository ScientistRepository { get; }

        Task SaveChangesAsync();
    }
}
