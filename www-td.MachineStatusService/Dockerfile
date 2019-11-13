FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["www-td.MachineStatusService/www-td.MachineStatusService.csproj", "www-td.MachineStatusService/"]
RUN dotnet restore "www-td.MachineStatusService/www-td.MachineStatusService.csproj"
COPY . .
WORKDIR "/src/www-td.MachineStatusService"
RUN dotnet build "www-td.MachineStatusService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "www-td.MachineStatusService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "www-td.MachineStatusService.dll"]