@echo off
echo ==================================================
echo   FACTORY BRIDGE IIoT SUITE - STARTUP SCRIPT
echo ==================================================
echo.
echo 1. Starting Legacy Turbine Simulator (Port 5020)...
start "Turbine Simulator" dotnet run --project LegacyMachine

echo 2. Waiting 5 seconds for simulator to warm up...
timeout /t 5

echo 3. Starting Dashboard UI...
echo    Your browser should open automatically.
dotnet run --project FactoryBridgeDashboard