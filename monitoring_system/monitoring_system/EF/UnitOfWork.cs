using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories.Impl;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class UnitOfWork : DAL.UnitOfWork.IUnitOfWork
    {
        private DatabaseContext _context;

        public ISensorRepository SensorRepository { get; }
        public IDataRepository DataRepository { get; }
        public IScientistRepository ScientistRepository { get; }

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;

            // Ініціалізація репозиторіїв
            SensorRepository = new SensorRepository(_context);
            DataRepository = new DataRepository(_context);
            ScientistRepository = new ScientistRepository(_context);

        }

        /// <summary>
        /// Зберігає зміни у контексті бази даних.
        /// </summary>
        public void SaveChanges()
        {

        }

        /// <summary>
        /// Звільняє ресурси.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
