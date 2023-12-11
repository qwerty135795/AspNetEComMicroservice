FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Services/Ordering/OrderingAPI/OrderingAPI.csproj", "src/Services/Ordering/OrderingAPI/"]
COPY ["src/Services/Ordering/OrderingDomain/OrderingDomain.csproj", "src/Services/Ordering/OrderingDomain/"]
COPY ["src/Services/Ordering/OrderingApplication/OrderingApplication.csproj", "src/Services/Ordering/OrderingApplication/"]
COPY ["src/Services/Ordering/OrderingInfrastructure/OrderingInfrastructure.csproj", "src/Services/Ordering/OrderingInfrastructure/"]
COPY ["src/BuildingBlocks/EventBusMessages/EventBusMessages.csproj", "src/BuildingBlocks/EventBusMessages/"]
RUN dotnet restore "src/Services/Ordering/OrderingAPI/OrderingAPI.csproj"
COPY . .
WORKDIR "/src/src/Services/Ordering/OrderingAPI"
RUN dotnet build "OrderingAPI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "OrderingAPI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderingAPI.dll"]
