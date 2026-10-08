# 03 – Alt basılıyken kuralları atlama

## Amaç

Bir linke tıklarken **`Alt` tuşu basılıysa** BrowserSelect tüm otomatik kuralları yok sayar ve seçim
penceresini gösterir (tek seferlik elle seçim). Özellik `Settings` → `Options` →
**`Hold Alt on a link to skip rules`** kutusuyla açılıp kapatılır (varsayılan: açık).

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam.
- Şu kural tanımlı olsun: `Match` = `Domain`, `Pattern` = `example.com`, `Browser` = Tarayıcı A → `Apply`.
- Kontrol: `.\bs-open.ps1 "https://example.com/?bs-test=alt"` → seçim penceresi **açılmadan** Tarayıcı A’da açılıyor.

## Test linki

- https://example.com/?bs-test=alt

## Adımlar

### Test 3A – Alt ile kuralı atla (özellik açık)

1. `Settings` → `Options` → `Hold Alt on a link to skip rules` **işaretli** olsun → `Close`.
2. PowerShell’de (tools klasörü):

   ```powershell
   .\bs-open.ps1 "https://example.com/?bs-test=alt" -Delay 3
   ```

3. Geri sayım başlar başlamaz **`Alt` tuşuna bas ve basılı tut**; BrowserSelect penceresi görünene kadar bırakma.
4. Pencere açılınca `Alt`’ı **bırak**, sonra Tarayıcı B’ye tıkla.

### Test 3B – Alt olmadan (kural yine çalışıyor mu?)

1. Aynı komutu çalıştır, bu sefer hiçbir tuşa basma.

### Test 3C – Özellik kapalıyken

1. `Settings` → `Options` → `Hold Alt on a link to skip rules` işaretini **kaldır** → `Close`.
2. Test 3A’yı tekrarla (Alt basılı tut).
3. Testten sonra kutuyu tekrar **işaretle**.

### Test 3D – Gerçek hayat (isteğe bağlı, BrowserSelect varsayılan tarayıcı olmalı)

1. Outlook / Teams / Word gibi bir uygulamada `https://example.com/?bs-test=alt` linkini oluştur.
2. `Alt` basılı tutarak linke tıkla (Word’de `Ctrl + Alt + tık`). Pencere açılınca Alt’ı bırak.

## Beklenen sonuç (geçti ölçütü)

- [ ] 3A: Kural olmasına rağmen **seçim penceresi açıldı**; Tarayıcı B seçilince link Tarayıcı B’de açıldı.
- [ ] 3B: Pencere açılmadı, link doğrudan Tarayıcı A’da açıldı (kural bozulmadı).
- [ ] 3C: Özellik kapalıyken Alt basılı olsa bile link doğrudan Tarayıcı A’da açıldı.
- [ ] Kutunun durumu BrowserSelect kapatılıp açılınca korunuyor.
- [ ] 3D (yaptıysan): Alt + tık seçim penceresini gösterdi.

## Notlar

- **Alt’ı tarayıcıya tıklamadan önce bırak.** Seçim penceresinde `Shift` veya `Alt` basılıyken bir tarayıcıya
  tıklamak linki **gizli (private) pencerede** açar (eski özellik). Alt’ı bırakmazsan Tarayıcı B gizli modda açılır.
- Alt tuşu, BrowserSelect **başlarken** kontrol edilir; bu yüzden pencere açılana kadar basılı tutulmalı.
- Bazı uygulamalarda `Alt + tık` kendi kısayolunu tetikleyebilir (ör. menü çubuğu, sütun seçimi). Böyle bir
  uygulamada 3A’daki PowerShell yöntemi esas alınır.
