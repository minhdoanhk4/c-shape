FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Delivery/CFMS.DeliveryService.API/CFMS.DeliveryService.API.csproj", "src/Services/Delivery/CFMS.DeliveryService.API/CFMS.DeliveryService.API.csproj"]
# Copy other related projects (Core, Infrastructure) if they exist
COPY ["src/Services/Delivery/CFMS.DeliveryService.Core/CFMS.DeliveryService.Core.csproj", "src/Services/Delivery/CFMS.DeliveryService.Core/"]
COPY ["src/Services/Delivery/CFMS.DeliveryService.Infrastructure/CFMS.DeliveryService.Infrastructure.csproj", "src/Services/Delivery/CFMS.DeliveryService.Infrastructure/"]
RUN dotnet restore "src/Services/Delivery/CFMS.DeliveryService.API/CFMS.DeliveryService.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Delivery/CFMS.DeliveryService.API"
RUN dotnet build "CFMS.DeliveryService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CFMS.DeliveryService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CFMS.DeliveryService.API.dll"]
