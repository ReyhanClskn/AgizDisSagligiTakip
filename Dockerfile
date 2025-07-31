# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Solution ve proje dosyalarını kopyala
COPY ["AgizDisSagligiTakip.sln", "./"]
COPY ["AgizDisSagligiTakip.Web/AgizDisSagligiTakip.Web.csproj", "AgizDisSagligiTakip.Web/"]
COPY ["AgizDisSagligiTakip.Business/AgizDisSagligiTakip.Business.csproj", "AgizDisSagligiTakip.Business/"]
COPY ["AgizDisSagligiTakip.Core/AgizDisSagligiTakip.Core.csproj", "AgizDisSagligiTakip.Core/"]
COPY ["AgizDisSagligiTakip.Data/AgizDisSagligiTakip.Data.csproj", "AgizDisSagligiTakip.Data/"]
COPY ["AgizDisSagligiTakip.Tests/AgizDisSagligiTakip.Tests.csproj", "AgizDisSagligiTakip.Tests/"]

# NuGet paketlerini geri yükle
RUN dotnet restore "AgizDisSagligiTakip.sln"

# Tüm kaynak kodları kopyala
COPY . .

# Build işlemi
WORKDIR "/src/AgizDisSagligiTakip.Web"
RUN dotnet build "AgizDisSagligiTakip.Web.csproj" -c Release -o /app/build

# Publish aşaması
FROM build AS publish
RUN dotnet publish "AgizDisSagligiTakip.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime aşaması - SDK image kullan (EF Tools için)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app

# Entity Framework CLI tools'u yükle
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# uploads klasörü için volume tanımla
VOLUME ["/app/wwwroot/uploads"]

# Port ayarları
EXPOSE 80
EXPOSE 443

# Publish edilen dosyaları kopyala
COPY --from=publish /app/publish .

# Proje dosyalarını da kopyala TODO:(EF için gerekli)
COPY --from=build /src .

# Giriş noktası
ENTRYPOINT ["dotnet", "AgizDisSagligiTakip.Web.dll"]