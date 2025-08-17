# === Build stage ===
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore nad web csproj (cache-friendly)
COPY PojistovnaFullAspPrzeczek/PojistovnaFullAspPrzeczek.csproj PojistovnaFullAspPrzeczek/
RUN dotnet restore PojistovnaFullAspPrzeczek/PojistovnaFullAspPrzeczek.csproj

# Zdroj�ky jen web projektu
COPY PojistovnaFullAspPrzeczek/ PojistovnaFullAspPrzeczek/
RUN dotnet publish PojistovnaFullAspPrzeczek/PojistovnaFullAspPrzeczek.csproj \
    -c Release -o /app/publish /p:UseAppHost=false

# === Runtime stage ===
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# poslouchej uvnit� kontejneru na 8080 (Railway si to namapuje)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PojistovnaFullAspPrzeczek.dll"]