FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Promotion/CFMS.PromotionService.API/CFMS.PromotionService.API.csproj", "src/Services/Promotion/CFMS.PromotionService.API/CFMS.PromotionService.API.csproj"]
# Copy other related projects (Core, Infrastructure) if they exist
COPY ["src/Services/Promotion/CFMS.PromotionService.Core/CFMS.PromotionService.Core.csproj", "src/Services/Promotion/CFMS.PromotionService.Core/"]
COPY ["src/Services/Promotion/CFMS.PromotionService.Infrastructure/CFMS.PromotionService.Infrastructure.csproj", "src/Services/Promotion/CFMS.PromotionService.Infrastructure/"]
RUN dotnet restore "src/Services/Promotion/CFMS.PromotionService.API/CFMS.PromotionService.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Promotion/CFMS.PromotionService.API"
RUN dotnet build "CFMS.PromotionService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CFMS.PromotionService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CFMS.PromotionService.API.dll"]
