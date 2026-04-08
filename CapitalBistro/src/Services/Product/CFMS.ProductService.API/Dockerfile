FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Product/CFMS.ProductService.API/CFMS.ProductService.API.csproj", "src/Services/Product/CFMS.ProductService.API/CFMS.ProductService.API.csproj"]
# Copy other related projects (Core, Infrastructure) if they exist
COPY ["src/Services/Product/CFMS.ProductService.Core/CFMS.ProductService.Core.csproj", "src/Services/Product/CFMS.ProductService.Core/"]
COPY ["src/Services/Product/CFMS.ProductService.Infrastructure/CFMS.ProductService.Infrastructure.csproj", "src/Services/Product/CFMS.ProductService.Infrastructure/"]
RUN dotnet restore "src/Services/Product/CFMS.ProductService.API/CFMS.ProductService.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Product/CFMS.ProductService.API"
RUN dotnet build "CFMS.ProductService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CFMS.ProductService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CFMS.ProductService.API.dll"]
