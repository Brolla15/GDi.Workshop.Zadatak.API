using Microsoft.EntityFrameworkCore;
using GDi.Workshop.Zadatak.Core.Entities;

namespace GDi.Workshop.Zadatak.Infrastructure
{
    public class WorkshopDbContext:DbContext
    {
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorType> SensorTypes { get; set; }
        public WorkshopDbContext(DbContextOptions options): base(options) { }
    }
}