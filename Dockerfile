FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PojistovnaFullAspPrzeczek/PojistovnaFullAspPrzeczek.csproj", "PojistovnaFullAspPrzeczek/"]
RUN dotnet restore "PojistovnaFullAspPrzeczek/PojistovnaFullAspPrzeczek.csproj"
COPY . .
WORKDIR "/src/PojistovnaFullAspPrzeczek"
RUN dotnet build "PojistovnaFullAspPrzeczek.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PojistovnaFullAspPrzeczek.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PojistovnaFullAspPrzeczek.dll"]
