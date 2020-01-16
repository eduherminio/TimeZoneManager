start "Backend" cmd /K "cd backend\TimeZoneManager.Api && dotnet build -c Release && dotnet run -c Release"

start "WebClient" cmd /K "cd frontend\ && npm install && npm run build && serve -s build -l 8002"