# 10 – Özel tarayıcı ikonları

## Amaç

Her tarayıcıya (kayıt defterinden bulunan veya elle eklenen) **özel ikon** atanabilir:
`Settings` → `Browsers` → tarayıcıyı seç → `Edit...` → `Icon:` → **`Change...`**.
Desteklenen dosyalar: `.ico`, `.exe`, `.dll`, `.png`, `.jpg`, `.jpeg`, `.bmp`, `.gif`.
**`Default`** butonu tarayıcının kendi ikonuna döndürür. Özel ikon `Refresh` ve yeniden başlatma sonrasında korunur.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam.
- Test dosyaları: `files\test-icon.png` ve `files\test-icon.ico` (kırmızı daire içinde beyaz **“BS”**).

## Test linki

- https://example.com/?bs-test=icon

## Adımlar

### Test 10A – PNG ikon

1. BrowserSelect’i Başlat menüsünden aç → `Settings` → `Browsers` → **Tarayıcı A**’yı seç → `Edit...`.
2. `Icon:` yanındaki **`Change...`** → `files\test-icon.png` → `Aç`.
3. Önizlemeye bak → `OK` → `Close`.
4. Ana pencereye bak.

- [ ] `Edit...` penceresindeki önizleme kırmızı **BS** ikonunu gösteriyor.
- [ ] Ana pencerede Tarayıcı A’nın ikonu kırmızı **BS** oldu (bozuk/kesik değil, ölçekli).
- [ ] İkona tıklayınca link hâlâ Tarayıcı A’da açılıyor (`.\bs-open.ps1 "https://example.com/?bs-test=icon"`).

### Test 10B – ICO ikon

1. Tarayıcı B için aynı adımlar, dosya: `files\test-icon.ico`.

- [ ] Tarayıcı B’nin ikonu kırmızı **BS** oldu.

### Test 10C – EXE / DLL’den ikon

1. Tarayıcı A → `Edit...` → `Change...` → `C:\Windows\System32\notepad.exe` → `OK`.
2. Tekrar dene: `C:\Windows\System32\shell32.dll`.

- [ ] Not Defteri ikonu, sonra shell32’nin ilk ikonu (genelde boş sayfa/belge simgesi) göründü.

### Test 10D – Geçersiz dosya

1. `Change...` → filtreyi `All files (*.*)` yap → `files\test-links.txt` seç.

- [ ] `Unable to load an icon from this file.` uyarısı çıktı, uygulama çökmedi, önceki ikon değişmedi.

### Test 10E – Kalıcılık

1. Tarayıcı A’ya tekrar `test-icon.png` ata.
2. `Browsers` → `Refresh`.
3. BrowserSelect’i tamamen kapat ve yeniden aç.

- [ ] Özel ikon `Refresh` sonrasında da duruyor.
- [ ] Yeniden açınca da duruyor.
- [ ] (İsteğe bağlı) `Options` → `Export...` → JSON’da `"browserOverrides"` altında Tarayıcı A için `"icon"` alanı var.

### Test 10F – Varsayılana dönüş

1. Tarayıcı A → `Edit...` → **`Default`** → `OK`. Tarayıcı B için de aynısını yap.

- [ ] `Default` butonu sadece özel ikon varken aktif.
- [ ] Orijinal tarayıcı ikonları geri geldi.

### Test 10G – Portable tarayıcıda ikon (isteğe bağlı)

[08](08-portable-tarayici.md)’deki portable tarayıcıyı eklerken `Add...` penceresinde `Change...` ile `test-icon.png` seç.

- [ ] Portable tarayıcı ilk eklendiği anda özel ikonla görünüyor.

## Notlar

- İkon, ayarların içine PNG olarak gömülür; ikon dosyasını sonradan silmek ikonu bozmaz.
- Çok büyük resimler küçültülür; kare olmayan resimler en-boy oranı korunarak sığdırılır.
