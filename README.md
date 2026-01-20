# FactoryBridge - Industrial IoT Middleware Suite

**A .NET 10 distributed system bridging legacy Modbus PLCs to modern real-time Dashboards.**

![System Overview](docs/system_overview.png)
*(Above: The Simulator and Dashboard running simultaneously)*

## 🚀 Quick Start (One-Click Demo)
Want to see it run? You don't need to manually configure ports.
1.  **Clone the repo.**
2.  **Double-click `run_demo.bat`** (Windows).
3.  The script automatically launches the **Turbine Simulator** (Port 5020) and the **Web Dashboard** in the correct order.

## 📂 Architecture
* **`/LegacyMachine`**: C# Console App acting as a Digital Twin (Modbus TCP Server).
* **`/FactoryBridgeDashboard`**: Blazor Server App with SQLite Historian and Background Orchestration.
* **`/BridgeClient`**: Lightweight CLI tool for headless connectivity testing.
