FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["KeToanGiaThanhSanPham.csproj", "."]
RUN dotnet restore --no-cache
COPY . .
RUN dotnet publish -c Release -o /app/publish \
    --no-self-contained \
    -p:PublishReadyToRun=false \
    -p:TieredCompilation=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_GCConserveMemory=9
ENTRYPOINT ["dotnet", "KeToanGiaThanhSanPham.dll"]
