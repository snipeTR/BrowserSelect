# 09 – Dosya ilişkilendirmeleri (.html / .url)

## Amaç

BrowserSelect artık **.htm, .html, .shtml, .xht, .xhtml** dosyalarını ve **.url** (Internet Shortcut) dosyalarını
açabilir:

- Installer ve `Settings` → `Default Browser` → **`File types...`** butonu BrowserSelect’i bu dosya türleri için
  kaydeder; dosyaların **“Birlikte aç” (Open with)** menüsünde görünür. Mevcut varsayılan uygulaman
  **değiştirilmez**, seçim sana kalır.
- `.html` dosyası açılınca seçim penceresi `file:///...` adresini gösterir.
- `.url` dosyası açılınca içindeki **hedef link** açılır (dosyanın kendisi değil).
- Yerel bir dosyada **`Always`** butonu alan adı kuralı yerine **Extension** kuralı (ör. `html`) oluşturur.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) adım 1–2 tamam; test dosyaları bilgisayarda.
- Test dosyaları (`Tests\human_test\files\`):
  - `test-page.html` → açılınca yeşil **“BrowserSelect .html testi BASARILI”** yazısı
  - `test-shortcut.url` → hedef: `https://example.com/?bs-test=url-file`
  - `test-shortcut-2.url` → ek bölümler içeren gerçekçi kısayol, hedef: `https://example.org/?bs-test=url-file-2`
- Kural listesinde `html` veya `example.com/org` ile eşleşen kural olmasın.

> Dosyaları elle oluşturmak istersen: Not Defteri’ne aşağıdakini yaz, **“Tüm dosyalar”** türünde
> `test-shortcut.url` adıyla kaydet:
>
> ```ini
> [InternetShortcut]
> URL=https://example.com/?bs-test=url-file
> ```

## Adımlar

### Test 9A – File types... butonu

1. BrowserSelect’i Başlat menüsünden aç → `Settings` → `Default Browser` → **`File types...`**.

- [ ] Bilgi mesajı çıktı: `BrowserSelect is now registered for .htm, .html, .shtml, .xht, .xhtml, .url files and
      appears in their "Open with" menu...`
- [ ] `OK`’dan sonra Windows **Varsayılan uygulamalar** ekranı (BrowserSelect sayfası veya genel liste) açıldı.
- [ ] Hata mesajı çıkmadı.

### Test 9B – .html dosyasını “Birlikte aç” ile açma

1. Dosya Gezgini’nde `files\test-page.html` → sağ tık → **`Birlikte aç`** → listede **BrowserSelect**’i seç
   (yoksa `Başka bir uygulama seç` → `BrowserSelect`). “Her zaman” seçeneğini **işaretleme**, `Bir kez`.
2. Seçim penceresinin başlığına bak.
3. Tarayıcı A’ya tıkla.

- [ ] BrowserSelect “Birlikte aç” listesinde çıktı.
- [ ] Başlıkta `file:///C:/.../test-page.html` yazıyor.
- [ ] Sayfa Tarayıcı A’da açıldı, yeşil **BASARILI** yazısı ve adres çubuğunda `file:///...test-page.html` görünüyor.

### Test 9C – Yerel dosyada “Always” → Extension kuralı

1. `test-page.html`’i 9B’deki gibi BrowserSelect ile aç.
2. Tarayıcı A’nın altındaki **`Always`** butonuna tıkla.
3. `test-page.html`’i tekrar BrowserSelect ile aç.
4. BrowserSelect’i Başlat menüsünden aç → `Settings` → kural tablosuna bak.

- [ ] 2. adımda sayfa Tarayıcı A’da açıldı.
- [ ] 3. adımda seçim penceresi **açılmadan** doğrudan Tarayıcı A’da açıldı.
- [ ] Tabloda yeni kural: `Match` = `Extension`, `Pattern` = `html`, `Browser` = Tarayıcı A
      (alan adı kuralı değil, `file` için anlamsız bir pattern değil).

(Sonra bu kuralı seç → `Delete` → `Apply`.)

### Test 9D – .url dosyası

1. `files\test-shortcut.url` → sağ tık → `Birlikte aç` → **BrowserSelect** (listede yoksa
   `Başka bir uygulama seç` → `Bu bilgisayarda bir uygulama ara` → `%LOCALAPPDATA%\BrowserSelect\BrowserSelect.exe`).
2. Başlığa bak, Tarayıcı A’ya tıkla.
3. Aynısını `files\test-shortcut-2.url` için yap.

- [ ] Başlıkta dosya yolu değil **hedef link** yazıyor: `https://example.com/?bs-test=url-file`.
- [ ] Tarayıcıda example.com açıldı (adres çubuğunda `?bs-test=url-file`).
- [ ] `test-shortcut-2.url` → `https://example.org/?bs-test=url-file-2` (ek bölümler sorun çıkarmadı).
- [ ] Hedefe uyan kurallar `.url` dosyasında da çalışıyor: `Domain` `example.com` → Tarayıcı B kuralı ekleyip
      `test-shortcut.url`’ü tekrar aç → doğrudan Tarayıcı B (sonra kuralı sil).

### Test 9E – Varsayılan .html uygulaması yapma (isteğe bağlı)

1. Windows Ayarlar → `Uygulamalar` → `Varsayılan uygulamalar` → **BrowserSelect** → `.html` → BrowserSelect seç.
2. `test-page.html`’e **çift tıkla**.

- [ ] Çift tıklama BrowserSelect seçim penceresini açtı.
- [ ] Test bitince `.html` için eski tarayıcını tekrar varsayılan yap.

### Test 9F – Kaldırma sonrası temizlik (isteğe bağlı, en son yap)

1. BrowserSelect’i `Ayarlar` → `Uygulamalar` üzerinden kaldır.
2. `test-page.html` → sağ tık → `Birlikte aç`.

- [ ] BrowserSelect artık listede yok (kaldırıcı dosya ilişkilendirmelerini temizledi).

> 9D’de BrowserSelect’i `Bu bilgisayarda bir uygulama ara` ile seçtiysen, Windows kendi “son kullanılanlar”
> kaydını tutabilir; listede kalan bu girdi kaldırıcıdan kaynaklanmaz.

## Notlar

- Windows, varsayılan uygulamayı sessizce değiştirmeye izin vermez; bu yüzden BrowserSelect sadece kendini
  **kaydeder**, varsayılan yapmak kullanıcının seçimidir (Test 9E).
- Bir `.url` dosyasına çift tıkladığında Windows normalde onu varsayılan **http/https** tarayıcısına verir.
  BrowserSelect varsayılan tarayıcıysa bu da seçim penceresini açar; bu reçetede özellikle **.url ilişkilendirmesini**
  test etmek için “Birlikte aç” kullanılıyor.
- `.url` okunamazsa (bozuk dosya) BrowserSelect dosyanın kendisini açmaya çalışır; çökmemelidir.
