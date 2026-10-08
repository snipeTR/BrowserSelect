# BrowserSelect – İnsan Testi Reçeteleri (human_test)

Bu klasör, snipeTR fork’unda **yeni eklenen** özelliklerin elle (insan tarafından) test edilmesi için
adım adım reçeteler içerir. Her reçetede: amaç, ön koşullar, tıklama adımları, test linkleri,
beklenen sonuç (geçti/kaldı ölçütü) ve dikkat edilecek noktalar var.

> Reçeteler varsayılan İngilizce arayüze göre yazılmıştır; buton ve menü adları **uygulamada göründüğü gibi**
> (ör. `Settings`, `Apply`, `Move Down`) geçer. Türkçe arayüz için bkz. [12 – Türkçe dil](12-turkce-dil.md).

## Başlamadan önce

**Önce [00 – Hazırlık](00-hazirlik.md) dosyasını uygula.** Kurulum, ayarların yedeklenmesi ve
testlerde kullanılan `tools\bs-open.ps1` yardımcı betiği orada anlatılıyor.

## Reçeteler (önerilen sıra)

| # | Özellik | Reçete | Süre |
|---|---|---|---|
| 00 | Hazırlık: kurulum, yedek, test aracı | [00-hazirlik.md](00-hazirlik.md) | 10 dk |
| 01 | Maximize pencere durumu düzeltmesi | [01-maximize-duzeltmesi.md](01-maximize-duzeltmesi.md) | 5 dk |
| 02 | Gelişmiş otomatik seçim kuralları (Domain / URL / Path / Keyword / Extension / Source App / Regex, Private, Arguments, ignore URL) | [02-gelismis-kurallar.md](02-gelismis-kurallar.md) | 25 dk |
| 03 | Alt basılıyken kuralları atlama | [03-alt-ile-kurallari-atla.md](03-alt-ile-kurallari-atla.md) | 5 dk |
| 04 | Sadece çalışan tarayıcıları gösterme | [04-sadece-calisan-tarayicilar.md](04-sadece-calisan-tarayicilar.md) | 5 dk |
| 05 | Tarayıcı listesini sıralama (Manual / Alphabetical / Most used) | [05-tarayici-siralama.md](05-tarayici-siralama.md) | 10 dk |
| 06 | Özel kısayol tuşları | [06-ozel-kisayollar.md](06-ozel-kisayollar.md) | 5 dk |
| 07 | Ayar / kural Export – Import | [07-export-import.md](07-export-import.md) | 10 dk |
| 08 | Portable tarayıcı ekleme (Add... / Edit... / Remove) | [08-portable-tarayici.md](08-portable-tarayici.md) | 10 dk |
| 09 | Dosya ilişkilendirmeleri (.html / .url) | [09-dosya-iliskilendirme.md](09-dosya-iliskilendirme.md) | 10 dk |
| 10 | Özel tarayıcı ikonları | [10-ozel-ikon.md](10-ozel-ikon.md) | 5 dk |
| 11 | Kural silme butonu (`Delete`, Move Down’un sağında) | [11-kural-silme-butonu.md](11-kural-silme-butonu.md) | 5 dk |
| 12 | Türkçe dil (Settings → Language) | [12-turkce-dil.md](12-turkce-dil.md) | 15 dk |
| 13 | Hakkında: snipeTR, bağış adresi, Orijinal proje bilgileri butonu | [13-hakkinda-bagis.md](13-hakkinda-bagis.md) | 5 dk |
| 14 | Yardım pencereleri (EN/TR) ve kural dokümanı linki | [14-yardim-ve-kural-dokumani.md](14-yardim-ve-kural-dokumani.md) | 10 dk |

## Klasör içeriği

```
Tests/human_test/
├─ README.md                  ← bu dosya (indeks + sonuç tablosu)
├─ 00-hazirlik.md … 13-*.md   ← reçeteler
├─ tools/
│  └─ bs-open.ps1             ← bir linki BrowserSelect ile açan yardımcı betik
└─ files/
   ├─ test-links.txt          ← tüm test linkleri (tarayıcı dışı bir uygulamadan tıklamak için)
   ├─ test-page.html          ← .html ilişkilendirme testi
   ├─ test-shortcut.url       ← .url (Internet Shortcut) testi → https://example.com/?bs-test=url-file
   ├─ test-shortcut-2.url     ← ek bölümlü .url testi       → https://example.org/?bs-test=url-file-2
   ├─ sample-settings.json    ← Import testi için hazır ayar dosyası
   ├─ broken-settings.json    ← Import hata testi (geçersiz dosya)
   ├─ test-icon.png           ← özel ikon testi (PNG)
   └─ test-icon.ico           ← özel ikon testi (ICO)
```

> Not: Depoda zaten birim test projesi olan `Tests/` klasörü var. Windows büyük/küçük harf ayırmadığı için
> ayrı bir `tests/` klasörü açmak yerine reçeteler `Tests/human_test/` altına konuldu.

## Ortak test linkleri

Kurallar ve özellikler aşağıdaki linklerle test edilir (tam liste: [files/test-links.txt](files/test-links.txt)).
Hepsi herkese açık, zararsız sayfalardır (`example.com/org/net` IANA’nın test alan adlarıdır).

| Link | Kullanıldığı test |
|---|---|
| https://example.com/ | Domain, genel açma |
| https://www.example.com/ | Domain wildcard (`*.example.com`) |
| https://example.org/ | Domain / Regex / eksik tarayıcı |
| https://example.net/ | ignore URL |
| https://github.com/snipeTR/BrowserSelect | URL wildcard |
| https://github.com/microsoft/vscode | URL wildcard (eşleşmemeli) |
| https://example.com/docs/intro | Path |
| https://example.com/blog/post-1 | Path (eşleşmemeli) |
| https://www.google.com/search?q=zoom+meeting | Keyword |
| https://zoom.us/test | Keyword |
| https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf | Extension (`pdf`) |
| https://example.com/files/archive.zip | Extension (`zip`) |
| https://www.example.org/regex-test | Regex |

> `example.com/docs/intro` gibi alt sayfalar sunucuda yoktur; tarayıcıda “Example Domain” sayfası veya 404
> görebilirsin. Bu normaldir: testlerde önemli olan **hangi tarayıcının açıldığı** (veya seçim penceresinin
> açılıp açılmadığı), sayfanın içeriği değil.

**Önemli:** Bir tarayıcının *içinde* tıklanan link BrowserSelect’e gitmez, o tarayıcıda açılır. Linkleri
`tools\bs-open.ps1` ile ya da tarayıcı dışı bir uygulamadan (Outlook, Teams, Slack, Word, Windows 11 Not
Defteri…) aç.

## İpucu: Hangi linkin açıldığını görmek

BrowserSelect seçim penceresinin **başlık çubuğunda açılmakta olan link** yazar. Kural testlerinde tarayıcıyı
gerçekten açmana gerek yoksa, pencerenin açıldığını ve başlığı kontrol edip `Esc` ile kapatabilirsin.

## Sonuç tablosu (doldur)

Testleri bitirdikçe bu tabloyu kopyalayıp doldurabilirsin (issue açarken ekle):

| # | Özellik | Sonuç (✅ / ❌ / ⚠️) | Not |
|---|---|---|---|
| 01 | Maximize düzeltmesi | | |
| 02 | Gelişmiş kurallar | | |
| 03 | Alt ile kuralları atlama | | |
| 04 | Sadece çalışan tarayıcılar | | |
| 05 | Sıralama | | |
| 06 | Özel kısayollar | | |
| 07 | Export / Import | | |
| 08 | Portable tarayıcı | | |
| 09 | Dosya ilişkilendirme | | |
| 10 | Özel ikon | | |
| 11 | Kural silme butonu | | |
| 12 | Türkçe dil | | |
| 13 | Hakkında / bağış | | |

Test ortamı: Windows sürümü: ______  BrowserSelect sürümü: ______  Yüklü tarayıcılar: ______

Hata bulursan: https://github.com/snipeTR/BrowserSelect/issues adresinde reçete numarası, adım numarası,
beklenen ve gerçekleşen sonuçla birlikte bildir.
