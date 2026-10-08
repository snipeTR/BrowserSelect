# 14 – Yardım pencereleri (EN/TR) ve kural dokümanı linki

## Amaç

- Tarayıcı listesindeki **?** butonu ve Ayarlar → Otomatik Seçim Kuralları → **Help / Yardım** butonu ile açılan yardım
  pencereleri yeni özelliklere göre güncellendi, İngilizce + Türkçe oldu ve kaydırılabilir.
- Kural listesinin üstündeki link artık bu fork’un dokümanını açıyor:
  - İngilizce arayüz: https://github.com/snipeTR/BrowserSelect/blob/master/help/filters.md
  - Türkçe arayüz: https://github.com/snipeTR/BrowserSelect/blob/master/help/filters.tr.md
- “Always” butonu, sağ tık “Open in Private Window” menüsü ve kural hata mesajları da çevrildi.

## Ön koşullar

- **v1.4.5.0 veya üstü** kurulu, BrowserSelect varsayılan tarayıcı ([00](00-hazirlik.md)).

## Test linkleri

- https://example.com/?bs-test=help
- https://github.com/snipeTR/BrowserSelect/blob/master/help/filters.md
- https://github.com/snipeTR/BrowserSelect/blob/master/help/filters.tr.md

## Adımlar

### Test 14A – Ana yardım (İngilizce)

1. Dil `English (EN)` iken `.\bs-open.ps1 "https://example.com/?bs-test=help"` → sağdaki dikey butonların altındaki **?**.

- [ ] Başlık “BrowserSelect - Help”.
- [ ] Metinde THE BROWSER LIST / AUTO SELECT RULES / SETTINGS bölümleri var; Shift ile gizli pencere, sağ tık menüsü,
      “Always”, Alt ile kuralları atlama, Add/Edit/Remove/Sort/Refresh, File types, Export/Import, Language anlatılıyor.
- [ ] Pencere küçültülünce dikey kaydırma çubuğu çıkıyor; metin `Close` butonunun altında kalmıyor.
- [ ] Açılışta metnin hiçbir kısmı seçili (mavi) değil; Esc veya `Close` kapatıyor.

> Not: Yeni sürüm yayınlandıysa **?** butonu güncelleme butonuna dönüşür; bu durumda yardım bu sürümde açılamaz.

### Test 14B – Kural yardımı (İngilizce)

1. Başlat menüsünden BrowserSelect → `Settings` → Auto Select Filters bölümündeki `Help`.

- [ ] Başlık “BrowserSelect - Auto Select rules help”; Match türleri (Domain…Regex), Pattern, Browser, Private, Arguments,
      öncelik sırası, Delete/Del, Apply ve “Link opened from” anlatılıyor.

### Test 14C – Kural dokümanı linki

1. Ayarlarda kural listesinin üstündeki metindeki **the project's github.** linkine tıkla.

- [ ] Tarayıcıda `github.com/snipeTR/BrowserSelect/blob/master/help/filters.md` açıldı (zumoshi değil).
- [ ] Sayfada Match tablosu (Domain, URL, Path, Keyword, Extension, Source App, Regex) ve 7 örnek var.

### Test 14D – Türkçe

1. Dili `Türkçe (TR)` yap, BrowserSelect’i yeniden başlat ([12](12-turkce-dil.md)).
2. Test 14A–14C’yi tekrarla.

- [ ] Ana yardım başlığı “BrowserSelect - Yardım”, metin Türkçe (TARAYICI LİSTESİ / OTOMATİK SEÇİM KURALLARI / AYARLAR), buton `Kapat`.
- [ ] Kural yardımı başlığı “BrowserSelect - Otomatik seçim kuralları yardımı”, metin Türkçe.
- [ ] Kural listesinin üstündeki **projenin GitHub sayfasında** linki `.../help/filters.tr.md` sayfasını açtı (Türkçe doküman).
- [ ] Tarayıcı listesinde butonlar `Her zaman`; bir tarayıcı simgesine sağ tıklayınca menü `Gizli pencerede aç`.
- [ ] Ayarlarda tarayıcısı seçilmemiş bir kural (ör. desen `example.org`, tarayıcı boş) → `Uygula`:
      “Geçersiz kural: 'example.org' kuralı için tarayıcı seçmeyi unuttunuz.” benzeri Türkçe mesaj.
