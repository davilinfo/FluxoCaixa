#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CarrefourFluxoCaixa/CarrefourFluxoCaixa.csproj", "CarrefourFluxoCaixa/"]
RUN dotnet restore "CarrefourFluxoCaixa/CarrefourFluxoCaixa.csproj"
COPY . .
WORKDIR "/src/CarrefourFluxoCaixa"
RUN dotnet build "CarrefourFluxoCaixa.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarrefourFluxoCaixa.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarrefourFluxoCaixa.dll"]