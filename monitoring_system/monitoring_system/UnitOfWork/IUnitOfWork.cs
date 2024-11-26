using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    using DAL.UnitOfWork;
    using global::DAL.Repositories.Interfaces;
    using System;

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

}
