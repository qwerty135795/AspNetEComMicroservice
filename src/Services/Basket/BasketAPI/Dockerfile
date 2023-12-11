FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80


FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Services/Basket/BasketAPI/BasketAPI.csproj", "src/Services/Basket/BasketAPI/"]
COPY ["src/BuildingBlocks/EventBusMessages/EventBusMessages.csproj", "src/BuildingBlocks/EventBusMessages/"]
RUN dotnet restore "src/Services/Basket/BasketAPI/BasketAPI.csproj"
COPY . .
WORKDIR "/src/src/Services/Basket/BasketAPI"
RUN dotnet build "BasketAPI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "BasketAPI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasketAPI.dll"]
