# 02 – Gelişmiş otomatik seçim kuralları

## Amaç

`Settings` → `Auto Select Filters` tablosuna yeni sütunlar eklendi:

| Sütun | Ne işe yarar |
|---|---|
| `Match` | Pattern’in linkin hangi kısmıyla karşılaştırılacağı: `Domain` (varsayılan, eski davranış), `URL`, `Path`, `Keyword`, `Extension`, `Source App`, `Regex` |
| `Pattern` | Eşleşme kalıbı |
| `Browser` | Eşleşince ne olacak: bir tarayıcı, `display BrowserSelect` (seçim penceresini göster) veya **`ignore URL (do nothing)`** (linki hiç açma) |
| `Private` | Gizli / InPrivate pencerede aç |
| `Arguments` | Tarayıcıya ek komut satırı parametreleri (ör. `--new-window`, `--incognito`) |

Bu reçete her eşleşme tipini, Private / Arguments / ignore URL seçeneklerini ve kural önceliğini test eder.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam, `tools\bs-open.ps1` çalışıyor.
- İki tarayıcı. Aşağıda:
  - **Tarayıcı A** = kuralın göndereceği tarayıcı (ör. `Google Chrome` veya `Firefox`)
  - **Tarayıcı B** = diğeri (ör. `Microsoft Edge`)
- Başlangıçta kural listesi boş olsun.

## Kural nasıl eklenir (tüm alt testler için)

1. Başlat menüsünden **BrowserSelect**’i aç → sağdaki dikey **`Settings`** butonu.
2. `Auto Select Filters` tablosunun **en alttaki boş satırına** tıkla.
3. `Match` hücresinden tipi seç, `Pattern` hücresine kalıbı yaz, `Browser` hücresinden hedefi seç;
   gerekiyorsa `Private` kutusunu işaretle ve `Arguments` yaz.
4. **`Apply`**’a bas (Apply pasifleşmeli, `Cancel` butonu `Close`’a dönmeli).
5. `Close` ile ayarları, `Esc` ile BrowserSelect’i kapat.

## Nasıl doğrulanır

`tools` klasöründe `.\bs-open.ps1 <link>` çalıştır:

- **Kural eşleşti** → BrowserSelect penceresi **görünmez**, link doğrudan kuraldaki tarayıcıda açılır.
- **Kural eşleşmedi** → BrowserSelect seçim penceresi açılır (başlıkta link yazar) → `Esc` ile kapat.

Her alt testten sonra eklediğin kuralı seç → `Delete` → `Apply` ile sil (ya da sonraki teste devam et,
kurallar birbirini etkilemiyorsa).

---

## 2.1 – Domain (eski davranış korunuyor mu?)

**Kural:** `Match` = `Domain`, `Pattern` = `*.example.com`, `Browser` = Tarayıcı A

| Link | Beklenen |
|---|---|
| https://www.example.com/ | Tarayıcı A’da doğrudan açılır |
| https://example.com/ | Tarayıcı A’da doğrudan açılır (`*.alan.com` kök alanı da kapsar) |
| https://example.org/ | Seçim penceresi açılır |

- [ ] Sonuçlar tabloyla aynı.
- [ ] (İsteğe bağlı) Eski sürümden güncelleme yaptıysan, eski kuralların `Match` = `Domain` olarak aynen duruyor ve çalışıyor.

## 2.2 – URL (wildcard)

**Kural:** `Match` = `URL`, `Pattern` = `github.com/snipeTR/*`, `Browser` = Tarayıcı A
(`https://` yazmak isteğe bağlı; `*` ve `?` joker karakterleri desteklenir.)

| Link | Beklenen |
|---|---|
| https://github.com/snipeTR/BrowserSelect | Tarayıcı A |
| https://github.com/snipeTR/BrowserSelect/releases | Tarayıcı A |
| https://github.com/microsoft/vscode | Seçim penceresi |

- [ ] Sonuçlar tabloyla aynı.

## 2.3 – Path

**Kural:** `Match` = `Path`, `Pattern` = `/docs/*`, `Browser` = Tarayıcı A
(Alan adından bağımsızdır; sadece yol ve sorgu kısmına bakar.)

| Link | Beklenen |
|---|---|
| https://example.com/docs/intro | Tarayıcı A |
| https://example.org/docs/api/v2 | Tarayıcı A (farklı alan adı ama yol aynı) |
| https://example.com/blog/post-1 | Seçim penceresi |

- [ ] Sonuçlar tabloyla aynı.

## 2.4 – Keyword

**Kural:** `Match` = `Keyword`, `Pattern` = `zoom, meet`, `Browser` = Tarayıcı A
(Virgülle ayrılmış kelimeler; link bunlardan **herhangi birini** içeriyorsa eşleşir; büyük/küçük harf duyarsız.)

| Link | Beklenen |
|---|---|
| https://www.google.com/search?q=zoom+meeting | Tarayıcı A |
| https://zoom.us/test | Tarayıcı A |
| https://meet.google.com/ | Tarayıcı A |
| https://example.com/?q=ZOOM | Tarayıcı A (büyük harf) |
| https://www.bing.com/search?q=hava+durumu | Seçim penceresi |

- [ ] Sonuçlar tabloyla aynı.

## 2.5 – Extension

**Kural:** `Match` = `Extension`, `Pattern` = `pdf, zip`, `Browser` = Tarayıcı A
(Linkin **yolundaki** dosya uzantısına bakar; sorgu kısmındaki dosya adına bakmaz.)

| Link | Beklenen |
|---|---|
| https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf | Tarayıcı A (gerçek bir PDF açılır) |
| https://example.com/files/archive.zip | Tarayıcı A (sayfa 404 olabilir, önemli olan tarayıcı seçimi) |
| https://example.com/files/page.html | Seçim penceresi |
| https://example.com/view?file=report.pdf | Seçim penceresi (uzantı sorguda, yolda değil) |

- [ ] Sonuçlar tabloyla aynı.

## 2.6 – Source App (linki açan uygulama)

1. Önce kaynağı öğren: `.\bs-open.ps1 https://example.com/?bs-test=source` → seçim penceresi açılır →
   sağdaki `Settings` → `Auto Select Filters` bölümünde **`Link opened from: powershell.exe`**
   (PowerShell 7’de `pwsh.exe`) yazısını gör. Fareyle üzerine gelince tam yol görünür.
2. Aynı pencerede kural ekle: `Match` = `Source App`, `Pattern` = `powershell.exe, pwsh.exe`,
   `Browser` = Tarayıcı A → `Apply` → `Close` → `Esc`.

| Nasıl açılıyor | Beklenen |
|---|---|
| PowerShell: `.\bs-open.ps1 https://example.org/?bs-test=source` | Tarayıcı A (kaynak powershell.exe) |
| `Win + R` → `"%LOCALAPPDATA%\BrowserSelect\BrowserSelect.exe" https://example.org/?bs-test=source` | Seçim penceresi (kaynak `explorer.exe`) |

3. `Pattern`’ı `powershell` (uzantısız) veya `power*` (joker) yapıp tekrar dene → yine eşleşmeli.

- [ ] “Link opened from:” etiketi doğru uygulamayı gösteriyor.
- [ ] PowerShell’den açılan link Tarayıcı A’ya gidiyor, Win+R ile açılan link seçim penceresini gösteriyor.

**Gerçek hayat (isteğe bağlı, BrowserSelect varsayılan tarayıcı olmalı):** Outlook’tan bir linke tıkla →
seçim penceresinde `Settings` → `Link opened from: OUTLOOK.EXE` → kural `Source App` = `outlook.exe` →
Tarayıcı A. Sonraki Outlook linkleri doğrudan Tarayıcı A’da açılmalı. Aynısını Teams (`ms-teams.exe`),
Slack (`slack.exe`), Discord (`discord.exe`) için deneyebilirsin.

> **Sınırlama:** Kaynak, BrowserSelect’i başlatan üst süreçtir. Bazı uygulamalar linki bir aracı süreç
> (ör. `RuntimeBroker`, `sihost`, uygulama güncelleyici) üzerinden açar; bu durumda etiket farklı bir ad
> gösterir veya hiç görünmez. Kuralı her zaman **“Link opened from:”** etiketinde yazan ada göre yaz.
> `cmd.exe`, `rundll32.exe`, `OpenWith.exe`, `conhost.exe` aracı sayılır ve atlanır.

## 2.7 – Regex

**Kural:** `Match` = `Regex`, `Pattern` = `^https://(www\.)?example\.org/`, `Browser` = Tarayıcı A
(.NET düzenli ifadesi, büyük/küçük harf duyarsız, tüm linke uygulanır.)

| Link | Beklenen |
|---|---|
| https://example.org/regex-test | Tarayıcı A |
| https://www.example.org/regex-test | Tarayıcı A |
| https://example.com/regex-test | Seçim penceresi |

**Geçersiz regex kontrolü:** Yeni bir satıra `Match` = `Regex`, `Pattern` = `([`, `Browser` = Tarayıcı A yaz →
`Apply`.

- [ ] Sonuçlar tabloyla aynı.
- [ ] Geçersiz regex’te `Invalid Rule: '([' is not a valid regular expression.` uyarısı çıkıyor, uygulama
      çökmüyor; diğer kurallar kaydediliyor. (Bozuk satırı `Delete` ile sil ve tekrar `Apply`.)

## 2.8 – Private

**Kural:** `Match` = `Domain`, `Pattern` = `example.com`, `Browser` = Tarayıcı A, **`Private` ✓**

| Link | Beklenen |
|---|---|
| https://example.com/?bs-test=private | Tarayıcı A’nın **gizli** penceresinde (Chrome: Incognito, Edge: InPrivate, Firefox: Private) açılır |

- [ ] Gizli pencerede açıldı.

## 2.9 – Arguments

**Kural:** `Match` = `Domain`, `Pattern` = `example.com`, `Browser` = Chrome veya Edge,
`Private` boş, `Arguments` = `--new-window`

1. Önce hedef tarayıcıyı normal şekilde aç ve açık bırak.
2. `.\bs-open.ps1 "https://example.com/?bs-test=args"`

- [ ] Link mevcut pencerede yeni sekme yerine **yeni bir pencerede** açıldı.

Ek denemeler (isteğe bağlı):

| Tarayıcı | Arguments | Beklenen |
|---|---|---|
| Chrome | `--incognito` | Gizli pencere |
| Edge | `--inprivate` | InPrivate pencere |
| Firefox | `-private-window` | Gizli pencere |
| Firefox | `-new-window` | Yeni pencere |
| Chrome | `--user-data-dir="C:\bs-test\chrome-temp"` | Ayrı, boş bir Chrome profili (tırnaklı değer, boşluklu yol desteği) |

## 2.10 – ignore URL (do nothing)

**Kural:** `Match` = `Domain`, `Pattern` = `example.net`, `Browser` = **`ignore URL (do nothing)`**

| Link | Beklenen |
|---|---|
| https://example.net/ | **Hiçbir şey olmaz**: ne seçim penceresi ne tarayıcı açılır |
| https://example.com/ | Normal davranış (kural yoksa seçim penceresi) |

- [ ] example.net hiç açılmadı, hata mesajı da çıkmadı.

## 2.11 – Öncelik ve `display BrowserSelect` istisnası

**Kurallar (bu sırayla):**

1. `Domain` `*` → Tarayıcı A
2. `Domain` `*.example.org` → `display BrowserSelect`
3. `Domain` `example.com` → Tarayıcı B

| Link | Beklenen | Neden |
|---|---|---|
| https://example.com/ | Tarayıcı B | Tam eşleşme (wildcard’sız) önce kontrol edilir |
| https://www.example.org/ | Seçim penceresi | Wildcard kural, `*`’dan önce gelir |
| https://github.com/ | Tarayıcı A | Hiçbiri tutmazsa her şeyi yakalayan `*` en son |

- [ ] Sonuçlar tabloyla aynı. (Kural sırasını `Move Up` / `Move Down` ile değiştirmek bu öncelikleri bozmamalı.)

## 2.12 – Kaldırılmış tarayıcıya giden kural

Kuraldaki tarayıcı artık yoksa (kaldırıldı, başka bilgisayardan import edildi) uygulama çökmemeli, seçim
penceresi açılmalı. Bu, [07 – Export/Import](07-export-import.md) (Test 7C) ve
[08 – Portable tarayıcı](08-portable-tarayici.md) (Test 8D) reçetelerinde test ediliyor.

## Temizlik

Tüm test kurallarını seç → `Delete` → `Apply`.

## Notlar

- Kural yardımı: ayarlar penceresindeki `Help` butonu veya
  https://github.com/snipeTR/BrowserSelect/blob/master/help/filters.md
- Kural eşleşmesine rağmen pencere açılıyorsa: `Alt` tuşu basılı kalmış olabilir (bkz. [03](03-alt-ile-kurallari-atla.md)).
