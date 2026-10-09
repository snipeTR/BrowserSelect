# 17 – Tam ekran pencereleri atla (tek ve çoklu monitör)

> Bu reçete v1.5.3.0 ve sonrası içindir. Çoklu monitör bölümleri (17D–17G) **gerçek iki (veya üç) monitörlü**
> bir bilgisayarda yapılmalıdır; GitHub'ın test makinesinde ikinci monitör yoktur, bu kısımlar otomatik test edilmedi.
> v1.5.3.0 gerçek çoklu monitörlü bir bilgisayarda bu reçeteyle elle test edildi ve doğru çalıştı.

## Amaç

Tarayıcı yeni bir linki **en son kullandığınız pencerede** açar. O pencere başka monitörde tam ekran video
oynatıyorsa link videoyu böler. BrowserSelect linki tarayıcıya vermeden hemen önce:

1. Seçilen tarayıcının görünür pencerelerine bakar (araç pencereleri, gizli/sıfır boyutlu pencereler sayılmaz).
2. Tam ekran **olmayan** pencere varsa, bunlardan **en son kullanılanı** öne getirir → link oraya gider.
   Simge durumuna küçültülmüş pencere sadece başka aday yoksa kullanılır (ve geri açılır).
3. Tarayıcının **bütün** pencereleri tam ekransa `If all windows are full screen` / `Tüm pencereler tam ekransa`
   ayarı geçerlidir:
   - `Window on primary monitor` / `Ana monitördeki pencere` (varsayılan): Windows'un **ana ekranındaki** pencere
     öne getirilir.
   - `Last used window` / `Son kullanılan pencere`: hiçbir şey yapılmaz, tarayıcı karar verir.
4. `Avoid full-screen windows` / `Tam ekran pencereleri atla` kapalıysa BrowserSelect hiçbir pencereye dokunmaz.

**Tam ekran** = pencere monitörün tamamını (görev çubuğu dahil) kaplar **ve** başlık çubuğu yoktur (F11, tam
ekran video). **Ekranı kaplayan (maximize) pencere tam ekran sayılmaz.**

Gizli pencerede açarken (Shift+tık, Private kural), yeni pencere açan parametrelerde (`--new-window`,
`--incognito`, `-private-window`, `--app=...`, `--kiosk` …) ve tarayıcı kapalıyken bu adım atlanır.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam; `BrowserSelect-1.5.3.0-x64-Setup.exe` (veya daha yeni) kurulu
  ([Releases](https://github.com/snipeTR/BrowserSelect/releases) sayfasından).
- En az bir tarayıcı: **Google Chrome** (veya Edge). Mümkünse **Firefox** ile de tekrarla (17H).
- Tam ekran için uzun bir video: ör. https://www.youtube.com/watch?v=aqz-KE-bpKQ (herhangi bir video olur)
  veya herhangi bir sayfada **F11**.
- Linki tarayıcı **dışından** açmak için: `tools\bs-open.ps1` (aşağıda) veya Not Defteri / Outlook / Teams'teki
  bir link. (Tarayıcının içinde tıklanan link BrowserSelect'e gitmez.)
- Seçim penceresiyle uğraşmamak için bir kural ekle: `Settings` → kural listesine
  `Domain` | `example.com` | `Google Chrome` → `Apply`. (Testten sonra silebilirsin.)

### Link nasıl açılır?

Her testte sıra şöyle:

1. Tarayıcının "son kullanılan" olmasını istediğin penceresine (ör. tam ekran video) **bir kez tıkla**.
   Tarayıcı, yeni linki normalde bu pencereye gönderir.
2. Sonra **ana monitördeki** PowerShell penceresine geç (tıkla veya Alt+Tab; tam ekran pencere tam ekran kalır)
   ve linki aç:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\bs-open.ps1 https://example.com/?bs-test=fs1
```

(Ya da Not Defteri / Outlook / Teams'teki bir linke tıkla.) Linki açan pencere **önde** olmalı: Windows,
sadece öndeki programın başlattığı programın başka bir pencereyi öne getirmesine izin verir. Gerçek kullanımda
da linke tıkladığın program öndedir. (`-Delay` ile bekleyip tarayıcıya tıklamak bu yüzden **yanlış sonuç**
verir; kullanma.)

> Her testte linkin **hangi pencerede yeni sekme olarak açıldığına** bak. Her seferinde `fs1`, `fs2` …
> gibi farklı bir link kullan ki sekmeleri karıştırma.

## 17A – Ayar penceresi (EN / TR)

1. `Settings` → sol taraftaki `Options` grubu.
   - [ ] `Avoid full-screen windows` onay kutusu **işaretli** (varsayılan).
   - [ ] Altında `If all windows are full screen:` yazısı ve açılır liste: `Window on primary monitor` seçili.
   - [ ] Listede iki seçenek var: `Window on primary monitor`, `Last used window`.
   - [ ] Onay kutusunu kaldırınca yazı ve açılır liste **soluklaşıyor** (pasif); işaretleyince geri geliyor.
   - [ ] `Export...` / `Import...` butonları grubun en altında, hiçbir yazı kesik değil.
2. Fareyi onay kutusu ve listenin üstünde beklet.
   - [ ] İpuçları (tooltip) çıkıyor ve ne yaptığını anlatıyor.
3. Dili Türkçe yap (`Language` → `Türkçe`), BrowserSelect'i kapatıp aç.
   - [ ] `Tam ekran pencereleri atla`, `Tüm pencereler tam ekransa:`, `Ana monitördeki pencere` / `Son kullanılan pencere`.
4. Ayarları değiştir, `Settings`'i kapatıp aç.
   - [ ] Seçimler korunuyor.
5. Ekran ölçeği %125 / %150 / %175'te `Settings`'i aç.
   - [ ] Yeni kontroller kesilmiyor, üst üste binmiyor; pencere ekrana sığmıyorsa kaydırma çubuğu çıkıyor.
6. `Export...` ile kaydet, dosyayı Not Defteri ile aç.
   - [ ] `"avoidFullscreen": true` ve `"fullscreenFallback": "Primary"` satırları var.
   - [ ] Değerleri değiştirip (`false`, `"LastUsed"`) `Import...` → Ayarlar penceresinde yeni değerler görünüyor.

## 17B – Tek monitör: tam ekran pencereye link gitmemeli

1. Chrome'da **iki pencere** aç (Ctrl+N). Pencere 1: normal (maximize olabilir). Pencere 2: video aç, **F11**
   (veya videoda tam ekran düğmesi).
2. **Tam ekran Pencere 2'ye** bir kez tıkla, sonra PowerShell'e geçip `bs-open.ps1 https://example.com/?bs-test=fs1` çalıştır.
   - [ ] Pencere 1 öne geliyor, link **Pencere 1'de** yeni sekme olarak açılıyor.
   - [ ] Pencere 2'deki video tam ekran kalıyor / bölünmüyor (Alt+Tab ile bak).
3. Aynısını Pencere 1 **küçültülmüş (simge durumunda)** iken yap.
   - [ ] Pencere 1 geri açılıyor ve link oraya gidiyor.

## 17C – Tek monitör: ayar kapalı ve "hepsi tam ekran"

1. `Avoid full-screen windows` işaretini **kaldır**. 17B-2'yi tekrarla (`fs2`).
   - [ ] Link Chrome'un son kullandığı pencerede (tam ekran Pencere 2) açılıyor = eski/normal tarayıcı davranışı.
2. İşareti geri koy. Chrome'da **tek pencere** bırak ve onu F11 ile tam ekran yap. Link aç (`fs3`).
   - [ ] Link o tek pencerede açılıyor (başka seçenek yok, hata/çökme yok).

## 17D – İki monitör: video 2. monitörde, link 1. monitördeki pencereye

Hazırlık: Windows Ayarlar → Sistem → Ekran'da hangi monitörün **ana ekran** olduğunu not et
("Bunu ana ekranım yap" işaretli olan).

1. Chrome Pencere A → **1. monitör** (normal veya maximize). Chrome Pencere B → **2. monitör**, video aç ve
   videoyu **tam ekran** yap.
2. **2. monitördeki videoya** bir kez tıkla, sonra 1. monitördeki PowerShell'den `bs-open.ps1 https://example.com/?bs-test=fs4` çalıştır.
   - [ ] Link **Pencere A'da** (1. monitör) açılıyor; video 2. monitörde oynamaya / tam ekran kalmaya devam ediyor.
3. Tersini dene: video 1. monitörde tam ekran, normal pencere 2. monitörde (`fs5`).
   - [ ] Link 2. monitördeki normal pencereye gidiyor (monitör sırası önemli değil, tam ekran olmayan pencere seçilir).
4. Üç pencere: A (1. monitör, normal), C (1. monitör, normal, A'dan **sonra** kullanılmış), B (2. monitör, tam ekran).
   Önce A'ya, sonra C'ye, sonra B'ye tıkla, link aç (`fs6`).
   - [ ] Link **C'de** açılıyor (tam ekran olmayanların en son kullanılanı).

## 17E – İki monitör: bütün pencereler tam ekran → yedek ayar

1. Pencere A (ana monitör) ve Pencere B (diğer monitör) ikisi de **F11 / tam ekran**.
2. Yedek ayar `Window on primary monitor`. Önce **B'ye** (ana olmayan monitör) tıkla, sonra linki aç (`fs7`).
   - [ ] Link **ana monitördeki A'da** açılıyor.
3. Ayarı `Last used window` yap, 2. adımı tekrarla (`fs8`).
   - [ ] Link **B'de** (son kullanılan) açılıyor = karar tarayıcıda.
4. Ana ekranı değiştir (Windows Ekran ayarlarında diğer monitörü "ana ekran" yap; ana ekranın **solda olmadığı**
   bir düzen olursa daha iyi). Ayar `Window on primary monitor`, tekrar dene (`fs9`).
   - [ ] Link **yeni ana ekrandaki** pencerede açılıyor (en soldaki monitör değil).
5. Hepsi tam ekran ama **ana monitörde hiç Chrome penceresi yok** (ör. iki tam ekran pencere de 2. ve 3. monitörde).
   - [ ] Hata yok; link tarayıcının son kullandığı pencerede açılıyor.

## 17F – Maximize ≠ tam ekran (görev çubuğu otomatik gizli)

1. Windows: Görev çubuğu ayarları → **Görev çubuğunu otomatik olarak gizle** açık.
2. Pencere A: **maximize** (F11 değil, başlık çubuğundaki kare düğme). Pencere B: F11 tam ekran.
3. Önce B'ye tıkla, sonra linki aç (`fs10`).
   - [ ] Link **A'da** açılıyor (maximize pencere tam ekran sayılmadı).
4. Görev çubuğu ayarını eski haline getir.

## 17G – Atlanması gereken durumlar

Düzen: Pencere A normal, Pencere B tam ekran ve son kullanılan.

1. Seçim penceresini aç (kuralsız bir link, ör. `https://example.org/?bs-test=fs11`), **Shift** basılıyken Chrome'a tıkla.
   - [ ] Yeni **gizli** pencere açılıyor; A veya B öne getirilmiyor.
2. Kural: `Domain` | `example.net` | `Google Chrome` | Arguments: `--new-window` → `Apply`. `https://example.net/?bs-test=fs12` aç.
   - [ ] Link **yeni bir pencerede** açılıyor; A öne getirilmiyor. (Testten sonra kuralı sil.)
3. Chrome'u tamamen kapat, link aç (`fs13`).
   - [ ] Chrome normal şekilde açılıyor ve link açılıyor (hata yok).
4. `ignore URL` kuralı olan bir link (ör. 02 reçetesindeki `example.net`) → hiçbir pencere öne gelmiyor.

## 17H – Firefox ve Edge ile tekrar

17B, 17D ve 17E-2'yi **Firefox** (F11 veya video tam ekranı) ve **Microsoft Edge** ile tekrarla.
- [ ] Sonuçlar Chrome ile aynı.

## Bilinen sınırlar (hata sayılmaz)

- Aynı tarayıcının **farklı profilleri** (aynı .exe, ör. Chrome "İş" ve "Kişisel") tek tarayıcı sayılır: link başka
  profilin penceresi öne getirildiği için o profilde açılabilir.
- YouTube **sinema modu** / tarayıcı içinde büyütülmüş video tam ekran değildir.
- Windows nadiren başka bir pencerenin öne getirilmesine izin vermez; o zaman pencere görev çubuğunda yanıp
  söner, link yine de açılır.
- Farklı sanal masaüstündeki pencereler sayılmaz.

## Sonuç

Her alt test için ✅ / ❌ yaz; ❌ ise monitör düzenini (kaç monitör, hangisi ana ekran, ölçekler), tarayıcıyı ve
linkin hangi pencerede açıldığını not et.

| Test | Sonuç | Not |
|---|---|---|
| 17A Ayarlar EN/TR, export/import | | |
| 17B Tek monitör, tam ekran atlandı | | |
| 17C Ayar kapalı / tek tam ekran pencere | | |
| 17D İki monitör, video 2. monitörde | | |
| 17E Hepsi tam ekran: ana monitör / son kullanılan | | |
| 17F Maximize + otomatik gizli görev çubuğu | | |
| 17G Gizli / --new-window / tarayıcı kapalı | | |
| 17H Firefox / Edge | | |
