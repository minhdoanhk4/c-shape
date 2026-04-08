FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Auth/CFMS.AuthService.API/CFMS.AuthService.API.csproj", "src/Services/Auth/CFMS.AuthService.API/CFMS.AuthService.API.csproj"]
# Copy other related projects (Core, Infrastructure) if they exist
COPY ["src/Services/Auth/CFMS.AuthService.Core/CFMS.AuthService.Core.csproj", "src/Services/Auth/CFMS.AuthService.Core/"]
COPY ["src/Services/Auth/CFMS.AuthService.Infrastructure/CFMS.AuthService.Infrastructure.csproj", "src/Services/Auth/CFMS.AuthService.Infrastructure/"]
RUN dotnet restore "src/Services/Auth/CFMS.AuthService.API/CFMS.AuthService.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Auth/CFMS.AuthService.API"
RUN dotnet build "CFMS.AuthService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CFMS.AuthService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CFMS.AuthService.API.dll"]
