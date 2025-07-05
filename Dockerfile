FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5077
EXPOSE 5077

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /

COPY ["Src/Orders.Integrations.Hub/Orders.Integrations.Hub.csproj", "Src/Orders.Integrations.Hub/"]
RUN dotnet restore "Src/Orders.Integrations.Hub/Orders.Integrations.Hub.csproj"

COPY . .

WORKDIR "Src/Orders.Integrations.Hub"
RUN dotnet build "Orders.Integrations.Hub.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Orders.Integrations.Hub.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orders.Integrations.Hub.dll"]