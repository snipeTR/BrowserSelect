# 07 – Ayar / kural Export – Import

## Amaç

`Settings` → `Options` → **`Export...`** kuralları ve ayarları okunabilir bir **JSON** dosyasına yazar;
**`Import...`** bu dosyayı geri yükler (başka bir bilgisayara taşımak veya yedek için).

Dışa aktarılanlar: kurallar (Match / Pattern / Browser / Private / Arguments), gizlenen tarayıcılar,
portable (elle eklenen) tarayıcılar, ikon / kısayol / argüman özelleştirmeleri, tarayıcı sırası, sıralama modu,
`Show running browsers only`, `Hold Alt on a link to skip rules`, güncelleme kontrolü.

Import, dosyada **bulunan** bölümleri değiştirir; dosyada olmayan bölümlere dokunmaz.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam (özellikle adım 5: kendi ayarlarının yedeği).
- Test dosyaları: `files\sample-settings.json`, `files\broken-settings.json`.

## Test linkleri

- https://example.net/
- https://example.com/?q=bs-import-test
- https://example.org/?bs-test=import

## Adımlar

### Test 7A – Export

1. `Settings` → `Auto Select Filters`’a iki kural ekle → `Apply`:
   - `Keyword` | `export-test` | Tarayıcı A | Private ✓ | Arguments boş
   - `URL` | `github.com/snipeTR/*` | Tarayıcı B | Private boş | Arguments `--new-window`
2. `Browsers` → `Sort:` = `Alphabetical`.
3. `Options` → `Export...` → dosya adı varsayılan `BrowserSelect-settings.json`, Masaüstü’ne kaydet.
4. Dosyayı Not Defteri ile aç.

- [ ] `Rules and settings exported to ...` bilgisi çıktı.
- [ ] Dosyada `"format": "BrowserSelect-settings"` ve `"rules"` altında iki kural var; `"match"`, `"pattern"`,
      `"browser"`, `"private"`, `"arguments"` alanları girdiğin değerlerle aynı (`"private": true`,
      `"arguments": "--new-window"`).
- [ ] `"sortMode": "Alphabetical"` ve `"altIgnoresRules"`, `"showRunningOnly"` alanları var.

### Test 7B – Geri yükleme (round-trip)

1. İki test kuralını seç → `Delete` → `Apply`. `Sort:` = `Manual` yap.
2. `Import...` → Masaüstündeki `BrowserSelect-settings.json` → onay sorusunda (`Importing replaces your current
   rules...`) **`Yes`**.

- [ ] `Settings imported (2 rules).` mesajı çıktı.
- [ ] Ayarlar penceresi yenilendi: iki kural tüm sütunlarıyla geri geldi, `Sort:` = `Alphabetical` oldu.
- [ ] `Apply` pasif (kaydedilmemiş değişiklik yok).

### Test 7C – Hazır dosyayı import et + eksik tarayıcı davranışı

`files\sample-settings.json` içinde 3 kural var:

| Match | Pattern | Browser |
|---|---|---|
| Domain | `example.net` | `ignore URL (do nothing)` |
| Keyword | `bs-import-test` | `display BrowserSelect` |
| Domain | `example.org` | `Olmayan Tarayici (test)` ← bu bilgisayarda olmayan bir tarayıcı |

1. `Import...` → `files\sample-settings.json` → `Yes`.
2. Kural tablosuna bak, `Close`, BrowserSelect’i kapat.
3. Linkleri dene:

| Komut | Beklenen |
|---|---|
| `.\bs-open.ps1 https://example.net/` | Hiçbir şey açılmaz (ignore URL) |
| `.\bs-open.ps1 "https://example.com/?q=bs-import-test"` | Seçim penceresi |
| `.\bs-open.ps1 "https://example.org/?bs-test=import"` | **Seçim penceresi** (tarayıcı bulunamadı → çökme yok) |

- [ ] `Settings imported (3 rules).` mesajı çıktı ve tabloda 3 kural var;
      üçüncü kuralın `Browser` hücresinde `Olmayan Tarayici (test)` görünüyor (hata vermeden).
- [ ] `Sort:` = `Alphabetical`; dosyada olmayan bölümler (gizli tarayıcılar, portable tarayıcılar, ikonlar) **değişmedi**.
- [ ] Link sonuçları tablodaki gibi; uygulama hiç çökmedi / hata penceresi göstermedi.

### Test 7D – Geçersiz dosya

1. `Import...` → `files\broken-settings.json` → `Yes`.
2. Ayrıca herhangi bir `.txt` dosyasını (ör. `files\test-links.txt`, filtreyi `All files (*.*)` yap) import etmeyi dene.

- [ ] `Import failed: The file is not a valid BrowserSelect settings file.` hatası çıktı.
- [ ] Mevcut kurallar ve ayarlar **değişmedi**.

### Test 7E – Kaydedilmemiş değişiklikle Export

1. Kural tablosuna yeni bir satır ekle ama **`Apply`’a basma**.
2. `Export...`’a bas.

- [ ] `You have unsaved rule changes. Apply them before exporting?` sorusu çıktı.
  - `Yes` → önce kaydedip yeni kuralla birlikte dışa aktarıyor.
  - `No` → son kaydedilmiş kuralları dışa aktarıyor.
  - `Cancel` → hiçbir şey yapmıyor.

### Test 7F – Import’tan vazgeçme

1. `Import...` → bir dosya seç → onay sorusunda **`No`**.

- [ ] Hiçbir şey değişmedi.

## Temizlik

`Import...` → [00 – Hazırlık](00-hazirlik.md) adım 5’te aldığın **yedek** dosyası → `Yes`.
(Yedek almadıysan test kurallarını `Delete` + `Apply` ile sil, `Sort:` = `Manual` yap.)

## Notlar

- JSON dosyasında tarayıcılar **adlarıyla** saklanır. Başka bilgisayarda aynı adlı tarayıcı yoksa o kurallar
  seçim penceresine düşer (Test 7C); `Settings`’te `Browser` hücresinden doğru tarayıcıyı seçip `Apply` ile düzeltilebilir.
- Portable tarayıcıların `.exe` yolu da dosyaya yazılır; hedef bilgisayarda aynı yolda olmalıdır.
