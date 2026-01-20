using System.Net.Sockets;
using NModbus;

namespace FactoryBridgeDashboard.Services
{
    // The Data Structure for our 3-Gauge Panel
    public class TurbineData
    {
        public double Speed { get; set; }
        public double Temperature { get; set; }
        public double Power { get; set; }
    }

    public class ModbusService
    {
        // The single event that sends all 3 data points
        public event Action<TurbineData>? OnValueUpdated;

        public async Task StartLegacyMonitoring()
        {
            try
            {
                // Connect to the Simulator (Port 5020)
                using (TcpClient client = new TcpClient("127.0.0.1", 5020))
                {
                    var factory = new ModbusFactory();
                    var master = factory.CreateMaster(client);

                    while (client.Connected)
                    {
                        try
                        {
                            // 1. Read 3 Registers (Speed, Temp, Power)
                            // Start at Address 1, Read 3 items
                            ushort[] registers = await master.ReadHoldingRegistersAsync(1, 1, 3);

                            if (registers.Length == 3)
                            {
                                var data = new TurbineData
                                {
                                    Speed = registers[0],
                                    Temperature = registers[1],
                                    Power = registers[2]
                                };

                                // Send to UI immediately
                                OnValueUpdated?.Invoke(data);
                            }
                        }
                        catch
                        {
                            // Ignore read errors, just retry next loop
                        }

                        // 2. WAIT 100ms (Fast Update Rate)
                        await Task.Delay(100);
                    }
                }
            }
            catch (Exception)
            {
                // Connection failed (Machine offline)
            }
        }
    }
}