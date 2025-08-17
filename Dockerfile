# === Build stage ===
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore nad web csproj (cache-friendly)
COPY PojistovnaFullAspPrzeczek/PojistovnaFullAspPrzeczek.csproj PojistovnaFullAspPrzeczek/
RUN dotnet restore PojistovnaFullAspPrzeczek/PojistovnaFullAspPrzeczek.csproj

# Zdrojáky jen web projektu
COPY PojistovnaFullAspPrzeczek/ PojistovnaFullAspPrzeczek/
RUN dotnet publish PojistovnaFullAspPrzeczek/PojistovnaFullAspPrzeczek.csproj \
    -c Release -o /app/publish /p:UseAppHost=false

# === Runtime stage ===
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Railway-friendly port binding
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
# bnd na dynamický PORT od Railway
ENV ASPNETCORE_URLS=http://+:${PORT}

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PojistovnaFullAspPrzeczek.dll"]