# 01 – Maximize pencere durumu düzeltmesi

## Amaç

Eskiden, link **ekranı kaplayan (maximize) bir uygulamadan** ya da “Maximized” çalışacak şekilde ayarlı bir
kısayoldan açıldığında BrowserSelect penceresi de **tam ekran** açılıyordu. Windows, başlatan uygulamanın
“pencere durumu” bilgisini (STARTUPINFO) yeni sürecin ilk penceresine uygular.

Düzeltmeden sonra BrowserSelect her zaman **normal boyutta, ortalanmış** küçük pencere olarak açılmalı ve
pencerede maximize / minimize butonları **olmamalı**.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) adım 1–3 tamam.
- Test için kural tanımlı olmasın (yoksa link seçim penceresi açılmadan tarayıcıya gider).

## Test linki

- https://example.com/?bs-test=maximize

## Adımlar

### Test 1A – PowerShell ile “Maximized” başlatma

1. PowerShell’de `tools` klasöründe:

   ```powershell
   .\bs-open.ps1 "https://example.com/?bs-test=maximize" -Maximized
   ```

2. Açılan BrowserSelect penceresine bak.
3. `Esc` ile kapat.

### Test 1B – cmd `start /max`

1. `Win + R` → `cmd` → Enter.
2. Şunu yaz:

   ```bat
   start "" /max "%LOCALAPPDATA%\BrowserSelect\BrowserSelect.exe" "https://example.com/?bs-test=maximize"
   ```

3. Pencereyi kontrol et, `Esc` ile kapat.

### Test 1C – “Maximized” kısayol (gerçek hayat senaryosu)

1. Masaüstüne sağ tık → `Yeni` → `Kısayol`.
2. Konum olarak şunu yapıştır:
   `"%LOCALAPPDATA%\BrowserSelect\BrowserSelect.exe" "https://example.com/?bs-test=maximize"`
   → `İleri` → ad: `BS maximize test` → `Son`.
3. Kısayola sağ tık → `Özellikler` → `Çalıştır:` = **`Ekranı Kapla` (Maximized)** → `Tamam`.
4. Kısayola çift tıkla.

### Test 1D – Maximize bir uygulamadan link tıklama (BrowserSelect varsayılan tarayıcıyken)

1. BrowserSelect varsayılan tarayıcı olsun ([00](00-hazirlik.md) adım 4).
2. Outlook / Word / Teams gibi bir uygulamayı **tam ekran (maximize)** yap.
3. İçinde `https://example.com/?bs-test=maximize` linkine tıkla
   (Word’de: linki yaz, boşluk bırak → otomatik link olur → `Ctrl + tıkla`).

## Beklenen sonuç (geçti ölçütü)

- [ ] 1A, 1B, 1C ve 1D’nin hepsinde BrowserSelect **küçük, normal boyutlu** pencere olarak açılıyor
      (sadece tarayıcı ikonları kadar geniş), ekranı kaplamıyor.
- [ ] Pencere, fare imlecinin bulunduğu monitörün ortasında.
- [ ] Başlık çubuğunda **maximize (□) ve minimize (–) butonları yok**, sadece kapatma (X) var.
- [ ] Başlık çubuğuna **çift tıklamak** pencereyi büyütmüyor.
- [ ] `Win + ↑` kısayolu pencereyi tam ekran yapmıyor (yaparsa anında normale dönmeli).
- [ ] Bir tarayıcıya tıklayınca link normal şekilde açılıyor.

**Kaldı** sayılır: Pencere tam ekran açılırsa, ikonlar sol üst köşede kalıp pencere boş büyük bir alan
gösterirse.

## Notlar

- Tarayıcının kendisinin maximize açılması bu testin konusu değil; tarayıcı kendi son pencere durumunu kullanır.
- Test 1C’den sonra masaüstündeki `BS maximize test` kısayolunu silebilirsin.
