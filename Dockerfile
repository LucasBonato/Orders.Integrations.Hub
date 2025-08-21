FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /

COPY ["Src/Orders.Integrations.Hub/Orders.Integrations.Hub.csproj", "Src/Orders.Integrations.Hub/"]
RUN dotnet restore "Src/Orders.Integrations.Hub/Orders.Integrations.Hub.csproj"

COPY . .
WORKDIR /Src/Orders.Integrations.Hub
RUN dotnet build "Orders.Integrations.Hub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orders.Integrations.Hub.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Orders.Integrations.Hub.dll"]