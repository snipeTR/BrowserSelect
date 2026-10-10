# 20 – Ayarlar penceresinin Windows 11 tasarımı (gezinti bölmesi, kartlar, anahtarlar)

> Bu reçete v1.5.7.0 ve sonrası içindir.

## Amaç

`Settings` penceresi Windows 11 Ayarlar uygulamasına benzer şekilde yeniden düzenlendi. **Hiçbir özellik
değişmedi, sadece yerleşim ve görünüm değişti.**

- Solda bir **gezinti bölmesi** var: `Browsers`, `Default Browser`, `Auto Select Filters`, `Options`,
  `Update checker` (Türkçe: `Tarayıcılar`, `Varsayılan Tarayıcı`, `Otomatik Seçim Kuralları`, `Seçenekler`,
  `Güncelleme denetimi`). Sayfa adları eski grup kutularının adlarıyla aynı, bu yüzden yardımdaki
  “Settings > Options” gibi yollar hâlâ doğru.
- Seçili öğe yuvarlak köşeli açık/koyu bir zeminle ve solunda **vurgu renginde dikey bir çubukla** gösterilir.
  Öğelerin solunda simge var (Windows 11: Segoe Fluent Icons, Windows 10: Segoe MDL2 Assets; ikisi de yoksa
  simgesiz).
- Gezinti bölmesinin altında (sol alt köşe) `Language`, `Theme` ve `Mica` duruyor.
- Sağda seçili sayfanın büyük başlığı ve altında **kartlar** (yuvarlak köşeli, ince kenarlıklı, zemini sayfadan
  biraz farklı bölümler) var; eski grup kutularının yerini aldılar.
- Butonlar yuvarlak köşeli. `Apply` ve `Set as Default Browser` **vurgu renginde** (Windows’un vurgu rengi).
- `Options` sayfasındaki onay kutuları ve `Mica`, `Update checker > enable` artık **açma/kapama anahtarı**
  (toggle). Tarayıcı listesindeki onay kutuları ve kural tablosundaki `Private` kutuları aynen duruyor.
- Açılır listeler (`Sort`, `Language`, `Theme`, `If all windows are full screen`) yuvarlak çerçeveli ve sağda ok
  simgeli.
- `Close` / `Apply` ve “Link opened from” yazısı her sayfada pencerenin sağ altında.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam; `BrowserSelect-1.5.7.0-x64-Setup.exe` (veya daha yeni) kurulu.
- En az iki tarayıcı. Mümkünse Windows 11 (Mica ve yuvarlak köşeler için) ve %100 dışında bir ekran ölçeği
  denenebilecek bir bilgisayar.

## Test linkleri

- https://example.com/?bs-test=settings-v157
- https://example.org/?bs-test=settings-v157

## Adımlar

### Test 20A – Gezinti bölmesi

1. BrowserSelect’i Başlat menüsünden aç → `Settings`.

- [ ] Pencere `Browsers` sayfasıyla açılıyor; soldaki `Browsers` öğesi seçili (yuvarlak zemin + vurgu renginde
      çubuk), sağda büyük `Browsers` başlığı var.
- [ ] Sırayla `Default Browser`, `Auto Select Filters`, `Options`, `Update checker` öğelerine tıkla: her
      tıklamada sağdaki sayfa ve başlık değişiyor, seçim çubuğu tıklanan öğeye geçiyor.
- [ ] Fareyle bir öğenin üzerine gelince (seçili değilse) hafif bir zemin beliriyor, ayrılınca kayboluyor.
- [ ] Klavye: `Tab` ile gezinti bölmesine gel (öğe etrafında odak çerçevesi çıkıyor), `↑` / `↓` ile öğeler
      arasında dolaşınca sayfa da değişiyor.
- [ ] Simgeler görünüyor (Windows 10’da da). Simge yerine kare/garip karakter görünmüyor.

### Test 20B – Browsers sayfası

- [ ] Tarayıcı listesi yuvarlak çerçeveli bir alanda; onay kutularını kaldırıp koymak tarayıcıyı gizliyor /
      gösteriyor (ana pencerede kontrol et).
- [ ] Sağdaki `Add...`, `Edit...`, `Remove`, `Refresh`, `Sort` ve ▲ / ▼ (ok simgeleri) eskisi gibi çalışıyor;
      liste boşken / seçim yokken `Edit...` ve `Remove` soluk (devre dışı) görünüyor.
- [ ] Bir tarayıcıya çift tıklamak `Edit browser` penceresini açıyor.

### Test 20C – Default Browser sayfası

- [ ] Açıklama yazısı tam görünüyor (kesik değil).
- [ ] `Set as Default Browser` vurgu renginde; BrowserSelect zaten varsayılansa soluk (devre dışı).
- [ ] `File types...` eskisi gibi mesajı gösterip Windows varsayılan uygulamalar ekranını açıyor.

### Test 20D – Auto Select Filters sayfası

1. `+` ile iki kural ekle (example.com → Tarayıcı A, example.org → Tarayıcı B).

- [ ] İlk değişiklikte sağ alttaki `Apply` aktif oluyor (vurgu rengi), `Close` `Cancel` oluyor.
- [ ] `Help`, `Move Up`, `Move Down`, `Delete` ve Del tuşu eskisi gibi çalışıyor (bkz. [19](19-kural-ekleme-arti-satiri.md)).
- [ ] Kural listesinin üstündeki link yardım sayfasını açıyor.
- [ ] `Apply` → `Close`; `.\bs-open.ps1 "https://example.com/?bs-test=settings-v157"` Tarayıcı A’da açılıyor.
- [ ] Kaydetmeden başka bir sayfaya geçip pencereyi kapatınca “kaydedilmemiş değişiklik” sorusu yine geliyor.

### Test 20E – Options sayfası (anahtarlar)

- [ ] `Show running browsers only`, `Hold Alt on a link to skip rules`, `Avoid full-screen windows` birer açma/
      kapama anahtarı; açıkken anahtar vurgu renginde ve topuz sağda, kapalıyken içi boş ve topuz solda.
- [ ] Yazıya veya anahtara tıklamak ayarı değiştiriyor; `Tab` ile anahtara gelip `Boşluk` tuşu da değiştiriyor.
- [ ] Ayarlar anında kaydediliyor: `Settings`’i kapatıp açınca anahtarlar aynı durumda.
- [ ] `Avoid full-screen windows` kapalıyken `If all windows are full screen:` yazısı ve açılır listesi soluk.
- [ ] `Export...` / `Import...` eskisi gibi çalışıyor (bkz. [07](07-export-import.md)).

### Test 20F – Update checker sayfası

- [ ] `enable` anahtarı güncelleme denetimini açıp kapatıyor (kapatıp `Settings`’i yeniden açınca kapalı kalıyor).
- [ ] `check now`a basınca buton istek sürerken vurgu rengine dönüyor, sonra “güncel” ya da güncelleme mesajı
      geliyor ve buton normale dönüyor.
- [ ] Altta geri bildirim yazısı tam görünüyor.

### Test 20G – Dil, tema ve Mica (sol alt)

- [ ] `Theme` → `Dark`: pencere hemen koyulaşıyor; kartlar sayfa zemininden biraz açık, kenarlıklar seçilebiliyor,
      yazılar okunuyor, açılır listeler ve anahtarlar koyu temaya uygun. `Light` ve `Follow Windows` da çalışıyor.
- [ ] Windows’ta vurgu rengini değiştir (Ayarlar → Kişiselleştirme → Renkler, örn. yeşil) → `Settings`’i kapatıp
      aç: seçim çubuğu, `Apply`, açık anahtarlar yeni vurgu renginde.
- [ ] `Mica` anahtarı (Windows 11 22H2+) başlık çubuğuna Mica efektini ekliyor/kaldırıyor.
- [ ] `Language` değiştirilince yeniden başlatma mesajı geliyor; yeniden başlattıktan sonra gezinti öğeleri, sayfa
      başlıkları ve butonlar yeni dilde.

### Test 20H – Diller (uzun metinler ve Japonca/Çince)

Her biri için `Language`’i değiştir, BrowserSelect’i yeniden başlat ve **beş sayfanın hepsine** bak:
`Deutsch`, `Русский`, `Polski`, `Français`, `日本語`, `简体中文`.

- [ ] Gezinti öğelerinde yazı kesilmiyor (uzun adlar iki satıra bölünebilir, örn. Almanca
      “Regeln für automatische Auswahl”).
- [ ] Butonlarda, anahtar yazılarında ve sayfa başlıklarında taşma veya `...` ile kesilme yok.
- [ ] Japonca/Çince: yazılar kare değil, okunaklı (Yu Gothic UI / Microsoft YaHei UI); başlık kalın.

### Test 20I – Ekran ölçeği (DPI) ve küçük ekran

1. Windows Ayarlar → Ekran → Ölçek: `%100`, `%125`, `%150`, `%175` (ve mümkünse `%200`). Her birinde oturumu
   kapatıp aç, `Settings`’i aç.

- [ ] Yazılar ve simgeler net; yuvarlak köşeler, seçim çubuğu ve anahtarlar ölçekle birlikte büyüyor, bozuk değil.
- [ ] 1920x1080 ekranda %175’te pencere ekrana sığıyor (görev çubuğunun altına taşmıyor, kaydırma çubuğu
      gerekmiyor). 1366x768 ekranda %100’de de sığıyor.
- [ ] Pencereyi büyütünce tarayıcı listesi ve kural tablosu büyüyor; en küçük boyuta küçültünce hiçbir buton
      başka bir kontrolün üstüne binmiyor.

## Geçti / kaldı

Bütün kutular işaretlendiyse **geçti**. Kesik yazı, üst üste binen kontrol, çalışmayan bir buton/anahtar veya
koyu temada okunmayan bir yazı varsa ekran görüntüsüyle birlikte (dil, tema, ölçek bilgisiyle) **kaldı** yaz.
