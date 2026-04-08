FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/API_Gateway/CFMS.Gateway.API/CFMS.Gateway.API.csproj", "src/API_Gateway/CFMS.Gateway.API/CFMS.Gateway.API.csproj"]
# Copy other related projects (Core, Infrastructure) if they exist
RUN dotnet restore "src/API_Gateway/CFMS.Gateway.API/CFMS.Gateway.API.csproj"
COPY . .
WORKDIR "/src/src/API_Gateway/CFMS.Gateway.API"
RUN dotnet build "CFMS.Gateway.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CFMS.Gateway.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CFMS.Gateway.API.dll"]
