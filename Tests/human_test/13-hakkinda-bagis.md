# 13 – Hakkında: snipeTR ve bitcoin bağış adresi

## Amaç

**Hakkında** (`About`) penceresine fork’un geliştiricisi **snipeTR** (fork linkiyle) ve bağış bölümüne snipeTR’ın
bitcoin adresi + QR kodu eklendi. Asıl geliştiricinin (Bor691) adresi de yerinde duruyor.

snipeTR bitcoin adresi: `bc1q3jqugh66ctwzqr7tqjafunlpaaejqgt265rwjq`
Bor691 bitcoin adresi: `1BA5Ndo24jtRgTEsvmGkrqRWTaJS4F3zNh`

## Ön koşullar

- **v1.4.3.0 veya üstü** kurulu.

## Test linkleri

- https://github.com/snipeTR/BrowserSelect
- https://github.com/zumoshi/BrowserSelect

## Adımlar

### Test 13A – Bilgiler

1. Bir linki aç (`.\bs-open.ps1 "https://example.com/?bs-test=about"`) → sağdaki dikey `About` butonu.

- [ ] Başlıkta `BrowserSelect v1.4.3` (veya üstü).
- [ ] “Coded By: Bor691”, e-posta ve zumoshi GitHub linki duruyor.
- [ ] Altında “This fork is maintained by: snipeTR” ve `https://github.com/snipeTR/BrowserSelect` linki var; tıklayınca fork sayfası açılıyor.

### Test 13B – Bağış bölümü

- [ ] Solda kalın “snipeTR (this fork) - Bitcoin:” başlığı, QR kodu, `Copy Address` butonu ve `bc1q3jq…rwjq` adresi var.
- [ ] Sağda “Bor691 (original author) - Bitcoin:” başlığı, eski QR, `Copy Address` ve `1BA5Ndo…F3zNh` adresi var.
- [ ] snipeTR QR kodu telefondaki bir cüzdan/QR okuyucu ile okununca `bitcoin:bc1q3jqugh66ctwzqr7tqjafunlpaaejqgt265rwjq` çıkıyor.

### Test 13C – Kopyalama

1. snipeTR tarafındaki `Copy Address` → Not Defteri’ne yapıştır.
2. Bor691 tarafındaki `Copy Address` → yapıştır.

- [ ] Buton yazısı ~1,5 sn `Copied!` oldu, sonra geri döndü.
- [ ] Yapıştırılan metinler birebir yukarıdaki adresler (boşluk/satır sonu yok).

### Test 13D – Adres linki

1. snipeTR adresine (mavi link) tıkla.

- [ ] Bitcoin cüzdanı kuruluysa ödeme ekranı bu adresle açıldı; kurulu değilse hata çıkmadı ve adres panoya kopyalandı (buton `Copied!` gösterdi).

### Test 13E – Türkçe

1. Dili `Türkçe (TR)` yap ([12](12-turkce-dil.md)), yeniden başlat, `Hakkında`’yı aç.

- [ ] Başlık “Browser Select: Hakkında”; “Geliştiren: Bor691”, “İletişim:”, “Bu fork'un geliştiricisi: snipeTR”, açıklama metni,
      “snipeTR (bu fork) - Bitcoin:”, “Bor691 (asıl geliştirici) - Bitcoin:”, `Adresi Kopyala`, `Kapat` Türkçe.
- [ ] Linkler etiketlerin hemen sağında, üst üste binmiyor.
