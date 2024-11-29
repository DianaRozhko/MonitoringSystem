using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.EF.Impl;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class DatabaseContext : DbContext
{

    // DbSet для кожної сутності
    public DbSet<Data> Data { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Scientist> Scientists { get; set; }
    public DbSet<Sensor> Sensors { get; set; }

    // Конструктор класу
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
}
