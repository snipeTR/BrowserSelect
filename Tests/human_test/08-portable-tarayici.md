# 08 – Portable tarayıcı ekleme (Add... / Edit... / Remove)

## Amaç

Kayıt defterinde görünmeyen tarayıcılar (portable sürümler, özel derlemeler) artık
`Settings` → `Browsers` → **`Add...`** ile `.exe` dosyası seçilerek eklenebilir. Eklenen tarayıcılar
`Edit...` ile düzenlenir, **`Remove`** ile listeden çıkarılır ve `Refresh` sonrasında kaybolmaz.

`Add...` penceresi (`Browser`) alanları: `Name:`, `Executable:` (`Browse...`), `Arguments:`, `Shortcut keys:`,
`Icon:` (`Change...` / `Default`), `OK` / `Cancel`.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam.
- Bir portable tarayıcı (birini seç):
  - **Firefox Portable** (önerilen): https://portableapps.com/apps/internet/firefox_portable
    → kur/çıkar, örn. `C:\bs-test\FirefoxPortable\FirefoxPortable.exe`
  - **ungoogled-chromium** (Windows x64 ZIP): https://ungoogled-software.github.io/ungoogled-chromium-binaries/
    → ZIP’i çıkar, örn. `C:\bs-test\ungoogled-chromium\chrome.exe`
  - İndirmek istemiyorsan Test 8F’deki “mevcut tarayıcıyı farklı parametreyle ekleme” yöntemini kullan.

## Test linki

- https://example.com/?bs-test=portable

## Adımlar

### Test 8A – Portable tarayıcı ekle

1. BrowserSelect’i Başlat menüsünden aç → `Settings` → `Browsers` → **`Add...`**.
2. `Name:` = `Firefox Portable`.
3. `Executable:` yanındaki **`Browse...`** → `FirefoxPortable.exe` dosyasını seç.
4. `Arguments:` boş, `Shortcut keys:` = `p` → `OK`.
5. `Close` → ana pencereye bak.
6. `Esc`, sonra `.\bs-open.ps1 "https://example.com/?bs-test=portable"` → `Firefox Portable`’a tıkla (veya `p`’ye bas).

- [ ] `Browsers` listesinde `Firefox Portable` işaretli (görünür) olarak eklendi ve seçili geldi.
- [ ] Ana pencerede kendi `.exe` ikonuyla ve `( <numara>,p )` kısayoluyla görünüyor.
- [ ] Link portable tarayıcıda açıldı.
- [ ] (Ek) `Name:` boşken `Browse...` ile exe seçilirse ad, programın ürün adından otomatik doldu (ör. `Firefox Portable`);
      ikon önizlemesi exe’nin ikonunu gösterdi.

### Test 8B – Doğrulama mesajları

`Add...` penceresinde sırayla dene (her birinde `OK`’a bas):

| Giriş | Beklenen uyarı |
|---|---|
| `Name:` boş | `Please enter a name for the browser.` |
| `Name:` = `ignore URL (do nothing)` | `This name is reserved, please choose another one.` |
| `Name:` = mevcut bir tarayıcının adı (ör. `Firefox Portable`) | `A browser named 'Firefox Portable' already exists, please choose another name.` |
| `Executable:` boş veya var olmayan yol | `Please select an existing executable (use Browse...).` |
| Farklı ad ama **aynı exe ve aynı Arguments** | `This browser (same executable and arguments) is already in the list.` |

- [ ] Her durumda uyarı çıktı ve pencere kapanmadı; `Cancel` hiçbir şey eklemedi.

### Test 8C – Kuralda kullanma ve yeniden adlandırma

1. `Auto Select Filters` → `Domain` | `example.com` | `Firefox Portable` → `Apply`.
2. `.\bs-open.ps1 "https://example.com/?bs-test=portable"` → doğrudan portable tarayıcıda açılmalı.
3. `Settings` → `Browsers` → `Firefox Portable`’ı seç → `Edit...` → `Name:` = `FF Portable` → `OK`.
4. Kural tablosuna bak; linki tekrar aç.

- [ ] Kural doğrudan portable tarayıcıyı açtı.
- [ ] Yeniden adlandırınca kuraldaki `Browser` değeri otomatik olarak `FF Portable` oldu ve kural çalışmaya devam etti.

### Test 8D – Remove ve eksik tarayıcıya giden kural

1. `Browsers` → kayıt defterinden bulunan bir tarayıcıyı (ör. Edge) seç → `Remove` butonunun **pasif** olduğunu gör.
2. `FF Portable`’ı seç → **`Remove`** → `Remove 'FF Portable' from the list?` → `Yes`.
3. `Close`, sonra `.\bs-open.ps1 "https://example.com/?bs-test=portable"`.

- [ ] `Remove` sadece elle eklenen tarayıcılarda aktif.
- [ ] Tarayıcı listeden ve ana pencereden kalktı.
- [ ] Kural hâlâ duruyor ama tarayıcı olmadığı için **seçim penceresi açıldı** (çökme / hata yok).

(Sonra kuralı `Delete` + `Apply` ile sil.)

### Test 8E – Refresh sonrası korunma

1. Test 8A’daki gibi portable tarayıcıyı tekrar ekle.
2. `Browsers` → **`Refresh`**.

- [ ] Portable tarayıcı listede kaldı, kısayolu ve ikonu korundu.

### Test 8F – İndirmeden test: mevcut tarayıcıyı parametreyle ekleme

1. `Add...` → `Name:` = `Chrome Gizli` (Edge için `Edge InPrivate`).
2. `Browse...` → `C:\Program Files\Google\Chrome\Application\chrome.exe`
   (bazı kurulumlarda `%LOCALAPPDATA%\Google\Chrome\Application\chrome.exe`)
   (Edge: `C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe`).
3. `Arguments:` = `--incognito` (Edge: `--inprivate`) → `OK` → `Close`.
4. Linki aç → `Chrome Gizli`’ye tıkla.

- [ ] Listede normal Chrome’un yanında ayrı bir `Chrome Gizli` girdisi var.
- [ ] Link her zaman gizli pencerede açılıyor.

## Temizlik

Eklediğin tarayıcıları `Remove` ile, test kurallarını `Delete` + `Apply` ile sil.

## Notlar

- **Firefox Portable + “Show running browsers only”:** `FirefoxPortable.exe` sadece bir başlatıcıdır; asıl
  çalışan süreç `App\Firefox64\firefox.exe` olduğu için [04](04-sadece-calisan-tarayicilar.md) özelliği portable
  Firefox’u “çalışıyor” göremeyebilir. Bu bilinen bir sınırlamadır. ungoogled-chromium’un `chrome.exe`’si
  doğrudan çalıştığı için bu sorun onda yoktur.
- Herhangi bir program eklenebilir; program linki komut satırı argümanı olarak almalıdır.
