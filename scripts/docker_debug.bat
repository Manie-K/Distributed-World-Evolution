REM Navigate to parent directory.
cd /d "%~dp0\.."

REM Build and run debug containers.
docker compose -f docker-compose-justdb.yml up --build