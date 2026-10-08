# 11 – Kural silme butonu (`Delete`)

## Amaç

`Settings` → `Auto Select Filters` bölümünde **`Move Down` butonunun hemen sağına** bir **`Delete`** butonu eklendi.
Seçili kuralı listeden siler ve değişikliği kaydetmek için `Apply`’ı aktif eder (onay sorusu yok;
`Apply`’a basılmadıkça kalıcı değildir).

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam.
- Kural listesi boş.

## Test linkleri

- https://example.com/?bs-test=delete
- https://example.org/?bs-test=delete

## Adımlar

### Test 11A – Butonun yeri

1. BrowserSelect’i Başlat menüsünden aç → `Settings`.

- [ ] Kural tablosunun altında sırasıyla `Move Up`, `Move Down`, **`Delete`** butonları var; `Delete` Move Down’un sağında.
- [ ] Ayarlar penceresi kenarından sürüklenerek büyütülüp küçültülünce butonlar üst üste binmiyor, `Delete` Move Down’un sağında kalıyor.

### Test 11B – Silme ve Apply

1. Üç kural ekle → `Apply`:
   1. `Domain` | `example.com` | Tarayıcı A
   2. `Domain` | `example.org` | Tarayıcı B
   3. `Keyword` | `delete-test` | Tarayıcı A
2. **2. kuralın** (`example.org`) herhangi bir hücresine tıkla → **`Delete`**.
3. Tabloya ve butonlara bak.
4. `Apply` → `Close` → BrowserSelect’i kapat.
5. `.\bs-open.ps1 "https://example.org/?bs-test=delete"` ve `.\bs-open.ps1 "https://example.com/?bs-test=delete"`.

- [ ] `example.org` kuralı listeden kalktı, diğer iki kural yerinde ve sıraları korunmuş.
- [ ] Silmeden sonra seçim, aynı konumdaki bir sonraki kurala (`delete-test`) geçti.
- [ ] `Apply` aktif oldu ve `Close` butonu `Cancel`’a döndü; `Apply`’dan sonra tekrar pasif / `Close` oldu.
- [ ] example.org artık seçim penceresini açıyor (kural silindi), example.com hâlâ doğrudan Tarayıcı A’da.
- [ ] `Settings` tekrar açıldığında silinen kural geri gelmiyor.

### Test 11C – Apply’sız kapatma (geri alma)

1. Bir kuralı seç → `Delete` → **`Apply`’a basmadan** `Cancel`’a bas.
   (Başlık çubuğundaki X ile kapatırsan `You have unsaved changes...` sorusu gelir → `Yes`.)
2. `Settings`’i tekrar aç.

- [ ] Silinen kural geri gelmiş (kaydedilmemiş silme kalıcı olmadı).

### Test 11D – Son kural ve boş liste

1. Listedeki **son** kuralı seç → `Delete` → seçim bir üstteki kurala geçmeli.
2. Kalan tüm kuralları tek tek `Delete` ile sil → `Apply`.
3. Liste boşken (sadece en alttaki boş “yeni satır” varken) `Delete`’e bas.

- [ ] Son kural silinince seçim bir öncekine geçti.
- [ ] Tüm kurallar silinip `Apply` sonrası liste boş kaydedildi.
- [ ] Boş listede veya boş “yeni satır” seçiliyken `Delete` hata vermedi, hiçbir şey olmadı.

### Test 11E – Move Up / Move Down ile birlikte

1. Üç kural ekle, `Apply`.
2. Üçüncüyü `Move Up` ile en üste taşı → onu `Delete` ile sil → `Apply`.

- [ ] Taşınan doğru kural silindi, diğer ikisinin sırası doğru.

## Notlar

- Klavyedeki `Delete` tuşu (satır başlığına tıklayıp tüm satırı seçince) da eskisi gibi satırı siler; buton bunun
  daha görünür hâlidir.
- Silme işlemi onay sormaz; yanlışlıkla silersen `Apply`’a basmadan `Cancel` ile geri alabilirsin.
