# 00 – Hazırlık (tüm testlerden önce)

## Gereken ortam

- Windows 10 veya 11, **64 bit** (installer 32 bit Windows’a kurulmaz).
- **.NET Framework 4.8** (Windows 10 1903+ ve Windows 11’de zaten yüklü). Yoksa:
  https://dotnet.microsoft.com/download/dotnet-framework/net48
- En az **iki tarayıcı**: Microsoft Edge (Windows’ta hazır) + Google Chrome veya Firefox.
  - Chrome: https://www.google.com/chrome/
  - Firefox: https://www.mozilla.org/firefox/new/
- Testlerin bir kısmı için (08) bir portable tarayıcı önerilir, reçetesinde link var.

## 1. Kurulum

1. Son sürümü indir: https://github.com/snipeTR/BrowserSelect/releases/latest
   (doğrudan: https://github.com/snipeTR/BrowserSelect/releases/download/v1.4.1.0-build.4/BrowserSelect-1.4.1.0-x64-Setup.exe)
2. `BrowserSelect-...-x64-Setup.exe` dosyasını çalıştır.
   - Installer imzasız olduğu için **SmartScreen** “Windows bilgisayarınızı korudu” uyarısı çıkabilir:
     `More info` / `Daha fazla bilgi` → `Run anyway` / `Yine de çalıştır`.
3. Kurulum yönetici izni istemez; varsayılan klasör: `%LOCALAPPDATA%\BrowserSelect`.
4. Başlat menüsünde **BrowserSelect** kısayolu oluşmalı.

**Kontrol:** Başlat menüsünden BrowserSelect’i aç → yüklü tarayıcıların ikonlarını gösteren küçük bir
pencere açılmalı. Sağ kenarda dikey `About` ve `Settings` butonları vardır.

## 2. Test dosyalarını bilgisayara al

1. Depoyu ZIP olarak indir: https://github.com/snipeTR/BrowserSelect/archive/refs/heads/master.zip
2. ZIP’i örneğin `C:\bs-test` klasörüne çıkar. Reçetelerdeki dosyalar şu klasörde olacak:
   `C:\bs-test\BrowserSelect-master\Tests\human_test\`
3. (İnternetten indirilen dosyalar “engelli” işaretlenir.) PowerShell’de bir kez çalıştır:

   ```powershell
   Get-ChildItem C:\bs-test -Recurse | Unblock-File
   ```

## 3. Test aracı: `tools\bs-open.ps1`

Kural testlerinin çoğunda BrowserSelect’i varsayılan tarayıcı yapmana gerek yok: bu betik
`BrowserSelect.exe`’yi verdiğin linkle, Windows’un link tıklandığında yaptığı gibi başlatır.

PowerShell’i aç ve şunları yaz (her yeni PowerShell penceresinde bir kez):

```powershell
cd C:\bs-test\BrowserSelect-master\Tests\human_test\tools
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
```

Kullanım örnekleri:

```powershell
.\bs-open.ps1 https://example.com/              # linki BrowserSelect ile aç
.\bs-open.ps1 https://example.com/ -Delay 3     # 3 sn bekleyip aç (Alt testi için)
.\bs-open.ps1 https://example.com/ -Maximized   # "maximize" durumunda başlat (01 testi)
.\bs-open.ps1 https://example.com/ -ViaShell    # Windows varsayılan tarayıcısı üzerinden aç
```

**Kontrol:** `.\bs-open.ps1 https://example.com/` → BrowserSelect penceresi açılmalı ve başlığında
`https://example.com/` yazmalı. Bir tarayıcıya tıklayınca sayfa o tarayıcıda açılmalı.

> Betik BrowserSelect’i PowerShell’den başlattığı için, BrowserSelect “linki açan uygulama” olarak
> `powershell.exe` (PowerShell 7’de `pwsh.exe`) görür. Bu, 02’deki **Source App** testinde işimize yarıyor.

## 4. (Bazı testler için) BrowserSelect’i varsayılan tarayıcı yapmak

Gerçek hayattaki “başka bir uygulamada linke tıkla” senaryosu (Outlook, Teams, Slack…) ve 03 / 09 testlerinin
bir kısmı için gerekir:

1. BrowserSelect’i Başlat menüsünden aç → sağdaki `Settings`.
2. `Default Browser` bölümünde `Set as Default Browser`.
3. Açılan Windows ekranında (Ayarlar > Uygulamalar > Varsayılan uygulamalar > BrowserSelect)
   **HTTP** ve **HTTPS** için BrowserSelect’i seç.
4. Kontrol: `.\bs-open.ps1 https://example.com/ -ViaShell` BrowserSelect penceresini açmalı.

Testler bitince eski tarayıcını aynı ekrandan geri varsayılan yapabilirsin.

## 5. Ayarlarını yedekle (önerilir)

Testler kural ekleyip siler, sıralamayı ve seçenekleri değiştirir. Mevcut ayarların varsa önce yedekle:

- `Settings` → `Options` bölümü → `Export...` → örn. `Masaüstü\BrowserSelect-yedek.json`.

Testler bitince `Import...` ile bu dosyayı geri yükleyebilirsin (bkz. [07](07-export-import.md)).

## 6. Temiz başlangıç

Her reçete kendi kurallarını ekler. Bir reçeteye başlamadan önce:

- `Settings` → `Auto Select Filters` listesinde önceki testten kalan kuralları seç → `Delete` → `Apply`.
- `Options` bölümünde `Show running browsers only` **kapalı**, `Hold Alt on a link to skip rules` **açık** olsun
  (reçete aksini söylemedikçe).
- `Sort:` = `Manual`.

## Ayarlar penceresinin hatırlatması

```
Settings
├─ Browsers           : tarayıcı listesi (✓ = göster), Add... / Edit... / Remove, ▲ / ▼, Sort:, Refresh
├─ Default Browser    : Set as Default Browser, File types...
├─ Auto Select Filters: kurallar tablosu (Match | Pattern | Browser | Private | Arguments)
│                       Move Up / Move Down / Delete / Apply / Help
│                       "Link opened from: ..." (BrowserSelect bir linkle açıldıysa)
├─ Options            : Show running browsers only, Hold Alt on a link to skip rules, Export..., Import...
└─ Update checker     : enable, check now
```
