FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["APIGateway/KayraExportThirdStep.APIGateway/KayraExportThirdStep.APIGateway.csproj", "APIGateway/KayraExportThirdStep.APIGateway/"]
RUN dotnet restore "APIGateway/KayraExportThirdStep.APIGateway/KayraExportThirdStep.APIGateway.csproj"

COPY . .
WORKDIR "/src/APIGateway/KayraExportThirdStep.APIGateway"
RUN dotnet build "KayraExportThirdStep.APIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KayraExportThirdStep.APIGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KayraExportThirdStep.APIGateway.dll"]