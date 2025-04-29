# Unter https://aka.ms/customizecontainer erfahren Sie, wie Sie Ihren Debugcontainer anpassen und wie Visual Studio dieses Dockerfile verwendet, um Ihre Images für ein schnelleres Debuggen zu erstellen.

# Diese Stufe wird verwendet, wenn sie von VS im Schnellmodus ausgeführt wird (Standardeinstellung für Debugkonfiguration).
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app


# Diese Stufe wird zum Erstellen des Dienstprojekts verwendet.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["xyPorts/xyPorts.csproj", "xyPorts/"]
RUN dotnet restore "./xyPorts/xyPorts.csproj"
COPY . .
WORKDIR "/src/xyPorts"
RUN dotnet build "./xyPorts.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Diese Stufe wird verwendet, um das Dienstprojekt zu veröffentlichen, das in die letzte Phase kopiert werden soll.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./xyPorts.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Diese Stufe wird in der Produktion oder bei Ausführung von VS im regulären Modus verwendet (Standard, wenn die Debugkonfiguration nicht verwendet wird).
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "xyPorts.dll"]