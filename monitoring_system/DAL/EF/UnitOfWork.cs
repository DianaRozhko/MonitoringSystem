using DAL.EF;
using DAL.EF.Impl;
using DAL.EF.Interfaces;
using System;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public ISensorRepository SensorRepository { get; }
        public IDataRepository DataRepository { get; }
        public IScientistRepository ScientistRepository { get; }

        public UnitOfWork(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            SensorRepository = new SensorRepository(_context);
            DataRepository = new DataRepository(_context);
            ScientistRepository = new ScientistRepository(_context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
