<div align="center">
  <a href="https://github.com/ReyhanClskn/AgizDisSagligiTakip"></a>

  <h3 align="center">Ağız ve Diş Sağlığı Takip Uygulaması</h3>

  <p align="center">
    Sağlıklı bir gülümseme için günlük alışkanlıklarınızı takip edin!
    <br />
    <a href="https://github.com/ReyhanClskn/AgizDisSagligiTakip"><strong>Dokümantasyonu İncele »</strong></a>
    <br />
    <br />
  </p>
</div>

<!-- BADGES -->
<div align="center">

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]

</div>

<!-- İÇERİK -->
<details>
  <summary>İçindekiler</summary>
  <ol>
    <li>
      <a href="#-proje-hakkında">Proje Hakkında</a>
      <ul>
        <li><a href="#-öne-çıkan-özellikler">Öne Çıkan Özellikler</a></li>
        <li><a href="#-kullanılan-teknolojiler">Kullanılan Teknolojiler</a></li>
      </ul>
    </li>
    <li>
      <a href="#-başlarken">Başlarken</a>
      <ul>
        <li><a href="#-gereksinimler">Gereksinimler</a></li>
        <li><a href="#-kurulum">Kurulum</a></li>
      </ul>
    </li>
    <li><a href="#-kullanım">Kullanım</a></li>
    <li><a href="#-yol-haritası">Yol Haritası</a></li>
    <li><a href="#-katkıda-bulunma">Katkıda Bulunma</a></li>
    <li><a href="#-lisans">Lisans</a></li>
    <li><a href="#-iletişim">İletişim</a></li>
    <li><a href="#-teşekkürler">Teşekkürler</a></li>
  </ol>
</details>

<!-- PROJE HAKKINDA -->
## 📋 Proje Hakkında

<div align="center">
  <img src="https://raw.githubusercontent.com/ReyhanClskn/AgizDisSagligiTakip/main/images/screenshot.png" alt="Uygulama Ekran Görüntüsü" width="600">
</div>

Bu proje, kullanıcıların ağız ve diş sağlığı alışkanlıklarını takip etmelerine yardımcı olan kapsamlı bir web uygulamasıdır. Kullanıcılar günlük diş fırçalama, diş ipi kullanımı ve ağız gargarası kullanımı gibi alışkanlıklarını kaydedebilir, hedefler belirleyebilir ve ilerlemelerini takip edebilirler.

### ✨ Öne Çıkan Özellikler

* 🔐 **Güvenli Kullanıcı Sistemi** - Email doğrulamalı kayıt ve giriş
* 📊 **İlerleme Takibi** - Günlük, haftalık ve aylık raporlar
* 🎯 **Hedef Belirleme** - Kişiselleştirilmiş sağlık hedefleri
* 📱 **Responsive Tasarım** - Her cihazda mükemmel görünüm
* 💡 **Sağlık Önerileri** - Uzman tavsiyeli öneriler

### 🛠️ Kullanılan Teknolojiler

Bu proje aşağıdaki teknolojiler kullanılarak geliştirilmiştir:

* [![.NET][.NET]][.NET-url]
* [![C#][CSharp]][CSharp-url]
* [![Entity Framework][EF]][EF-url]
* [![SQL Server][SQLServer]][SQLServer-url]
* [![Bootstrap][Bootstrap.com]][Bootstrap-url]
* [![jQuery][JQuery.com]][JQuery-url]

<!-- BAŞLARKEN -->
## 🚀 Başlarken

Projeyi lokal ortamınızda çalıştırmak için aşağıdaki adımları takip edin.

### 📋 Gereksinimler

* .NET 8.0 SDK
  ```sh
  # .NET versiyonunu kontrol edin
  dotnet --version
  ```
* SQL Server veya PostgreSQL
* Visual Studio 2022 veya VS Code
* Git

### 🔧 Kurulum

1. **Repoyu klonlayın**
   ```sh
   git clone https://github.com/ReyhanClskn/AgizDisSagligiTakip.git
   cd AgizDisSagligiTakip
   ```

2. **Veritabanı bağlantısını yapılandırın**
   
   `appsettings.json` dosyasını düzenleyin:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=AgizDisSagligiDB;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
   ```

3. **Email ayarlarını yapılandırın**

   ### 📧 Gmail App Password Oluşturma:
   
   1. **Google Hesabınızda 2 Faktörlü Doğrulamayı Aktifleştirin:**
      - [myaccount.google.com/security](https://myaccount.google.com/security) adresine gidin
      - "2-Step Verification" (2 Adımlı Doğrulama) tıklayın
      - Adımları takip ederek aktifleştirin
   
   2. **App Password Oluşturun:**
      - [myaccount.google.com/apppasswords](https://myaccount.google.com/apppasswords) adresine gidin
      - Yeni bir App Password oluşturun
      - "AgizDisSagligi" gibi bir isim verin
      - "Generate" butonuna tıklayın
      - **16 haneli şifreyi kopyalayın** ve güvenli bir yere kaydedin
        
   3. **EmailService.cs Dosyasını Güncelleyin:**
   ```cs
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _fromEmail = "mail adresiniz";
        private readonly string _fromPassword = "app passwordünüz"; 
   ```
   
   4. **appsettings.json Dosyasını Güncelleyin:**
   ```json
   {
     "EmailSettings": {
      "SmtpServer": "smtp.gmail.com",
      "SmtpPort": 587,
      "SenderEmail": "mail adresiniz",
      "SenderPassword": "app passswordünüz", 
      "SenderName": "Ağız ve Diş Sağlığı Takip"
     }
   }
   ```

4. **Veritabanını oluşturun**
   ```sh
   dotnet ef database update
   ```

5. **Uygulamayı çalıştırın**
   ```sh
   dotnet run --project AgizDisSagligiTakip.Web
   ```

6. **Tarayıcınızda açın**
   <br/>
   Terminale basılan adrese gidin
   ```
   https://localhost:5021
   ```

<!-- KULLANIM -->
## 💡 Kullanım

### İlk Adımlar

1. **Kayıt Olun** - Email adresiniz ve gereksinimleri karşılayan bir şifre ile kayıt olun
2. **Email Doğrulama** - Gelen kutunuzu kontrol edin ve hesabınızı doğrulayın
3. **Hedef Belirleyin** - Günlük diş fırçalama, diş ipi kullanımı gibi hedefler ekleyin
4. **Takip Edin** - Her gün alışkanlıklarınızı kaydedin

<!-- YOL HARİTASI -->
## 🗺️ Yol Haritası

- [x] Temel kullanıcı yönetimi
- [x] Email doğrulama sistemi
- [x] Hedef oluşturma ve takip
- [x] Günlük veri girişi
- [ ] Push bildirimler
- [ ] AI destekli öneriler
- [ ] Çoklu dil desteği
  - [ ] İngilizce
- [ ] Karanlık/Aydınlık tema

Planlanan özellikler için [issues](https://github.com/ReyhanClskn/AgizDisSagligiTakip/issues) sayfasına bakın.

<!-- KATKIDA BULUNMA -->
## 🤝 Katkıda Bulunma

Projeye katkıda bulunmak isterseniz:

1. Projeyi Fork'layın
2. Feature Branch oluşturun (`git checkout -b feature/HarikaOzellik`)
3. Değişikliklerinizi Commit'leyin (`git commit -m 'HarikaOzellik eklendi'`)
4. Branch'e Push'layın (`git push origin feature/HarikaOzellik`)
5. Pull Request açın

<!-- LİSANS -->
## 📄 Lisans

Bu proje henüz bir lisansa sahip değil.

<!-- İLETİŞİM -->
## 📧 İletişim

reyhanclksn0404@gmail.com

<!-- TEŞEKKÜRLER -->
## 🙏 Teşekkürler

[TRtek Yazılım](https://trtekyazilim.com) - Staj fırsatı için

<p align="right">(<a href="#readme-top">başa dön</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
[contributors-shield]: https://img.shields.io/github/contributors/ReyhanClskn/AgizDisSagligiTakip.svg?style=for-the-badge
[contributors-url]: https://github.com/ReyhanClskn/AgizDisSagligiTakip/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/ReyhanClskn/AgizDisSagligiTakip.svg?style=for-the-badge
[forks-url]: https://github.com/ReyhanClskn/AgizDisSagligiTakip/network/members
[stars-shield]: https://img.shields.io/github/stars/ReyhanClskn/AgizDisSagligiTakip.svg?style=for-the-badge
[stars-url]: https://github.com/ReyhanClskn/AgizDisSagligiTakip/stargazers
[issues-shield]: https://img.shields.io/github/issues/ReyhanClskn/AgizDisSagligiTakip.svg?style=for-the-badge
[issues-url]: https://github.com/ReyhanClskn/AgizDisSagligiTakip/issues
[license-shield]: https://img.shields.io/github/license/ReyhanClskn/AgizDisSagligiTakip.svg?style=for-the-badge
[license-url]: https://github.com/ReyhanClskn/AgizDisSagligiTakip/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/ReyhanClskn

<!-- Teknoloji Badge'leri -->
[.NET]: https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[.NET-url]: https://dotnet.microsoft.com/
[CSharp]: https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white
[CSharp-url]: https://docs.microsoft.com/en-us/dotnet/csharp/
[EF]: https://img.shields.io/badge/Entity_Framework-512BD4?style=for-the-badge&logo=.net&logoColor=white
[EF-url]: https://docs.microsoft.com/en-us/ef/
[SQLServer]: https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white
[SQLServer-url]: https://www.microsoft.com/en-us/sql-server
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[JQuery.com]: https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[JQuery-url]: https://jquery.com
