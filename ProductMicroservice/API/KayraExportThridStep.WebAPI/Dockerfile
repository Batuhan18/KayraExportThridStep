FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Application/*.csproj Application/
COPY Core/*.csproj Core/
COPY Infrastructure/*.csproj Infrastructure/
COPY API/KayraExportThridStep.WebAPI/*.csproj API/KayraExportThridStep.WebAPI/

RUN dotnet restore API/KayraExportThridStep.WebAPI/KayraExportThridStep.WebAPI.csproj

COPY . .
WORKDIR /src/API/KayraExportThridStep.WebAPI
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KayraExportThridStep.WebAPI.dll"]
