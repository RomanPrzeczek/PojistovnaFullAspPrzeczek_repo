# === Build stage ===
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1) jen csproj kvùli cache restore
COPY PojistovnaFullAspPrzeczek/PojistovnaFullAspPrzeczek.csproj PojistovnaFullAspPrzeczek/
RUN dotnet restore PojistovnaFullAspPrzeczek/PojistovnaFullAspPrzeczek.csproj

# 2) jen zdrojáky web projektu (ne celé repo)
COPY PojistovnaFullAspPrzeczek/ PojistovnaFullAspPrzeczek/
RUN dotnet publish PojistovnaFullAspPrzeczek/PojistovnaFullAspPrzeczek.csproj \
    -c Release -o /app/publish /p:UseAppHost=false

# === Runtime stage ===
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Railway friendly listening
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PojistovnaFullAspPrzeczek.dll"]