FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/ApiGateways/ShoppingAggregator/ShoppingAggregator.csproj", "src/ApiGateways/ShoppingAggregator/"]
RUN dotnet restore "src/ApiGateways/ShoppingAggregator/ShoppingAggregator.csproj"
COPY . .
WORKDIR "/src/src/ApiGateways/ShoppingAggregator"
RUN dotnet build "ShoppingAggregator.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "ShoppingAggregator.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingAggregator.dll"]
