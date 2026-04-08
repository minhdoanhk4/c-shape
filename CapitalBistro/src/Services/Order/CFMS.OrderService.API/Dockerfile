FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Order/CFMS.OrderService.API/CFMS.OrderService.API.csproj", "src/Services/Order/CFMS.OrderService.API/CFMS.OrderService.API.csproj"]
# Copy other related projects (Core, Infrastructure) if they exist
COPY ["src/Services/Order/CFMS.OrderService.Core/CFMS.OrderService.Core.csproj", "src/Services/Order/CFMS.OrderService.Core/"]
COPY ["src/Services/Order/CFMS.OrderService.Infrastructure/CFMS.OrderService.Infrastructure.csproj", "src/Services/Order/CFMS.OrderService.Infrastructure/"]
COPY ["src/Shared/CFMS.Shared.Events/CFMS.Shared.Events.csproj", "src/Shared/CFMS.Shared.Events/"]
RUN dotnet restore "src/Services/Order/CFMS.OrderService.API/CFMS.OrderService.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Order/CFMS.OrderService.API"
RUN dotnet build "CFMS.OrderService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CFMS.OrderService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CFMS.OrderService.API.dll"]
