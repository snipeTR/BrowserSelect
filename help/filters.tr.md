# Otomatik seçim kuralları (filtreler)

🇬🇧 [English](filters.md)

**Ayarlar → Otomatik Seçim Kuralları** bölümünden kurallar ekleyerek BrowserSelect'in tarayıcı listesini
göstermek yerine linki otomatik olarak bir tarayıcıda açmasını sağlayabilirsiniz.

**Kural ekleme ve silme:** kural eklemek için son kuralın altındaki satırda, sol sütundaki **+** işaretine tıklayın
(Eşleşme = `Domain` olan yeni bir satır eklenir ve Desen hücresine hemen yazabilirsiniz). Kural silmek için kuralları
seçip **Sil**'e tıklayın (veya **Del** tuşuna basın); yalnızca seçili kurallar silinir, **+** satırı bir kural değildir ve
silinemez. **Uygula**'ya basana kadar hiçbir şey kaydedilmez.

Her kuralın beş sütunu vardır:

**Eşleşme (Match)**: desenin linkin hangi kısmıyla karşılaştırılacağı:

| Eşleşme | Desen neyle karşılaştırılır | Örnekler |
|---|---|---|
| `Domain` (varsayılan) | alan adı (host) | `google.com`, `*.google.com`, `website.*`, `*` |
| `URL` | linkin tamamı, `*` ve `?` joker karakterleriyle (`scheme://` kısmı isteğe bağlı) | `github.com/myorg/*`, `https://*/login*` |
| `Path` | linkin yolu (ve sorgu kısmı); başta `/` yoksa eklenir | `/watch*`, `/docs/*` |
| `Keyword` | virgül (veya `;`) ile ayrılmış kelimeler; link bunlardan birini içeriyorsa eşleşir | `zoom, meet, webex` |
| `Extension` | linkin veya açılan dosyanın uzantısı (virgül ya da boşlukla ayrılır) | `pdf, zip`, `html` |
| `Source App` | linke tıklanan uygulama (`.exe` ile ya da `.exe` olmadan dosya adı veya tam yol; joker karakter kullanılabilir) | `outlook.exe`, `slack`, `teams*` |
| `Regex` | linkin tamamıyla karşılaştırılan .NET düzenli ifadesi (büyük/küçük harf duyarsız) | `^https://(www\.)?example\.(com\|org)/` |

Tüm karşılaştırmalar büyük/küçük harfe duyarsızdır. Eşleşme türlerinin adları programda İngilizce görünür.

**Desen (Pattern)**: kuralın hangi linklerle eşleşeceği. `Domain` kurallarında şunları kullanabilirsiniz:
- bir alan adı (yolu olmayan bir URL, ör. `google.com , www.mywebsite.org`)
- joker karakterli bir desen ( `*.local , *.us , *.google.com , website.*` )
- her şeyle eşleşen yıldız ( `*` )

**Tarayıcı (Browser)**: desen eşleşince ne olacağı:
- linkin otomatik olarak açılacağı bir tarayıcı (veya tarayıcı profili),
- `display BrowserSelect`: bu desen için her zaman tarayıcı listesini göster (daha geniş bir kuralın dışında bırakır),
- `ignore URL (do nothing)`: linki hiç açma.

**Gizli (Private)**: linki gizli/incognito pencerede aç.

**Argümanlar (Arguments)**: tarayıcıya verilecek ek komut satırı parametreleri, ör. `--incognito`,
`--disable-web-security --user-data-dir="C:\temp\chrome"` veya Firefox profili için `-P work`.

Gizli kurallar ve yeni pencere açan parametreler (`--new-window`, `--incognito`, `-private-window`, `--app=...`,
`--kiosk`, ...) *Ayarlar → Seçenekler → Tam ekran pencereleri atla* adımını atlar: link zaten yeni pencerede açıldığı
için mevcut bir tarayıcı penceresi öne getirilmez.

Sıra ve öncelik
---

- Önce tam desenler, sonra joker karakterli desenler, en son da her şeyi yakalayan `*` (veya regex `.*`) denetlenir.
- Aynı öncelikteki kurallar yukarıdan aşağıya denetlenir; sırayı **Yukarı** / **Aşağı** ile değiştirin.
- İlk eşleşen kural uygulanır. Hiçbir kural eşleşmezse tarayıcı listesi gösterilir.
- Geçersiz bir kural (ör. hatalı bir düzenli ifade) linkin açılmasını asla engellemez; sadece atlanır.

Kuralları düzenleme
---

- **Sil** düğmesi (Aşağı düğmesinin sağında) veya <kbd>Del</kbd> tuşu seçili kuralı siler.
- Değişiklikler ancak **Uygula**'ya basınca kaydedilir.
- Ayarlar penceresi geçerli linki hangi uygulamanın açtığını gösterir ("Linki açan: ..."); `Source App`
  kurallarında bu adı kullanın.
- **Seçenekler → Dışa aktar... / İçe aktar...** kuralları (ve diğer ayarları) bir `.json` dosyasına kaydeder ve geri yükler.

*Not*: Filtreleri değiştirmek için BrowserSelect'i Başlat menüsünden elle açabilirsiniz. Başlat menüsünden
açıldığında BrowserSelect, filtrelerden bağımsız olarak her zaman kendi penceresini açar.

*İpucu*: Bir linke tıklarken **Alt** tuşunu basılı tutarsanız tüm kurallar atlanır ve tarayıcıyı elle seçersiniz
(Ayarlar → Seçenekler → "Alt basılıyken kuralları atla" ile kapatılabilir).

*İpucu*: Tarayıcı listesindeki **Always** (Her zaman) düğmesi geçerli site için otomatik olarak bir `Domain` kuralı oluşturur.

Örnekler
---

1. tüm siteleri Firefox'ta, sadece companywebsite.com'u IE'de aç:
    - kural 1: `*` -> `Firefox`
    - kural 2: `*.companywebsite.com` -> `IE`
2. Google sitelerini (`mail.google.com, contacts.google.com, ...`) Chrome'da, GitHub'ı Firefox'ta aç, diğer siteler için sor:
    - kural 1: `*.google.com` -> `Chrome`
    - kural 2: `github.com` -> `Firefox`

    not: `*` -> `display BrowserSelect` şeklinde üçüncü bir kural ekleyebilirsiniz ama gerekmez; hiçbir kural
    eşleşmezse BrowserSelect zaten sorar.
3. `.us` alan adları için hangi tarayıcının kullanılacağını sor, diğer tüm siteleri Firefox'ta aç:
    - kural 1: `*.us` -> `display BrowserSelect`
    - kural 2: `*` -> `Firefox`
4. Outlook'ta tıklanan her linki Edge'de, her Zoom/Meet linkini Chrome'da aç:
    - kural 1: `Source App` `outlook.exe` -> `Edge`
    - kural 2: `Keyword` `zoom.us, meet.google.com` -> `Google Chrome`
5. sadece kurumunuzun GitHub depolarını özel parametrelerle yeni bir Chrome penceresinde aç:
    - kural 1: `URL` `github.com/myorg/*` -> `Google Chrome`, Argümanlar: `--new-window`
6. PDF linklerini Edge'de, YouTube videolarını gizli pencerede aç:
    - kural 1: `Extension` `pdf` -> `Edge`
    - kural 2: `Path` `/watch*` -> `Firefox`, Gizli işaretli
7. izleme (tracking) linklerini hiç açma:
    - kural 1: `Regex` `^https?://(www\.)?tracker\.example\.com/` -> `ignore URL (do nothing)`
