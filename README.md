# Distributed-World-Evolution
[PL] Tytuł pracy: Rozproszony serwer do modelowania ewolucyjnego świata z modułową zawartością  
[ENG] Thesis title: Distributed server for modeling an evolutionary world with modular content 

## Tech stack
C#  
MonoGame  
Docker  
PostgreSQL  
Python (map editor)  

## Developers
Franciszek Gwarek  
Maciej Góralczyk  
Przemysław Dębek  
Michał Węsiora

## Branches
master - release  
*development*  
server  
client  

## How to run  
1. Download source code
2. Compile:
    - Shared.sln 
    - Client.sln
    - Server.sln
3. Run docker_release.bat script located in "scripts" directory
4. Configure appsettings.json file in "Client" directory (not needed when running on the same PC as server)
5. Launch client application with .exe file

## How to generate documentation
1. Open root directory in cmd
2. Run "dotnet tool install -g docfx"
3. Run "docfx docfx.json --serve"

