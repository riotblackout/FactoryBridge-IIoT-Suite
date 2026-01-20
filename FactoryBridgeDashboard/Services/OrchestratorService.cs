namespace FactoryBridgeDashboard.Services
{
    // This class runs forever in the background
    public class OrchestratorService : IHostedService
    {
        private readonly ModbusService _modbus;
        private readonly HistorianService _historian;
        private readonly AlertService _alerter;

        private DateTime _lastSave = DateTime.MinValue;

        public OrchestratorService(ModbusService modbus, HistorianService historian, AlertService alerter)
        {
            _modbus = modbus;
            _historian = historian;
            _alerter = alerter;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Subscribe to the stream immediately on startup
            _modbus.OnValueUpdated += HandleData;

            // Ensure the legacy monitoring is actually running
            _ = _modbus.StartLegacyMonitoring();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up when server shuts down
            _modbus.OnValueUpdated -= HandleData;
            return Task.CompletedTask;
        }

        private void HandleData(TurbineData data)
        {
            // 1. THROTTLING: Only save once per second
            if ((DateTime.Now - _lastSave).TotalSeconds < 1) return;

            // 2. LOGIC: Check Critical Status
            string status = "OK";
            if (data.Speed > 2400 || data.Temperature > 90)
            {
                status = "CRITICAL";
                // Trigger Email Alert (Fire and forget)
                _ = Task.Run(() => _alerter.SendCriticalAlert("Turbine-1", data.Speed));
            }

            // 3. PERSISTENCE: Save to Database
            _ = Task.Run(() => _historian.RecordData("Turbine-1", data.Speed, status));

            _lastSave = DateTime.Now;
        }
    }
}