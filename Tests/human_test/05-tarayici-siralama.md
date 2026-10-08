# 05 – Tarayıcı listesini sıralama

## Amaç

`Settings` → `Browsers` bölümünde tarayıcıların seçim penceresindeki sırası değiştirilebilir:

- **`Sort:` = `Manual`** → `▲` / `▼` butonlarıyla elle sıralama
- **`Sort:` = `Alphabetical`** → isme göre A→Z
- **`Sort:` = `Most used`** → en çok kullanılan en solda

Sıralama `Refresh` (tarayıcıları yeniden tara) sonrasında da korunmalı.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam.
- En az **üç** görünen tarayıcı/profil olması idealdir (Edge + Chrome + Firefox, veya bir tarayıcının
  birkaç profili). İki tarayıcıyla da test edilebilir.
- `Show running browsers only` kapalı, kural listesi boş.

## Test linki

- https://example.com/?bs-test=sort

## Adımlar

### Test 5A – Manual (▲ / ▼)

1. BrowserSelect’i Başlat menüsünden aç, ikonların soldan sağa sırasını not et (ör. `1: Edge, 2: Chrome, 3: Firefox`).
2. `Settings` → `Browsers` → `Sort:` = `Manual`.
3. Listede **en alttaki** tarayıcıyı seç → `▲` butonuna basarak en üste taşı.
4. `Close` → ana pencereye bak.
5. BrowserSelect’i kapat (`Esc`) ve `.\bs-open.ps1 "https://example.com/?bs-test=sort"` ile tekrar aç.

- [ ] `▲` / `▼` butonları sadece `Manual` modunda aktif; en üstteki öğede `▲`, en alttakinde `▼` pasif.
- [ ] Taşıdığın tarayıcı ana pencerede **en solda** ve kısayolu `1` oldu.
- [ ] Yeniden açınca sıra korunuyor.

### Test 5B – Alphabetical

1. `Settings` → `Sort:` = `Alphabetical` → `Close`.

- [ ] Ana pencerede tarayıcılar ada göre alfabetik (büyük/küçük harf duyarsız) sıralandı.
- [ ] `▲` / `▼` butonları pasif.

### Test 5C – Most used

1. `Sort:` = `Manual`’a dön ve listenin **en sağındaki** tarayıcıyı (Tarayıcı Z diyelim) not et.
2. `.\bs-open.ps1 "https://example.com/?bs-test=sort"` → Tarayıcı Z’ye tıkla. Bunu **3 kez** tekrarla.
3. `Settings` → `Sort:` = `Most used` → `Close`.

- [ ] Tarayıcı Z, kendisinden **daha az** kullanılmış tüm tarayıcıların solunda (hiç kullanılmamışsa diğerleri
      arasında en solda).
- [ ] Aynı sayıda kullanılan tarayıcılar manuel sıralarını koruyor.
- [ ] Z’yi birkaç kez daha açınca (en çok kullanılan olduğunda) **en sola** geçiyor.

> Kullanım sayacı **kümülatiftir** ve kuralla otomatik açılışlarda da artar. 02 reçetesinde Tarayıcı A’yı
> çok açtıysan, Z’nin en sola geçmesi için A’dan daha fazla açman gerekir. Daha kolay test için 5C’yi
> önceki reçetelerde neredeyse hiç kullanmadığın bir tarayıcıyla yap.

### Test 5D – Refresh sonrası korunma

1. `Sort:` = `Manual`, 5A’daki gibi bir tarayıcının yerini değiştir.
2. `Browsers` bölümündeki **`Refresh`** butonuna bas.

- [ ] Elle verdiğin sıra kaybolmadı.
- [ ] Gizlediğin (işaretini kaldırdığın) tarayıcılar gizli kalmaya devam ediyor.

## Notlar

- Sıralama ayarı anında kaydedilir; `Apply` gerekmez (`Apply` sadece kurallar içindir).
- Numara kısayolları sadece ilk 9 tarayıcı için geçerlidir (bkz. [06](06-ozel-kisayollar.md)).
