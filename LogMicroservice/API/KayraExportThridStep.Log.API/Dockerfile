FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["LogMicroservice/KayraExportThirdStep.Log.API/KayraExportThirdStep.Log.API.csproj", "LogMicroservice/KayraExportThirdStep.Log.API/"]
COPY ["LogMicroservice/KayraExportThirdStep.Log.Application/KayraExportThirdStep.Log.Application.csproj", "LogMicroservice/KayraExportThirdStep.Log.Application/"]
COPY ["LogMicroservice/KayraExportThirdStep.Log.Core/KayraExportThirdStep.Log.Core.csproj", "LogMicroservice/KayraExportThirdStep.Log.Core/"]
COPY ["LogMicroservice/KayraExportThirdStep.Log.Infrastructure/KayraExportThirdStep.Log.Infrastructure.csproj", "LogMicroservice/KayraExportThirdStep.Log.Infrastructure/"]

# Restore
RUN dotnet restore "LogMicroservice/KayraExportThirdStep.Log.API/KayraExportThirdStep.Log.API.csproj"

# Copy everything
COPY . .

# Build
WORKDIR "/src/LogMicroservice/KayraExportThirdStep.Log.API"
RUN dotnet build "KayraExportThirdStep.Log.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KayraExportThirdStep.Log.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KayraExportThirdStep.Log.API.dll"]