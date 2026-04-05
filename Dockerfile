# .NET SDK görüntüsünü kullanarak derleme yapalım
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Proje dosyalarını kopyalayıp bağımlılıkları yükleyelim
COPY *.csproj ./
RUN dotnet restore

# Tüm dosyaları kopyalayıp yayınlayalım
COPY . ./
RUN dotnet publish -c Release -o out

# Çalışma zamanı görüntüsüne geçelim
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Uygulamayı başlatalım
ENTRYPOINT ["dotnet", "PortfolyoProjesi.dll"]