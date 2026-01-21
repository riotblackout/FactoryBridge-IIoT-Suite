# 🗺️ Project Roadmap & Production Readiness

This project is a Proof of Concept (PoC) demonstrating IIoT connectivity patterns.
Below are the steps required to move from PoC to Production v1.0.

## 🚧 Known Limitations & Future Work

### 1. Security
* [ ] **Authentication:** Implement Identity (OIDC/OAuth2) to secure the Dashboard.
* [ ] **HTTPS:** Enforce TLS encryption for web traffic.

### 2. Resilience
* [ ] **Logging:** Migrate from Console logging to Serilog (File/Seq sinks) for persistence.
* [ ] **Health Checks:** Add ASP.NET Core Health Checks endpoint (`/health`) for watchdog monitoring.

### 3. Quality Assurance
* [ ] **Unit Tests:** Add xUnit project to cover Modbus Byte Parsing logic.
* [ ] **CI/CD:** Add GitHub Actions workflow for automated build and test.

### 4. Scalability
* [ ] **Database:** Evaluate migration from SQLite to PostgreSQL if retention exceeds 1 year.