@echo off
echo ==================================================
echo   FACTORY BRIDGE IIoT SUITE - STARTUP SCRIPT
echo ==================================================
echo.

echo 1. Starting Legacy Turbine Simulator (Port 5020)...
:: Opens in a new window so this script can continue
start "Turbine Simulator" dotnet run --project LegacyMachine

echo 2. Waiting 5 seconds for simulator to warm up...
timeout /t 5 /nobreak >nul

echo 3. Starting Dashboard Backend...
:: Opens in a new window
start "FactoryBridge Dashboard" dotnet run --project FactoryBridgeDashboard

echo 4. Waiting 5 seconds for Web Server to start...
timeout /t 5 /nobreak >nul

echo 5. Launching Browser...
:: This command forces your default browser to open
start http://localhost:5008

echo.
echo ==================================================
echo   SYSTEM IS LIVE.
echo   Close the black console windows to stop.
echo ==================================================
pause