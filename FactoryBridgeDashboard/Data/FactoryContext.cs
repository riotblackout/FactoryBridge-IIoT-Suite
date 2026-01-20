using Microsoft.EntityFrameworkCore;
using System;

namespace FactoryBridgeDashboard.Data
{
    // 1. The Schema (What one row looks like)
    public class MachineReading
    {
        public int Id { get; set; } // Unique ID for every row
        public string MachineName { get; set; } = "";
        public double Speed { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; } = "OK"; // "OK" or "CRITICAL"
    }

    // 2. The Database Manager
    public class FactoryContext : DbContext
    {
        public DbSet<MachineReading> Readings { get; set; } // Our Table

        // Simple configuration to use a local file
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=factory.db");
    }
}