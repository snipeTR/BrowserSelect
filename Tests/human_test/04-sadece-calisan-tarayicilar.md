# 04 – Sadece çalışan tarayıcıları gösterme

## Amaç

`Settings` → `Options` → **`Show running browsers only`** açıkken seçim penceresinde yalnızca **o an çalışan**
tarayıcılar listelenir. Hiçbir tarayıcı çalışmıyorsa liste boş kalmasın diye **tüm** tarayıcılar gösterilir.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam.
- En az iki tarayıcı (ör. Chrome + Edge, veya Firefox + Edge).
- Kural listesi boş (ya da test linkiyle eşleşen kural yok).

## Test linki

- https://example.com/?bs-test=running

## Adımlar

1. `Settings` → `Options` → `Show running browsers only` **işaretle** → `Close`.
2. **Tüm tarayıcıları kapat.** Arka planda çalışanları da kapatmak için Görev Yöneticisi’nde (`Ctrl+Shift+Esc`)
   `chrome.exe`, `msedge.exe`, `firefox.exe` süreçlerinin olmadığını kontrol et (bkz. Notlar).
3. **Test 4A:** `.\bs-open.ps1 "https://example.com/?bs-test=running"` → listeye bak → `Esc`.
4. Sadece **Tarayıcı A**’yı aç (ör. Chrome), açık bırak.
5. **Test 4B:** Aynı komutu çalıştır → listeye bak → `Esc`.
6. Tarayıcı B’yi de aç.
7. **Test 4C:** Aynı komutu çalıştır → listeye bak → `Esc`.
8. **Test 4D:** `Settings` → `Show running browsers only` işaretini **kaldır** → `Close` → komutu tekrar çalıştır.

## Beklenen sonuç (geçti ölçütü)

- [ ] 4A (hiç tarayıcı çalışmıyor): **Tüm** tarayıcılar listelendi (boş pencere yok).
- [ ] 4B (sadece A çalışıyor): Listede **yalnız Tarayıcı A** (ve varsa onun profilleri) var.
- [ ] 4C (A ve B çalışıyor): İkisi de listede, çalışmayan diğerleri yok.
- [ ] 4D (özellik kapalı): Gizlenmemiş tüm tarayıcılar listede.
- [ ] Kısayol numaraları (1, 2, …) gösterilen listeye göre yeniden sıralanıyor.
- [ ] Ayarı değiştirince açık olan ana pencere hemen güncelleniyor (Settings’i ana pencereden açtıysan).

## Notlar

- **Edge “Başlangıç hızlandırma” (Startup boost)** ve Chrome’un **“Uygulamalar kapalıyken arka planda çalışmaya devam et”**
  seçeneği tarayıcıyı pencere olmadan çalışır tutar; BrowserSelect bunu “çalışıyor” sayar. Test 4A/4B’de bu
  tarayıcı listede görünürse hata değildir: Görev Yöneticisi’nde süreci sonlandır veya
  `edge://settings/system` / `chrome://settings/system` sayfasından bu seçeneği kapat.
- Aynı tarayıcının profilleri (ör. `Google Chrome (Work)`) aynı `.exe`’yi kullandığından, tarayıcı çalışıyorsa
  tüm profilleri gösterilir.
- Testten sonra kutuyu tercihine göre bırak (varsayılan: kapalı).
