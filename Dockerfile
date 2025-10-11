FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# The context is set to the root of the repository
WORKDIR /src

COPY Server/Server.Core/*.csproj ./Server/Server.Core/
COPY Shared/SharedLibrary/*.csproj ./Shared/SharedLibrary/

RUN dotnet restore Server/Server.Core/Server.Core.csproj

#COPY Server/* ./Server/
#COPY Shared/* ./Shared/
COPY . ./

RUN dotnet publish Server/Server.Core/Server.Core.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /src
COPY --from=build /src/out ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "Server.Core.dll"]
