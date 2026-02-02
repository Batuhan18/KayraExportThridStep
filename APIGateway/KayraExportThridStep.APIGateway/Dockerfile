FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY KayraExportThridStep.APIGateway/*.csproj KayraExportThridStep.APIGateway/
RUN dotnet restore KayraExportThridStep.APIGateway/KayraExportThridStep.APIGateway.csproj

COPY . .
WORKDIR /src/KayraExportThridStep.APIGateway
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KayraExportThridStep.APIGateway.dll"]

