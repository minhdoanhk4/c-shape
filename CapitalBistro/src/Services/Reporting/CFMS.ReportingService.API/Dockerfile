FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Reporting/CFMS.ReportingService.API/CFMS.ReportingService.API.csproj", "src/Services/Reporting/CFMS.ReportingService.API/CFMS.ReportingService.API.csproj"]
# Copy other related projects (Core, Infrastructure) if they exist
COPY ["src/Services/Reporting/CFMS.ReportingService.Core/CFMS.ReportingService.Core.csproj", "src/Services/Reporting/CFMS.ReportingService.Core/"]
COPY ["src/Services/Reporting/CFMS.ReportingService.Infrastructure/CFMS.ReportingService.Infrastructure.csproj", "src/Services/Reporting/CFMS.ReportingService.Infrastructure/"]
RUN dotnet restore "src/Services/Reporting/CFMS.ReportingService.API/CFMS.ReportingService.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Reporting/CFMS.ReportingService.API"
RUN dotnet build "CFMS.ReportingService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CFMS.ReportingService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CFMS.ReportingService.API.dll"]
