using DAL.EF.Impl;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class DatabaseContext : DbContext
    {
        // DbSet для кожної сутності
        public DbSet<Data> Data { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Scientist> Scientists { get; set; }
        public DbSet<Sensor> Sensors { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
    }
}
