FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY API/KayraExportThridStep.Log.API/*.csproj API/KayraExportThridStep.Log.API/
COPY Application/*.csproj Application/
COPY Core/*.csproj Core/
COPY Infrastructure/*.csproj Infrastructure/

RUN dotnet restore API/KayraExportThridStep.Log.API/KayraExportThridStep.Log.API.csproj

COPY . .
WORKDIR /src/API/KayraExportThridStep.Log.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KayraExportThridStep.Log.API.dll"]
