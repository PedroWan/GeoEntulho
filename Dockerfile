FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["backend/GeoEntulho.API/GeoEntulho.API.csproj", "backend/GeoEntulho.API/"]
RUN dotnet restore "backend/GeoEntulho.API/GeoEntulho.API.csproj"

COPY . .
WORKDIR "/src/backend/GeoEntulho.API"
RUN dotnet build "GeoEntulho.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeoEntulho.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GeoEntulho.API.dll"]
