# 12 – Türkçe dil (Settings → Language)

## Amaç

Arayüze **Türkçe** eklendi (`BrowserSelect/Localization/Strings.tr.resx`). Ayarlar penceresinin sol altındaki
**Language** açılır listesinde `Türkçe (TR)` seçilip BrowserSelect yeniden başlatılınca Ayarlar, tarayıcı
Ekle/Düzenle ve Hakkında pencereleri, sağdaki dikey `About` / `Settings` butonları ve mesaj kutuları Türkçe
görünür. Varsayılan dil İngilizce kalır; dil değiştirmek kuralları ve davranışı **değiştirmez**.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam, **v1.4.3.0 veya üstü** kurulu.
- Kurulum klasöründe (`%LOCALAPPDATA%\Programs\BrowserSelect` ya da kurduğun yer) `tr\BrowserSelect.resources.dll` var.

## Test linkleri

- https://example.com/?bs-test=lang
- https://example.org/?bs-test=lang

## Adımlar

### Test 12A – Dil listesi

1. BrowserSelect’i Başlat menüsünden aç → `Settings`.
2. Sol alttaki `Language:` listesini aç.

- [ ] Listede `English (EN)` ve `Türkçe (TR)` var.
- [ ] İlk kurulumda (veya dil hiç seçilmediyse) `English (EN)` seçili; arayüz İngilizce.

### Test 12B – Türkçeye geçiş

1. `Türkçe (TR)` seç.
2. Çıkan mesajı oku → `OK`.
3. Ayarları kapat, BrowserSelect’i kapat ve tekrar aç (`.\bs-open.ps1 "https://example.com/?bs-test=lang"`).

- [ ] Seçimden sonra “The new language will be used the next time BrowserSelect starts.” mesajı geldi.
- [ ] Yeniden açılışta seçim penceresinin sağındaki dikey butonlar `Hakkında` ve `Ayarlar`.
- [ ] `Ayarlar` penceresi Türkçe: `Tarayıcılar`, `Ekle...`, `Düzenle...`, `Kaldır`, `Sıra:`, `Varsayılan Tarayıcı`,
      `Varsayılan Yap`, `Dosya türü...`, `Seçenekler`, `Dışa aktar...`, `İçe aktar...`, `Güncelleme denetimi`,
      `Otomatik Seçim Kuralları`, sütunlar `Eşleşme / Desen / Tarayıcı / Gizli / Argümanlar`, butonlar
      `Yardım / Yukarı / Aşağı / Sil / Kapat / Uygula`, sol altta `Dil:`.
- [ ] Hiçbir yazı butondan/kutudan taşmıyor veya kesilmiyor (özellikle `Dosya türü...`, `denetle`, `Sıra:`).
- [ ] Butonların üzerine gelince çıkan ipuçları (tooltip) Türkçe.
- [ ] Kural tablosunun üstündeki açıklamada “projenin GitHub sayfasında” kısmı link; tıklayınca proje sayfası açılıyor.

### Test 12C – Türkçe mesajlar ve diyaloglar

1. Bir kuralın hücresini değiştir → alt sağdaki buton `İptal` olmalı → pencereyi başlıktaki X ile kapat.
2. `Hayır` → `Uygula`.
3. `Dışa aktar...` → kaydet; `İçe aktar...` → aynı dosyayı seç → `Evet`.
4. `Ekle...` → hiçbir şey doldurmadan `Tamam`.
5. `Güncelleme denetimi` → `denetle`.

- [ ] 1: “Kaydedilmemiş değişiklikler var…” sorusu Türkçe; değişiklik sonrası `Kapat` → `İptal`, `Uygula`’dan sonra tekrar `Kapat`.
- [ ] 3: Dosya diyaloğu başlığı/filtre Türkçe (“BrowserSelect ayarları (*.json)”); başarı mesajları Türkçe; kurallar korunmuş.
- [ ] 4: `Tarayıcı Ekle` penceresi Türkçe (`Ad:`, `Program:`, `Gözat...`, `Argümanlar:`, `Kısayol tuşları:`, `Simge:`…); “Lütfen tarayıcı için bir ad girin.” uyarısı geldi.
- [ ] 5: Sonuç mesajı Türkçe.

### Test 12D – Kurallar dilden etkilenmiyor

1. Türkçe arayüzde bir kural ekle: `Domain` | `example.org` | Tarayıcı B → `Uygula`.
2. `.\bs-open.ps1 "https://example.org/?bs-test=lang"`.
3. Dili `English (EN)` yap, BrowserSelect’i yeniden başlat, aynı linki tekrar aç.

- [ ] Her iki dilde de example.org doğrudan Tarayıcı B’de açıldı.
- [ ] `Domain`, `Manual` gibi eşleşme/sıralama türü adları iki dilde de aynı (bunlar kayıtlı değerlerdir, çevrilmez).
- [ ] İngilizceye dönünce tüm arayüz yeniden İngilizce.

## Notlar

- Yardım pencereleri (`Yardım` butonu) henüz İngilizce.
- Dil seçimi ayarlarda saklanır ve Export/Import’a dahildir.
