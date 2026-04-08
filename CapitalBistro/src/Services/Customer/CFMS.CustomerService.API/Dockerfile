FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Customer/CFMS.CustomerService.API/CFMS.CustomerService.API.csproj", "src/Services/Customer/CFMS.CustomerService.API/CFMS.CustomerService.API.csproj"]
# Copy other related projects (Core, Infrastructure) if they exist
COPY ["src/Services/Customer/CFMS.CustomerService.Core/CFMS.CustomerService.Core.csproj", "src/Services/Customer/CFMS.CustomerService.Core/"]
COPY ["src/Services/Customer/CFMS.CustomerService.Infrastructure/CFMS.CustomerService.Infrastructure.csproj", "src/Services/Customer/CFMS.CustomerService.Infrastructure/"]
COPY ["src/Shared/CFMS.Shared.Events/CFMS.Shared.Events.csproj", "src/Shared/CFMS.Shared.Events/"]
RUN dotnet restore "src/Services/Customer/CFMS.CustomerService.API/CFMS.CustomerService.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Customer/CFMS.CustomerService.API"
RUN dotnet build "CFMS.CustomerService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CFMS.CustomerService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CFMS.CustomerService.API.dll"]
