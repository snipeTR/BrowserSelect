# 16 – Windows 10/11 görünümü, Tema (Açık / Koyu / Windows'a uy) ve Mica

> Bu reçete **UI test kopyası** (`snipeTR/BrowserSelect-ui-test`, v1.5.1.0) içindir. Değişiklikler sadece
> **görünümle** ilgilidir: pencerelerdeki kontrollerin yerleri, özellikler ve davranış aynı kalmalıdır.

## Amaç

- Tüm pencerelerde **Segoe UI** yazı tipi, ince kenarlı **düz (flat) butonlar** ve fareyle üzerine gelince hafif renk.
- Windows 11 açık tema renkleri; kural tablosunda düz başlıklar, açık gri çizgiler, yumuşak seçim rengi.
- Ayarlar sol altta, **Language / Dil**'in altında **Theme / Tema** açılır listesi: `Light / Açık` (varsayılan),
  `Dark / Koyu`, `Follow Windows / Windows'a uy`. Seçim **hemen** uygulanır.
- Yanında **Mica** onay kutusu: sadece Windows 11 22H2+ başlık çubuğunda Mica efekti (Windows 10'da etkisiz).
- Windows 11'de pencere köşeleri yuvarlak.
- Yüksek DPI (125% / 150%) ekranlarda yazılar bulanık değil, net.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam; UI test kopyasının draft release'indeki
  `BrowserSelect-1.5.1.0-x64-Setup.exe` kurulu.
  > Dikkat: test kurulumu ana BrowserSelect kurulumunun **üzerine** kurulur (aynı klasör, aynı ayarlar).
  > Test bitince ana repodaki son sürümü tekrar kurarak geri dönebilirsin.
- Karşılaştırma için mümkünse eski sürümün (v1.4.6.0) ekran görüntülerini al. Otomatik karşılaştırma için:
  GitHub → Actions → **UI screenshots (manual)** → *Run workflow* (boş bırak = yeni görünüm; `ref` alanına
  `b894d9abc18ac61f8e2a15cb96d1f6a33a4b682c` yaz = eski görünüm). Sonuçlar *Artifacts → ui-screenshots* içinde
  (Light/Dark, EN/TR; seçim penceresi, Ayarlar, Hakkında, ? yardım + kontrol konumları `.txt`).

## Test linkleri

- Seçim penceresini açmak için: https://example.com/?bs-test=theme
  (ör. `tools\bs-open.ps1 https://example.com/?bs-test=theme`)

## 16A – Açık tema (varsayılan)

1. Linki aç → seçim penceresi gelir.
   - [ ] Pencere boyutu ve tarayıcı simgelerinin yeri eski sürümle aynı.
   - [ ] Tarayıcı adı, kısayol satırı `( 1,c )` ve `Always` butonu kesilmeden okunuyor (Segoe UI).
   - [ ] `Always`, sağdaki dikey `About` / `Settings` butonları düz, ince kenarlı; üzerine gelince açık mavi oluyor.
2. `Settings`'i aç.
   - [ ] Kontroller ve pencere boyutu eskisiyle aynı (sadece görünüm değişti).
   - [ ] Hiçbir yazı kesik değil (özellikle grup başlıkları, `Hold Alt on a link to skip rules`, alt satırdaki butonlar).
   - [ ] Kural tablosu: başlıklar düz gri, çizgiler açık gri, seçili satır açık mavi + siyah yazı.
   - [ ] En altta `Theme:` + açılır liste + `Mica` görünüyor, `Language:`'ın hemen altında.
   - [ ] `check now`'a basınca buton denetim sırasında mavi oluyor, sonra eski haline dönüyor.
3. `About` → `Original project info...`, `?` (yardım) ve `Settings → Help` pencerelerini aç.
   - [ ] Hepsi Segoe UI, düz butonlu; ayırıcı çizgiler ince.

## 16B – Koyu tema

1. `Settings` → `Theme` → `Dark`.
   - [ ] Ayarlar penceresi **hemen** koyulaşıyor: arka plan, listeler, kural tablosu, açılır listeler, butonlar.
   - [ ] Başlık çubuğu koyu (Windows 10 1809+ / Windows 11).
   - [ ] Kaydırma çubukları (tarayıcı listesi, kural tablosu) koyu (Windows 10 1809+).
   - [ ] Linkler (kural listesinin üstündeki GitHub linki) açık mavi ve okunuyor.
2. `Close` → linki tekrar aç.
   - [ ] Seçim penceresi koyu; simgeye sağ tık → `Open in Private Window` menüsü koyu.
3. `About`, `?` yardım, `Settings → Help`, `Add...`/`Edit...` (tarayıcı düzenleme) pencereleri koyu ve okunuyor.
   - [ ] Mesaj kutuları (ör. `check now` sonucu) Windows'un kendi açık renkli mesaj kutusu olarak kalabilir — bu beklenen.

## 16C – Windows'a uy

1. `Theme` → `Follow Windows`.
2. Windows Ayarlar → Kişiselleştirme → Renkler → **Varsayılan uygulama modu**: `Koyu` yap.
3. BrowserSelect pencerelerini kapatıp tekrar aç.
   - [ ] Koyu tema kullanılıyor.
4. Uygulama modunu `Açık` yap, pencereleri tekrar aç.
   - [ ] Açık tema kullanılıyor.

## 16D – Mica ve yuvarlak köşeler (Windows 11)

1. Windows 11'de `Mica` kutusunu işaretle.
   - [ ] Ayarlar penceresinin başlık çubuğu masaüstü arka planından hafif renk alıyor (Mica). İşareti kaldırınca normale dönüyor.
   - [ ] Pencere köşeleri yuvarlak.
2. Windows 10'da `Mica`'yı işaretle.
   - [ ] Hiçbir şey değişmiyor, hata yok.

## 16E – Yüksek DPI

1. Windows Ayarlar → Ekran → Ölçek: `%150` (veya iki farklı ölçekli monitör).
2. Seçim penceresini ve Ayarlar'ı aç; birden fazla monitör varsa pencereyi diğer monitöre sürükle.
   - [ ] Yazılar net (bulanık değil), kontroller üst üste binmiyor, butonlar kesilmiyor.

## 16F – Türkçe

1. `Language` → `Türkçe (TR)`, BrowserSelect'i yeniden başlat.
   - [ ] `Tema:` listesinde `Açık`, `Koyu`, `Windows'a uy`; ipuçları (tooltip) Türkçe.
   - [ ] `?` yardım penceresinde **Tema** satırı Türkçe.

## Geçti / Kaldı

- **Geçti:** 16A–16F'deki kutuların hepsi işaretli; hiçbir pencerede yazı kesilmiyor, kontrol yerleri değişmemiş.
- **Kaldı:** kesik yazı, okunmayan renk (ör. koyu temada siyah yazı), yer değiştirmiş kontrol, hata mesajı. Ekran görüntüsü al ve pencere adını not et.
