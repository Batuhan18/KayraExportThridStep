FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ProductMicroservice/API/KayraExportThridStep.WebAPI/KayraExportThridStep.WebAPI.csproj ProductMicroservice/API/KayraExportThridStep.WebAPI/
COPY ProductMicroservice/Application/KayraExportThridStep.Application.csproj ProductMicroservice/Application/
COPY ProductMicroservice/Core/KayraExportThridStep.Core.csproj ProductMicroservice/Core/
COPY ProductMicroservice/Infrastructure/KayraExportThridStep.Infrastructure.csproj ProductMicroservice/Infrastructure/

RUN dotnet restore ProductMicroservice/API/KayraExportThridStep.WebAPI/KayraExportThridStep.WebAPI.csproj

COPY . .
WORKDIR /src/ProductMicroservice/API/KayraExportThridStep.WebAPI
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KayraExportThridStep.WebAPI.dll"]
