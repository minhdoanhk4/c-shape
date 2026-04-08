FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Payment/CFMS.PaymentService.API/CFMS.PaymentService.API.csproj", "src/Services/Payment/CFMS.PaymentService.API/CFMS.PaymentService.API.csproj"]
# Copy other related projects (Core, Infrastructure) if they exist
COPY ["src/Services/Payment/CFMS.PaymentService.Core/CFMS.PaymentService.Core.csproj", "src/Services/Payment/CFMS.PaymentService.Core/"]
COPY ["src/Services/Payment/CFMS.PaymentService.Infrastructure/CFMS.PaymentService.Infrastructure.csproj", "src/Services/Payment/CFMS.PaymentService.Infrastructure/"]
RUN dotnet restore "src/Services/Payment/CFMS.PaymentService.API/CFMS.PaymentService.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Payment/CFMS.PaymentService.API"
RUN dotnet build "CFMS.PaymentService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CFMS.PaymentService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CFMS.PaymentService.API.dll"]
