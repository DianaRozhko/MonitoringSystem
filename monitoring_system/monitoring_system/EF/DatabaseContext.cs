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
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        // DbSet для кожної сутності
        public DbSet<Data> Data { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Scientist> Scientists { get; set; }
        public DbSet<Sensor> Sensors { get; set; }

        // Конфігурація моделі
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Data
            modelBuilder.Entity<Data>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<Data>()
                .HasOne(d => d.Report)
                .WithMany(r => r.Data)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.Cascade); // Видалення пов'язаної Data при видаленні Report

            modelBuilder.Entity<Data>()
                .HasOne(d => d.Sensor)
                .WithMany(s => s.Data)
                .HasForeignKey(d => d.SensorId)
                .OnDelete(DeleteBehavior.Restrict); // Обмеження видалення

            // Report
            modelBuilder.Entity<Report>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Scientist)
                .WithMany(s => s.Reports)
                .HasForeignKey(r => r.ScientistId)
                .OnDelete(DeleteBehavior.Cascade);

            // Scientist
            modelBuilder.Entity<Scientist>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Scientist>()
                .HasMany(s => s.Reports)
                .WithOne(r => r.Scientist)
                .HasForeignKey(r => r.ScientistId);

            // Sensor
            modelBuilder.Entity<Sensor>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Sensor>()
                .HasMany(s => s.Data)
                .WithOne(d => d.Sensor)
                .HasForeignKey(d => d.SensorId);
        }
    }
}
