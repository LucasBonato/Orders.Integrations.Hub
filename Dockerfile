FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /app
EXPOSE 5077

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /

COPY ["Src/BizPik.Orders.Hub/BizPik.Orders.Hub.csproj", "Src/BizPik.Orders.Hub/"]
RUN dotnet restore "Src/BizPik.Orders.Hub/BizPik.Orders.Hub.csproj"

COPY . .

WORKDIR "Src/BizPik.Orders.Hub"
RUN dotnet build "BizPik.Orders.Hub.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BizPik.Orders.Hub.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BizPik.Orders.Hub.dll"]