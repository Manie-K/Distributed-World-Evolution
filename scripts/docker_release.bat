REM Navigate to parent directory.
cd /d "%~dp0\.."

REM Build and run release containers.
docker compose -f docker-compose.yml up --build