# 06 – Özel kısayol tuşları

## Amaç

Seçim penceresinde bir tarayıcı; sırasındaki numara (1–9) veya adının kelimelerinin ilk harfleriyle seçilebiliyordu.
Artık her tarayıcıya **özel kısayol tuş(lar)ı** atanabilir: `Settings` → `Browsers` → tarayıcıyı seç →
**`Edit...`** → **`Shortcut keys:`**.

- Özel kısayol, otomatik harf kısayollarının **yerine geçer** ve diğer tarayıcıların otomatik kısayollarından **önceliklidir**.
- Büyük/küçük harf fark etmez.
- Numara kısayolları sadece ilk **9** tarayıcıda gösterilir/çalışır.
- İkonun altındaki parantez yalnızca **gerçekten çalışan** kısayolları gösterir, ör. `( 1,w )`.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam, en az iki tarayıcı.
- Kural listesi boş.

## Test linki

- https://example.com/?bs-test=shortcut

## Adımlar

### Test 6A – Özel kısayol ata

1. BrowserSelect’i aç; ikonların altındaki parantezleri not et (ör. Chrome: `( 2,g,c )`).
2. `Settings` → `Browsers` → **Tarayıcı A**’yı seç → `Edit...`.
3. `Shortcut keys:` alanına `w` yaz → `OK` → `Close`.
4. Ana pencerede Tarayıcı A’nın altındaki parantezi kontrol et.
5. `Esc` ile kapat, `.\bs-open.ps1 "https://example.com/?bs-test=shortcut"` ile aç, klavyeden **`w`**’ya bas.

- [ ] Tarayıcı A’nın altında `( <numara>,w )` görünüyor; eski otomatik harfleri (ör. `g,c`) artık yok.
- [ ] `w`’ya basınca link Tarayıcı A’da açıldı.

### Test 6B – Büyük harf ve Shift

1. Tekrar aç ve **`Caps Lock` açıkken** `W`’ya bas → Tarayıcı A’da açılmalı.
2. Tekrar aç ve `Shift + w`’ya bas.

- [ ] `W` (büyük harf) de çalıştı.
- [ ] `Shift + w` linki Tarayıcı A’nın **gizli** penceresinde açtı (Shift = private, eski özellik).

### Test 6C – Öncelik (çakışan harf)

1. Tarayıcı B’nin otomatik harfini öğren (ör. `Microsoft Edge` → `m,e`).
2. Tarayıcı A’nın `Shortcut keys:` değerini bu harf yap (ör. `e`) → `OK` → `Close`.
3. Linki aç, `e`’ye bas.

- [ ] Link **Tarayıcı A**’da açıldı (özel kısayol otomatik olana üstün geldi).
- [ ] Tarayıcı B’ye hâlâ diğer harfi (ör. `m`) veya numarasıyla ulaşılabiliyor.

### Test 6D – Birden fazla tuş

1. Tarayıcı A için `Shortcut keys:` = `wq` (veya `w,q`) → `OK`.

- [ ] Parantezde `w,q` görünüyor; hem `w` hem `q` Tarayıcı A’yı açıyor.

### Test 6E – Varsayılana dönüş

1. `Edit...` → `Shortcut keys:` alanını **boşalt** → `OK`.

- [ ] Otomatik kısayollar (adın baş harfleri) geri geldi.

### Test 6F – 9’dan fazla tarayıcı (isteğe bağlı)

10+ görünen tarayıcı/profil varsa: 10. ve sonraki ikonların altında numara **olmamalı**; `0` tuşu hiçbir şey açmamalı.

## Notlar

- `Edit...` penceresinde ipucu: `e.g. w (leave empty for automatic)`.
- Kısayollar seçim penceresi odaktayken çalışır; `Esc` linki açmadan kapatır.
