using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // Визначення таблиць для наявних сутностей
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Data> Data { get; set; }
        public DbSet<Scientist> Scientists { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування зв’язків між сутностями

            // Sensor -> Data (1 до багатьох)
            modelBuilder.Entity<Sensor>()
                .HasMany(static s => s.Data)
                .WithOne(staticd => d.Sensor)
                .HasForeignKey(d => d.SensorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Scientist -> Report (1 до багатьох)
            modelBuilder.Entity<Scientist>()
                .HasMany(s => s.Reports)
                .WithOne(r => r.Scientist)
                .HasForeignKey(r => r.ScientistId)
                .OnDelete(DeleteBehavior.Cascade);

            // Data -> Report (1 до багатьох)
            modelBuilder.Entity<Data>()
                .HasMany(d => d.Reports)
                .WithOne(r => r.Data)
                .HasForeignKey(r => r.DataId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
