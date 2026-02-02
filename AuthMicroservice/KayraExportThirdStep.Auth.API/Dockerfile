FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY KayraExportThirdStep.Auth.API/*.csproj KayraExportThirdStep.Auth.API/
COPY KayraExportThridStep.Auth.Application/*.csproj KayraExportThridStep.Auth.Application/
COPY KayraExportThridStep.Auth.Core/*.csproj KayraExportThridStep.Auth.Core/
COPY KayraExportThridStep.Auth.Infrastructure/*.csproj KayraExportThridStep.Auth.Infrastructure/

WORKDIR /src/KayraExportThirdStep.Auth.API
RUN dotnet restore

WORKDIR /src
COPY . .

WORKDIR /src/KayraExportThirdStep.Auth.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KayraExportThirdStep.Auth.API.dll"]
