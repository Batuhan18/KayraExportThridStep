FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore APIGateway/KayraExportThridStep.APIGateway/KayraExportThridStep.APIGateway.csproj

RUN dotnet publish APIGateway/KayraExportThridStep.APIGateway/KayraExportThridStep.APIGateway.csproj \
    -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KayraExportThridStep.APIGateway.dll"]