# SDK 9.0 (En güncel sürüm) kullanarak derleyelim
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Proje dosyasını kopyalayıp bağımlılıkları yükleyelim
COPY *.csproj ./
RUN dotnet restore

# Tüm dosyaları kopyalayıp yayınlayalım
COPY . ./
RUN dotnet publish -c Release -o out

# Çalışma zamanı (Runtime) görüntüsü için de 9.0 kullanalım
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Render port ayarı (Kritik!)
ENV ASPNETCORE_URLS=http://+:10000

ENTRYPOINT ["dotnet", "PortfolyoProjesi.dll"]