FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Shift/CFMS.ShiftService.API/CFMS.ShiftService.API.csproj", "src/Services/Shift/CFMS.ShiftService.API/CFMS.ShiftService.API.csproj"]
# Copy other related projects (Core, Infrastructure) if they exist
COPY ["src/Services/Shift/CFMS.ShiftService.Core/CFMS.ShiftService.Core.csproj", "src/Services/Shift/CFMS.ShiftService.Core/"]
COPY ["src/Services/Shift/CFMS.ShiftService.Infrastructure/CFMS.ShiftService.Infrastructure.csproj", "src/Services/Shift/CFMS.ShiftService.Infrastructure/"]
RUN dotnet restore "src/Services/Shift/CFMS.ShiftService.API/CFMS.ShiftService.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Shift/CFMS.ShiftService.API"
RUN dotnet build "CFMS.ShiftService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CFMS.ShiftService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CFMS.ShiftService.API.dll"]
