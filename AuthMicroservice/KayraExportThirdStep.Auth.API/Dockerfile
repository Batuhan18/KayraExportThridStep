FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["AuthMicroservice/KayraExportThirdStep.Auth.API/KayraExportThirdStep.Auth.API.csproj", "AuthMicroservice/KayraExportThirdStep.Auth.API/"]
COPY ["AuthMicroservice/KayraExportThirdStep.Auth.Application/KayraExportThirdStep.Auth.Application.csproj", "AuthMicroservice/KayraExportThirdStep.Auth.Application/"]
COPY ["AuthMicroservice/KayraExportThirdStep.Auth.Core/KayraExportThirdStep.Auth.Core.csproj", "AuthMicroservice/KayraExportThirdStep.Auth.Core/"]
COPY ["AuthMicroservice/KayraExportThirdStep.Auth.Infrastructure/KayraExportThirdStep.Auth.Infrastructure.csproj", "AuthMicroservice/KayraExportThirdStep.Auth.Infrastructure/"]

# Restore
RUN dotnet restore "AuthMicroservice/KayraExportThirdStep.Auth.API/KayraExportThirdStep.Auth.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/AuthMicroservice/KayraExportThirdStep.Auth.API"
RUN dotnet build "KayraExportThirdStep.Auth.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KayraExportThirdStep.Auth.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KayraExportThirdStep.Auth.API.dll"]