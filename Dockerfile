FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY ["Src/Orders.Integrations.Hub/Orders.Integrations.Hub.csproj", "Src/Orders.Integrations.Hub/"]
RUN dotnet restore "Src/Orders.Integrations.Hub/Orders.Integrations.Hub.csproj" \
    --runtime linux-musl-x64

COPY . .
WORKDIR /src/Src/Orders.Integrations.Hub
RUN dotnet publish "Orders.Integrations.Hub.csproj" \
    -c Release \
    -o /app/publish \
    --runtime linux-musl-x64 \
    --self-contained false \
    /p:UseAppHost=false \
    /p:PublishSingleFile=false \
    /p:DebugType=None \
    /p:DebugSymbols=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final

# Run as non-root for security
RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser

WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Orders.Integrations.Hub.dll"]