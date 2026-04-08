FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Franchise/CFMS.FranchiseService.API/CFMS.FranchiseService.API.csproj", "src/Services/Franchise/CFMS.FranchiseService.API/CFMS.FranchiseService.API.csproj"]
# Copy other related projects (Core, Infrastructure) if they exist
COPY ["src/Services/Franchise/CFMS.FranchiseService.Core/CFMS.FranchiseService.Core.csproj", "src/Services/Franchise/CFMS.FranchiseService.Core/"]
COPY ["src/Services/Franchise/CFMS.FranchiseService.Infrastructure/CFMS.FranchiseService.Infrastructure.csproj", "src/Services/Franchise/CFMS.FranchiseService.Infrastructure/"]
RUN dotnet restore "src/Services/Franchise/CFMS.FranchiseService.API/CFMS.FranchiseService.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Franchise/CFMS.FranchiseService.API"
RUN dotnet build "CFMS.FranchiseService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CFMS.FranchiseService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CFMS.FranchiseService.API.dll"]
