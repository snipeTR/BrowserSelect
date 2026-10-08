# 15 – Güncellemeyi indirip kurma ve kurulumda çalışan programı kapatma

## Amaç

- Güncelleme denetimi yeni sürüm bulduğunda, “Yeni güncelleme var!” mesajında `Tamam`’a basınca
  **“Yeni sürümü şimdi indirip çalıştırmak ister misiniz?”** (Evet/Hayır) sorusu geliyor.
  - **Evet:** son *yayınlanmış* sürümün kurulum dosyası (`BrowserSelect-<sürüm>-x64-Setup.exe`) GitHub API üzerinden
    bulunup `%TEMP%` klasörüne indiriliyor (ilerleme çubuğu + `İptal`), ardından kurulum başlatılıyor.
  - **Hayır:** hiçbir şey indirilmiyor.
  - Hata olursa (ağ yok, sürümde .exe yok, dosya eksik) Türkçe/İngilizce hata mesajı + “Sürümler sayfasını açıp elle
    indirmek ister misiniz?” sorusu; `Evet` → https://github.com/snipeTR/BrowserSelect/releases/latest açılıyor.
- Kurulum (ve kaldırma) başlarken **BrowserSelect çalışıyorsa** kapatmak için izin istiyor:
  - `Evet` → program kapatılıyor (önce normal kapatma, gerekirse zorla) ve kurulum devam ediyor.
  - `Hayır` → “Lütfen programı kapatıp kurulumu yeniden başlatın.” uyarısı ve kurulum kapanıyor.
- Kurulum programı Windows görüntüleme dili **Türkçe** ise Türkçe, değilse İngilizce açılıyor.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam.
- **v1.4.6.0 veya üstü** kurulu (Ayarlar → Hakkında’da sürüm görünür).
- İnternet bağlantısı.
- Önerilen: **v1.4.6.0 yayınlanmış olsun.** Güncelleyici sadece *yayınlanmış* (draft olmayan) son sürümü indirir.
  1.4.6.0 henüz draft ise indirilen dosya 1.4.5.0 olur; Test 15B sonunda 1.4.5.0 kurulursa 1.4.6.0’ı tekrar kur.

## Test linkleri

- Son yayınlanmış sürüm: https://github.com/snipeTR/BrowserSelect/releases/latest
- Güncelleyicinin kullandığı API: https://api.github.com/repos/snipeTR/BrowserSelect/releases/latest
  (tarayıcıda açınca `assets` → `browser_download_url` içinde `...-x64-Setup.exe` görünmeli)
- Seçim penceresini açmak için: https://example.com/?bs-test=update

## Hazırlık – “yeni sürüm var” durumunu taklit etmek

Kurulu sürümden daha yeni bir sürüm yayınlanmadıysa güncelleme butonu görünmez. Test için ayar dosyasında son
sürümü elle büyük bir değer yap:

1. BrowserSelect’i tamamen kapat.
2. PowerShell’de ayar dosyasını bul:
   ```powershell
   Get-ChildItem $env:LOCALAPPDATA -Recurse -Filter user.config -ErrorAction SilentlyContinue |
     Where-Object FullName -like '*BrowserSelect*' | Select-Object FullName
   ```
3. En yeni sürüm klasöründeki (`...\1.4.6.0\user.config`) dosyayı Not Defteri ile aç.
4. `<setting name="last_version" ...>` altındaki `<value>nope</value>` değerini `<value>9.9.9.9</value>` yap, kaydet.
   (Satır yoksa önce Ayarlar’da bir ayarı değiştirip `Uygula`’ya bas, dosya oluşur.)

- [ ] `.\bs-open.ps1 "https://example.com/?bs-test=update"` ile açılan seçim penceresinde sağdaki **?** butonu
      güncelleme butonuna dönüştü.

> Not: Ayarlar → `Şimdi denetle` gerçek denetim yapar ve yeni sürüm yoksa “güncel” der, `9.9.9.9` değerini de
> `nope`’a geri çevirir. Bu yüzden 15A–15D’de **seçim penceresindeki güncelleme butonunu** kullan.

## Adımlar

### Test 15A – Soru geliyor, Hayır indirmiyor

1. Güncelleme butonuna tıkla.
2. “Yeni güncelleme var!” mesajında `Tamam`.
3. “Yeni sürümü (9.9.9.9) şimdi indirip çalıştırmak ister misiniz?” sorusunda `Hayır`.

- [ ] `Tamam`’dan sonra Evet/Hayır sorusu geldi (soru gelmeden indirme başlamadı).
- [ ] `Hayır` sonrası pencere/indirme yok; `%TEMP%` klasöründe yeni `BrowserSelect-*-x64-Setup.exe` oluşmadı.

### Test 15B – Evet: indir ve kurulumu başlat (program açıkken)

1. 15A’yı tekrarla ama soruda `Evet`.
2. “BrowserSelect - Güncelleme indiriliyor” penceresini izle.
3. Kurulum açılınca BrowserSelect hâlâ açık olduğu için soru gelecek: “BrowserSelect şu anda çalışıyor. …
   BrowserSelect şimdi kapatılsın mı?” → `Evet`.
4. Kurulumu bitir (`Kabul ediyorum` → `Kur` → `Kapat`).

- [ ] Önce “Son sürümün kurulum dosyası aranıyor...”, sonra “BrowserSelect-x.y.z.w-x64-Setup.exe indiriliyor...”
      ve dolan ilerleme çubuğu + KB sayacı göründü; BrowserSelect penceresi donmadı.
- [ ] İndirme bitince kurulum kendiliğinden açıldı; dosya `%TEMP%\BrowserSelect-x.y.z.w-x64-Setup.exe` konumunda.
- [ ] Kurulum BrowserSelect’in açık olduğunu fark edip izin istedi; `Evet` sonrası BrowserSelect penceresi kapandı.
- [ ] Kurulum hatasız bitti (“dosya yazılamadı” hatası yok); Başlat menüsünden BrowserSelect açılıyor.
- [ ] Windows dili Türkçe ise kurulum ekranları ve sorular Türkçe (başlık “BrowserSelect … (64-bit) Kurulumu”).

> SmartScreen / antivirüs uyarısı çıkarsa (imzasız dosya) `Daha fazla bilgi` → `Yine de çalıştır`.
> Uyarıyı iptal edersen kurulum başlamaz, hata mesajı da çıkmaz (bilinçli).

### Test 15C – İndirmeyi iptal etme

1. Hazırlık’taki `9.9.9.9` ayarını tekrar yap (15B’den sonra sıfırlanmış olabilir).
2. Güncelleme butonu → `Tamam` → `Evet` → indirme penceresinde hemen `İptal` (veya Esc / pencereyi kapat).

- [ ] Pencere kapandı, kurulum başlamadı, hata mesajı çıkmadı.
- [ ] `%TEMP%` içinde yarım kalmış kurulum dosyası kalmadı.

> Dosya küçük (~700 KB) olduğu için hızlı bağlantıda iptale yetişemeyebilirsin; o zaman bu testi ⚠️ olarak işaretle.

### Test 15D – Ağ hatası ve sürümler sayfası

1. `9.9.9.9` ayarı hâlâ duruyorken interneti kes (Wi-Fi kapat / Uçak modu).
2. Güncelleme butonu → `Tamam` → `Evet`.
3. Hata mesajında `Evet`.
4. İnterneti tekrar aç.

- [ ] “Güncelleme indirilemedi (ağ hatası: …).” ve altında “Sürümler sayfasını açıp yeni sürümü elle indirmek ister
      misiniz? https://github.com/snipeTR/BrowserSelect/releases/latest” mesajı geldi.
- [ ] `Evet` → tarayıcıda releases/latest adresi açılmaya çalışıldı; `Hayır` → hiçbir şey açılmadı.

### Test 15E – Kurulumda “Hayır” (programı kapatma)

1. BrowserSelect’i Başlat menüsünden aç ve açık bırak.
2. `%TEMP%` içindeki (veya releases sayfasından indirdiğin) `BrowserSelect-...-x64-Setup.exe` dosyasını çalıştır.
3. “BrowserSelect şimdi kapatılsın mı?” sorusunda `Hayır`.

- [ ] “BrowserSelect çalışırken kurulum devam edemez. Lütfen programı kapatıp kurulumu yeniden başlatın.” uyarısı geldi.
- [ ] `Tamam` sonrası kurulum kapandı; BrowserSelect açık kalmaya devam ediyor; hiçbir dosya değişmedi.
- [ ] BrowserSelect’i kapatıp kurulumu yeniden başlatınca soru gelmeden lisans ekranı açıldı.

### Test 15F – Kaldırırken programın açık olması

1. BrowserSelect’i aç ve açık bırak.
2. Ayarlar → Uygulamalar → BrowserSelect → `Kaldır` (veya `%LOCALAPPDATA%\BrowserSelect\Uninstall.exe`).
3. Önce `Hayır`, sonra kaldırmayı tekrar başlatıp `Evet` dene.

- [ ] Soru: “BrowserSelect şu anda çalışıyor. Kaldırılabilmesi için programın kapatılması gerekiyor. …”.
- [ ] `Hayır` → “BrowserSelect çalışırken kaldırılamaz. Lütfen programı kapatıp kaldırma işlemini yeniden başlatın.” ve çıkış.
- [ ] `Evet` → BrowserSelect kapandı, kaldırma tamamlandı, `%LOCALAPPDATA%\BrowserSelect\BrowserSelect.exe` silindi.
- [ ] Sonra tekrar kur ([00](00-hazirlik.md)) — diğer testler için gerekli.

### Test 15G – İngilizce kontrolü

1. Uygulama dilini `English (EN)` yap ve yeniden başlat ([12](12-turkce-dil.md)); 15A–15B’yi tekrarla.

- [ ] Sorular İngilizce: “Do you want to download and run the new version (9.9.9.9) now?”, indirme penceresi
      “BrowserSelect - Downloading update”, `Cancel` butonu.
- [ ] Windows görüntüleme dili İngilizce olan bir bilgisayarda kurulum soruları İngilizce:
      “BrowserSelect is currently running. … Do you want to close BrowserSelect now?” /
      “Please close the program and restart the setup.”

## Notlar

- Güncelleyici yalnızca **yayınlanmış** sürümleri görür; draft’lar ne denetimde ne indirmede kullanılır.
- Sessiz kurulumda (`Setup.exe /S`) soru sorulmaz, çalışan BrowserSelect otomatik kapatılır. Bu akış her
  GitHub Actions derlemesinde otomatik test ediliyor (“Smoke test installer” adımı).
- Kurulum sadece **senin kullanıcı hesabında** çalışan BrowserSelect’i kapatır.
