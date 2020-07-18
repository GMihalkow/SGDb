FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SGDb.Identity/SGDb.Identity.csproj", "SGDb.Identity/"]
COPY ["SGDb.Common/SGDb.Common.csproj", "SGDb.Common/"]
RUN dotnet restore "SGDb.Identity/SGDb.Identity.csproj"
COPY . .
WORKDIR "/src/SGDb.Identity"
RUN dotnet build "SGDb.Identity.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "SGDb.Identity.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SGDb.Identity.dll"]