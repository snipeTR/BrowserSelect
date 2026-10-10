# 21 – Tüm pencerelerde Windows 11 tasarımı (seçim penceresi, Hakkında, Tarayıcı ekle/düzenle, yardım, güncelleme)

> Bu reçete v1.5.8.0 ve sonrası içindir.

## Amaç

[20](20-ayarlar-yeni-tasarim.md)’deki Windows 11 (Fluent) görünümü artık **bütün pencerelerde** var. **Hiçbir
özellik değişmedi, sadece yerleşim ve görünüm değişti.** Pencere köşeleri Windows 11’in standart yuvarlak köşeleri
(~8 piksel); Windows 10’da köşeler düz kalır.

- **Ana pencere (tarayıcı seçimi):** her tarayıcı yuvarlak köşeli bir **kart** (simge, ad, kısayol ipucu, altta
  `Always` butonu). Fareyle üzerine gelince kart hafifçe renklenir ve kenarı vurgu rengine döner, basılıyken biraz
  daha koyulaşır.
- Sağdaki dikey yazılı `About` / `Settings` butonları yerine **küçük yuvarlak simge butonları** var:
  **(i)** Hakkında, **dişli** Ayarlar, **?** yardım. Üzerine gelince ipucu (tooltip) çıkar. Güncelleme varsa
  **?** yerine vurgu renginde bir **indirme oku** görünür. (Simge yazı tipi yoksa eski dikey yazılı butonlar
  yuvarlak olarak çizilir.)
- **Hakkında** ve **Orijinal proje bilgileri:** büyük başlık, bilgiler / iletişim / bağış **kartlarda**; `Copy`
  ve `Original project...` yuvarlak buton.
- **Tarayıcı ekle/düzenle:** alanlar bir kartta, metin kutuları yuvarlak (odaklanınca altta vurgu çizgisi), simge
  önizlemesi yuvarlak bir kutuda, `OK` vurgu renginde.
- **Yardım pencereleri:** metin yuvarlak bir kartın içinde, `Close` yuvarlak buton.
- **Güncelleme indirme penceresi:** ince, yuvarlak uçlu ilerleme çubuğu, yuvarlak `Cancel`.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam; `BrowserSelect-1.5.8.0-x64-Setup.exe` (veya daha yeni) kurulu.
- En az üç tarayıcı. Mümkünse Windows 11 ve %100 dışında bir ekran ölçeği denenebilecek bir bilgisayar.

## Test linkleri

- https://example.com/?bs-test=ui-v158
- https://example.org/?bs-test=ui-v158

## Adımlar

### Test 21A – Tarayıcı kartları

1. `.\bs-open.ps1 "https://example.com/?bs-test=ui-v158"` (veya `files\test-links.txt`’ten bir linke tıkla).

- [ ] Her tarayıcı ayrı, yuvarlak köşeli bir kart; simge, ad ve altında kısayol ipucu (ör. `( 1, c )`) tam
      görünüyor, kesik değil.
- [ ] Fareyle bir kartın üzerine gel: kart hafifçe renkleniyor ve kenarı vurgu renginde; ayrılınca normale dönüyor.
      Basılı tutunca biraz daha koyu.
- [ ] Karta tıklamak linki o tarayıcıda açıyor; `Shift` + tıklama gizli pencerede açıyor.
- [ ] Sağ tık → `Open in private window` eskisi gibi çalışıyor.
- [ ] Kartın altındaki `Always` butonu yuvarlak; tıklayınca eskisi gibi bu site için kural oluşturuyor
      (sonra `Settings → Auto Select Filters`’ta kuralı sil).
- [ ] Klavye: `1`–`9`, tarayıcı adının ilk harfi, özel kısayollar, `Shift` ile gizli pencere ve `Esc` (kapat)
      eskisi gibi çalışıyor.
- [ ] Pencere eskisi kadar hızlı açılıyor (link tıklamasından sonra belirgin bir gecikme yok).

### Test 21B – Simge butonları ve güncelleme oku

- [ ] Sağda alt alta üç küçük yuvarlak buton var: **(i)**, **dişli**, **?**. Üzerlerine gelince ipucu çıkıyor
      (`About`, `Settings`, yardım).
- [ ] **(i)** Hakkında’yı, **dişli** Ayarlar’ı, **?** yardım penceresini açıyor.
- [ ] Butonlar kartlarla aynı hizada, birbirine ya da kartlara binmiyor; tek tarayıcı varken de düzgün.
- [ ] (Eski bir sürüm kuruluyken veya [15](15-guncelleme-indir-kur.md)’teki gibi güncelleme varken) **?** yerine
      vurgu renginde **indirme oku** görünüyor, ipucu “güncelleme var” diyor; tıklayınca güncelleme sorusu geliyor.

### Test 21C – Hakkında ve Orijinal proje bilgileri

1. Ana pencerede **(i)**.

- [ ] Üstte simge, büyük başlık, sürüm; altında kartlar: bilgi, GitHub, bağış (QR kodu, adres, `Copy`).
- [ ] `Copy` adresi panoya kopyalıyor ve kısa süre `Copied` yazıyor; buton genişliği değişmiyor, yazı kesilmiyor.
- [ ] GitHub linki ve bağış linki tarayıcıda açılıyor.
- [ ] `Original project...` yuvarlak buton; tıklayınca açılan pencerede de başlık, bilgi kartı ve bağış kartı var,
      e-posta / GitHub linkleri ve `Copy` çalışıyor.
- [ ] `Close` ve `Esc` pencereyi kapatıyor.

### Test 21D – Tarayıcı ekle/düzenle

1. `Settings → Browsers → Add...`, sonra bir tarayıcıyı seçip `Edit...`.

- [ ] Etiketler solda, yuvarlak metin kutuları sağda hizalı; tıklanan kutunun altında vurgu renginde çizgi çıkıyor.
- [ ] `Browse...` ile bir .exe seçince yol ve ad doluyor; simge önizlemesi yuvarlak kutuda görünüyor.
- [ ] Simge `Change...` / `Default` (bkz. [10](10-ozel-ikon.md)) ve kısayol alanı ([06](06-ozel-kisayollar.md))
      eskisi gibi çalışıyor; kısayol ipucu yazısı kesik değil.
- [ ] `OK` vurgu renginde; `OK` kaydediyor, `Cancel` / `Esc` kaydetmeden kapatıyor.

### Test 21E – Yardım pencereleri

- [ ] Ana penceredeki **?** ve `Settings → Auto Select Filters → Help`: metin yuvarlak bir kartın içinde,
      kaydırma çubuğu kartın içinde; altta sağda yuvarlak `Close`.
- [ ] Pencereyi büyütüp küçültünce kart ve `Close` birlikte hareket ediyor, `Close` metnin üstüne binmiyor.
- [ ] Yardımda “THE BROWSER LIST” bölümünde sağdaki küçük butonları anlatan 4. madde var; “Display scale”
      satırı pencerenin önce daraldığını, sonra kaydırma çubuğu çıktığını söylüyor.

### Test 21F – Güncelleme indirme penceresi

1. [15](15-guncelleme-indir-kur.md)’teki gibi bir güncellemeyi indirmeye başla.

- [ ] İlerleme çubuğu ince ve yuvarlak uçlu, vurgu renginde doluyor; yüzde/boyut yazısı güncelleniyor.
- [ ] `Cancel` yuvarlak; indirmeyi iptal ediyor.

### Test 21G – Tema, Mica, ölçek ve diller

- [ ] `Settings → Theme → Dark`: yukarıdaki bütün pencereler koyu; kartlar zeminden ayırt ediliyor, yazılar
      okunuyor, kart hover rengi koyu temada da görünür. `Light`’ta da aynı.
- [ ] `Mica` açık/kapalı iken pencereler düzgün (başlık çubuğu dışında fark yok).
- [ ] Windows 11’de bütün pencerelerin köşeleri yuvarlak (~8 piksel).
- [ ] Ölçek `%100`, `%125`, `%150`, `%175`, `%200` (her birinde oturumu kapatıp aç): kartlar, simge butonları,
      köşeler ölçekle büyüyor; yazı kesilmiyor, kontroller üst üste binmiyor.
- [ ] `Deutsch`, `Русский`, `Polski`, `日本語`, `简体中文`: Hakkında, Tarayıcı ekle/düzenle ve yardım pencerelerinde
      yazı kesilmiyor; uzun yazılar alt satıra geçiyor, butonlar yazıya göre genişliyor.

## Geçti / kaldı

Bütün kutular işaretlendiyse **geçti**. Kesik yazı, üst üste binen kontrol, çalışmayan bir buton/kısayol veya
koyu temada okunmayan bir yazı varsa ekran görüntüsüyle birlikte (dil, tema, ölçek bilgisiyle) **kaldı** yaz.
