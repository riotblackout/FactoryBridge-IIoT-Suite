using FactoryBridgeDashboard.Data;
// NEW: Add this required namespace at the VERY TOP of the file:
using Microsoft.EntityFrameworkCore;

namespace FactoryBridgeDashboard.Services
{
    public class HistorianService
    {
        public async Task RecordData(string machine, double speed, string status)
        {
            try
            {
                // Open the database connection
                using (var db = new FactoryContext())
                {
                    // Create the database file if it doesn't exist yet
                    await db.Database.EnsureCreatedAsync();

                    // Create the new row
                    var record = new MachineReading
                    {
                        MachineName = machine,
                        Speed = speed,
                        Status = status,
                        Timestamp = DateTime.Now
                    };

                    // Save it
                    db.Readings.Add(record);
                    await db.SaveChangesAsync();

                    Console.WriteLine($"[DB SAVED] ID: {record.Id} | {speed} RPM");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR] {ex.Message}");
            }
        }
        public async Task<List<MachineReading>> GetRecentHistory()
        {

            using (var db = new FactoryContext())
            {
                // FIX: Create the table if it's missing (Prevents "No such table" crash)
                await db.Database.EnsureCreatedAsync();

                // Fetch the last 50 records, newest first
                return await db.Readings
                    .OrderByDescending(r => r.Timestamp)
                    .Take(50)
                    .ToListAsync();
            }
        }
    }
}