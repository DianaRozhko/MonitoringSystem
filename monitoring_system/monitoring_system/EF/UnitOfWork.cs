using DAL.EF.Impl;
using DAL.EF.Interfaces;
using DAL.UnitOfWork; // Доступ до інтерфейсів репозиторіїв

namespace DAL.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public ISensorRepository SensorRepository { get; private set; }
        public IDataRepository DataRepository { get; private set; }
        public IScientistRepository ScientistRepository { get; private set; }

        public UnitOfWork(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            SensorRepository = new SensorRepository(_context);
            DataRepository = new DataRepository(_context);
            ScientistRepository = new ScientistRepository(_context);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
