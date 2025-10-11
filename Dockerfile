FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# The context is set to the root of the repository
WORKDIR /app

COPY ./Server/Server.Core/*.csproj ./
COPY ./Shared/SharedLibrary/*.csproj ./Shared/SharedLibrary/
RUN dotnet restore Server.Core.csproj

COPY . ./
RUN dotnet publish Server.Core.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "Server.Core.dll"]
