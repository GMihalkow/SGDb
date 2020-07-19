FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SGDb.Watchdog/SGDb.Watchdog.csproj", "SGDb.Watchdog/"]
RUN dotnet restore "SGDb.Watchdog/SGDb.Watchdog.csproj"
COPY . .
WORKDIR "/src/SGDb.Watchdog"
RUN dotnet build "SGDb.Watchdog.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "SGDb.Watchdog.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SGDb.Watchdog.dll"]