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

## 🚀 Getting Started

To see the system in action, run the Simulator first, then the Dashboard.

### Prerequisites
* .NET 8.0 SDK or higher (supports .NET 10 Preview)

### Installation
1.  **Clone the repo:**
    ```bash
    git clone [https://github.com/riotblackout/FactoryBridge-IIoT-Suite.git](https://github.com/riotblackout/FactoryBridge-IIoT-Suite.git)
    ```

2.  **Start the Simulator (The Turbine):**
    ```bash
    cd LegacyMachine
    dotnet run
    ```
    *Keep this terminal open. It will start listening on Port 5020.*

3.  **Start the Dashboard (The UI):**
    Open a new terminal:
    ```bash
    cd FactoryBridgeDashboard
    dotnet run
    ```
    *Open your browser to http://localhost:5xxx to see the live telemetry.*
