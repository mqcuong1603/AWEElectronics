# Test Desktop Login
Write-Host "Starting Desktop application test..." -ForegroundColor Green
Write-Host "Please login with:" -ForegroundColor Yellow
Write-Host "  Username: admin" -ForegroundColor Cyan
Write-Host "  Password: 123456" -ForegroundColor Cyan
Write-Host ""

# Start the Desktop application
Start-Process -FilePath "D:\AWEElectronics\Desktop\bin\Debug\Desktop.exe" -Wait

Write-Host "Desktop application closed." -ForegroundColor Green
