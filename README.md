[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey.svg)](https://www.microsoft.com/windows)
[![Downloads](https://img.shields.io/github/downloads/cagritaskn/GoodbyeDPI-Turkey/total.svg)](https://github.com/cagritaskn/GoodbyeDPI-Turkey/releases/)

# DPIA - Dynamic DPI Assistant 🚀
[Download/Yükle](https://github.com/xassiver/dpi/releases/tag/APP)

[Turkish](#türkçe) | [English](#english)

---

## Türkçe

DPIA, internet sansürünü ve derin paket inceleme (DPI) engellerini aşmak için tasarlanmış, kullanıcı dostu ve akıllı bir Windows terminal uygulamasıdır. Teknik bilgi gerektirmeden, saniyeler içinde internetinizi özgürleştirir.

### ✨ Öne Çıkan Özellikler

*   **Akıllı Kurulum (İteratif Deneme):** DPIA, bölgeniz ve ISS'niz (Süperonline, Türk Telekom vb.) için en uygun ayarı bulana kadar farklı profilleri otomatik olarak dener.
*   **Sayısal Menü Sistemi:** Karmaşık komutlar yerine `1`, `2`, `3` gibi sayılarla kolay yönetim.
*   **Düşük Kaynak Tüketimi:** Bir Windows Servisi olarak arka planda çalışır; CPU'yu yormaz, FPS veya Ping (MS) değerlerini etkilemez.
*   **Durum Takibi:** Uygulamayı her açtığınızda sistem durumunu (KURULU / DEĞİL) anında raporlar.
*   **Uzman Modu:** İleri düzey kullanıcılar için manuel parametre (TTL, DNS vb.) belirleme imkanı.

### 🛠️ Nasıl Kullanılır?

1.  `DPIA.exe` dosyasını indirin.
2.  Dosyayı **Yönetici Olarak** çalıştırın.
3.  `1` tuşuna basarak akıllı kurulumu başlatın.
4.  Ekranda çıkan "Erişim var mı?" sorularını yanıtlayarak kurulumu tamamlayın.

---

# Kaspersky Antivirüsü Hakkında Önemli Not
>
> [!CAUTION]
> Kaspersky antivirüs yazılımı, GoodbyeDPI çekirdeğinin çalışmasına engel olmaktadır. Kaspersky kullanıyorsanız, devre dışı bırakmanız veya dışlamalara eklemeniz çoğu zaman yeterli olmaz; yazılımı tamamen kaldırmanız gerekebilir. Alternatif olarak Windows Defender kullanabilirsiniz.

## Virüs & Veri Sızıntısı & Bitcoin Mining

DPIA açık kaynak kodludur. Bazı antivirüs yazılımları, paket yakalama ve manipülasyon için kullanılan `WinDivert.dll` ve `WinDivert64.sys` dosyalarını "zararlı" olarak işaretleyebilir (False Positive). Bu dosyalar paket başlıklarını değiştirmek için sisteme düşük seviyeli erişim sağlar.

> [!IMPORTANT]
> WinDivert dosyalarının açıklamalarında görünen Bitcoin adresi, kütüphanenin asıl geliştiricisi olan [basil00](https://github.com/basil00)'a ait bağış adresidir. Bu uygulama ile herhangi bir veri sızıntısı veya mining söz konusu değildir.

## Sık Karşılaşılan Sorunlar

- **WinDivert dosyaları bulunamadı hatası:** Antivirüs programınız dosyaları silmiş olabilir. DPIA klasörünü antivirüsünüzde "Dışlamalar/İstisnalar" listesine ekleyin.
- **Servis başlatılamadı hatası:** DPIA'yı mutlaka **Yönetici Olarak** çalıştırdığınızdan emin olun.
- **Siteler yavaş açılıyor veya açılmıyor:** İteratif kurulumda farklı profilleri deneyin. Özellikle Superonline Fiber kullanıcıları Expert Mode'da (Seçenek 2) farklı TTL değerleri (3, 4 veya 5) denemelidir.
- **Discord Bağlantı Sorunu:** Discord uygulaması açılmıyorsa, web üzerinden giriş yapmayı deneyin. Eğer webde sorun yoksa ancak uygulamada varsa, arka planda çalışan diğer WinDivert kullanan uygulamaları kapatın.

---

## English

DPIA is a user-friendly and intelligent Windows terminal application designed to bypass internet censorship and Deep Packet Inspection (DPI) blocks. It liberates your internet connection in seconds, requiring zero technical knowledge.

### ✨ Key Features

*   **Smart Installation (Iterative Trial):** DPIA automatically tests different profiles until it finds the best configuration for your region and ISP.
*   **Numeric Menu System:** Easy management using simple numbers like `1`, `2`, `3` instead of complex commands.
*   **Low Resource Usage:** Runs in the background as a Windows Service; it's extremely lightweight and won't affect your FPS or Ping (MS).
*   **Status Tracking:** Reports the system status (INSTALLED / NOT INSTALLED) immediately every time you open the app.
*   **Expert Mode:** Allows advanced users to manually set parameters like TTL and DNS redirection.

### 🛠️ How to Use?

1.  Download the `DPIA.exe` file.
2.  Run the file **as Administrator**.
3.  Press `1` to start the smart installation.
4.  Answer the "Is there access?" questions to finalize the setup.

---

### 💻 Developer Information

DPIA is an open-source tool. It works by installing a background service that intelligently manipulates packet headers.

- **Source Code:** [Program.cs](file:///c:/Users/Orhan/Desktop/Programlar/PROJELER/DPIA/Program.cs)
- **Build Script:** [BUILD.bat](file:///c:/Users/Orhan/Desktop/Programlar/PROJELER/DPIA/BUILD.bat)

*DPIA is a standalone project focused on internet freedom.*
