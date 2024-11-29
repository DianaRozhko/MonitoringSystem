using System;
using DAL.EF.Interfaces;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ISensorRepository SensorRepository { get; }
        IDataRepository DataRepository { get; }
        IScientistRepository ScientistRepository { get; }

        void SaveChanges();
    }
}
