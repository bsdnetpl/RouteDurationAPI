using Microsoft.EntityFrameworkCore;
using RouteDurationAPI.Models;

namespace RouteDurationAPI.DB
{
    public class RouteDurationContext : DbContext
    {
        public RouteDurationContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<VehicleEntry> VehicleEntries { get; set; }
    }
}
