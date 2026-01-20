using System.Net;
using System.Net.Sockets;
using NModbus;

Console.WriteLine("--- 1990s LEGACY TURBINE CONTROLLER (Modbus TCP) ---");

// 1. Listen on Localhost Port 5020
// (Real machines use 502, but we use 5020 to avoid admin rights issues)
var listener = new TcpListener(IPAddress.Any, 5020);
listener.Start();

// 2. Create the Modbus Factory
var factory = new ModbusFactory();
var network = factory.CreateSlaveNetwork(listener);

// 3. Define our "Device ID" (Slave ID 1)
var slave = factory.CreateSlave(1);
network.AddSlave(slave);

Console.WriteLine("Machine Started. Listening on Port 5020...");
Console.WriteLine("Simulating Data in Register 40001...");

// 4. The Simulation Loop (The Machine 'Run' Cycle)
var random = new Random();
var dataStore = slave.DataStore; // This is the machine's memory

// Run forever
_ = Task.Run(async () =>
{
    // ... inside the Task.Run loop ...
    while (true)
    {
        // 1. Simulate RPM (Speed)
        ushort speed = (ushort)random.Next(1500, 2500);

        // 2. Simulate Temperature (Follows speed: High Speed = Hotter)
        // Formula: Base 60°C + (Speed/100) + random jitter
        ushort temp = (ushort)(60 + (speed / 100) + random.Next(-2, 5));

        // 3. Simulate Power (kW)
        // Formula: Torque * Speed (roughly simulated)
        ushort power = (ushort)((speed * 0.8) + random.Next(-10, 10));

        // WRITE ALL 3 REGISTERS AT ONCE
        // Address 1: Speed
        // Address 2: Temp
        // Address 3: Power
        dataStore.HoldingRegisters.WritePoints(1, new ushort[] { speed, temp, power });

        Console.Write($"\r[Machine State] Speed: {speed} RPM | Temp: {temp}°C | Power: {power} kW    ");

        // UPDATE SPEED: 100ms (Fast)
        await Task.Delay(100);
    }
});

// 5. Keep the server alive to answer requests
await network.ListenAsync();