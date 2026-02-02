FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["ProductMicroservice/API/KayraExportThridStep.WebAPI.csproj", "ProductMicroservice/API/"]
COPY ["ProductMicroservice/Application/KayraExportThridStep.Application.csproj", "ProductMicroservice/Application/"]
COPY ["ProductMicroservice/Core/KayraExportThridStep.Core.csproj", "ProductMicroservice/Core/"]
COPY ["ProductMicroservice/Infrastructure/KayraExportThridStep.Infrastructure.csproj", "ProductMicroservice/Infrastructure/"]

# Restore
RUN dotnet restore "ProductMicroservice/API/KayraExportThridStep.WebAPI.csproj"

# Copy everything
COPY . .

# Build
WORKDIR "/src/ProductMicroservice/API"
RUN dotnet build "KayraExportThridStep.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KayraExportThridStep.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KayraExportThridStep.WebAPI.dll"]