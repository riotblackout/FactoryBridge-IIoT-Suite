## 📂 Project Structure

This repository contains the full end-to-end simulation and monitoring suite:

1.  **`/LegacyMachine` (The Simulator)**
    * A multi-threaded C# console application that acts as a "Digital Twin" of a legacy turbine.
    * Simulates Modbus TCP responses on Port 5020.
    * Generates synthetic telemetry (RPM, Temperature, Power) with random jitter to mimic real-world sensor noise.

2.  **`/FactoryBridgeDashboard` (The Core Product)**
    * The production-grade Blazor Server application.
    * **Features:** Real-time gauges, SQLite persistence, Background Service Orchestrator, and SMTP Alerting.
    * **Tech:** .NET 10, Entity Framework Core, System.IO.Ports.

3.  **`/BridgeClient` (The CLI Tool)**
    * A lightweight diagnostic tool used to test connectivity during the initial site survey.
    * Used for "Headless" debugging when a UI is not available.