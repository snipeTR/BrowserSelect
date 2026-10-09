# 18 – 12 arayüz dili, Japonca/Çince yazı tipi, taşma ve yardım pencereleri

> Bu reçete v1.5.5.0 ve sonrası içindir.

## Amaç

Arayüz artık 12 dilde: **English, Türkçe, Русский, Українська, Deutsch, Français, Español, Português (Brasil),
Italiano, Polski, 简体中文, 日本語**. Bu reçetede her dil tek tek seçilir, BrowserSelect yeniden başlatılır ve:

- metinlerin o dilde göründüğü,
- Japonca ve Çincede doğru Windows yazı tipinin kullanıldığı (kare kutu / "tofu" yok, bulanık değil, 9 pt'den küçük değil),
- dar butonlarda ve etiketlerde yazıların **kesilmediği** (son harf, "..." veya iki nokta görünmeli),
- yardım pencerelerinin (**?** ve Ayarlar → Yardım) o dilde açıldığı ve kaydırılabildiği

kontrol edilir. Çeviriler Google Translate ve Bing Translator ile geri çeviri yapılarak kontrol edildi; yine de
anlamı garip gelen bir ifade görürsen Not sütununa yaz.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam; `BrowserSelect-1.5.5.0-x64-Setup.exe` (veya daha yeni) kurulu.
- Kurulum klasöründe şu alt klasörlerin her birinde `BrowserSelect.resources.dll` var:
  `tr`, `ru`, `uk`, `de`, `fr`, `es`, `pt-BR`, `it`, `pl`, `zh-Hans`, `ja`.
- Windows 10 veya 11 (Japonca için **Yu Gothic UI** ya da **Meiryo UI**, Çince için **Microsoft YaHei UI**
  Windows'ta varsayılan olarak yüklüdür; ek dil paketi gerekmez).
- İsteğe bağlı: 125 % ve 150 % ekran ölçeğinde de tekrarlamak (Windows Ayarlar → Ekran → Ölçek).

## Dil değiştirme (her dil için aynı)

1. Sağdaki dikey **Settings** butonuna tıkla (dile göre adı değişir, tabloya bak).
2. Sol alttaki dil listesinden (`Language:` / `Dil:` / ...) sıradaki dili seç → yeniden başlatma uyarısını onayla.
3. Ayarlar penceresini kapat, BrowserSelect'i tamamen kapat ve Başlat menüsünden **yeniden aç**
   (ya da `tools\bs-open.ps1 https://example.com/?bs-test=lang18` ile bir link aç).

| Dil | Listede görünen ad | Dil etiketi | Settings | Apply | Always | Help | About |
|---|---|---|---|---|---|---|---|
| İngilizce | English (EN) | Language: | Settings | Apply | Always | Help | About |
| Türkçe | Türkçe (TR) | Dil: | Ayarlar | Uygula | Her zaman | Yardım | Hakkında |
| Rusça | Русский (RU) | Язык: | Настройки | Применить | Всегда | Справка | Сведения |
| Ukraynaca | Українська (UK) | Мова: | Параметри | Застосувати | Завжди | Довідка | Відомості |
| Almanca | Deutsch (DE) | Sprache: | Einstellungen | Anwenden | Immer | Hilfe | Info |
| Fransızca | Français (FR) | Langue : | Paramètres | Appliquer | Toujours | Aide | À propos |
| İspanyolca | Español (ES) | Idioma: | Ajustes | Aplicar | Siempre | Ayuda | Acerca de |
| Portekizce (Brezilya) | Português (Brasil) (PT-BR) | Idioma: | Configurar | Aplicar | Sempre | Ajuda | Sobre |
| İtalyanca | Italiano (IT) | Lingua: | Impostazioni | Applica | Sempre | Aiuto | Info |
| Lehçe | Polski (PL) | Język: | Ustawienia | Zastosuj | Zawsze | Pomoc | O programie |
| Basitleştirilmiş Çince | 简体中文 (ZH-HANS) | 语言： | 设置 | 应用 | 始终 | 帮助 | 关于 |
| Japonca | 日本語 (JA) | 言語： | 設定 | 適用 | 常に使う | ヘルプ | 情報 |

> Bir dilde kaybolursan: Ayarlar penceresinde sol alttaki **ilk açılır liste** her zaman dil listesidir;
> `English (EN)` seç ve yeniden başlat.

## Test linkleri

- https://example.com/?bs-test=lang18
- https://example.org/?bs-test=lang18

## Adımlar

### Test 18A – Dil listesi

1. `Settings` → sol alttaki dil listesini aç.

- [ ] Listede 12 dil var, adları tablodaki gibi **kendi dillerinde** yazıyor (ör. `日本語 (JA)`, `简体中文 (ZH-HANS)`).
- [ ] Arapça / Farsça listede **yok** (sağdan sola diller sadece kurulumda var).

### Test 18B – Her dil için (12 kez tekrarla)

Dili değiştir ve yeniden başlat (yukarıdaki 3 adım), sonra:

1. Bir test linki aç → seçim penceresi.
   - [ ] Sağdaki dikey `About` / `Settings` butonları seçilen dilde, yazılar kesik değil.
   - [ ] Her tarayıcının altındaki `Always` linki seçilen dilde ve tamamen görünüyor.
2. `Settings` penceresini aç.
   - [ ] Butonlar (Add / Edit / Remove, Move Up / Move Down / Delete, Apply, Help, Refresh, Export / Import,
     File types, Set as default, Check now) ve etiketler seçilen dilde; **hiçbir yazının sonu kesilmemiş**.
     Dar butonlarda yazı küçük puntoyla sığdırılabilir, bu normaldir (Japonca/Çincede küçültme yok, bkz. 18C).
   - [ ] `Options` grubundaki onay kutuları (çalışan tarayıcılar, Alt, tam ekran pencereler) tek satırda ve okunuyor.
   - [ ] Sol alttaki dil / tema / sıralama listeleri etiketlerinin üstüne binmiyor (uzun etiketlerde liste sağa kayar).
   - [ ] Kural tablosunda eşleşme türleri (`Domain`, `URL`, ...) ve `display BrowserSelect` / `ignore URL (do nothing)`
     **İngilizce kalıyor** (bunlar kayıtlı değerlerdir, bilerek çevrilmedi).
3. Bir tarayıcı seç → `Edit...` → tarayıcı düzenleme penceresi.
   - [ ] Etiketler (Ad, Program, Argümanlar, Kısayol, Simge) ve butonlar seçilen dilde, kesik değil.
4. `About` penceresi → `Original project` butonu.
   - [ ] İki pencere de seçilen dilde; linkler (snipeTR ve zumoshi) tıklanabiliyor.
5. Seçim penceresindeki **?** ve Ayarlar → `Help` → yardım pencereleri.
   - [ ] Metin seçilen dilde, kaydırılabiliyor, `Close` butonu metnin altında görünüyor.
   - [ ] Ana yardımda "Dil" satırında 12 dilin listesi var.
   - [ ] Kural yardımındaki "Tam ekran pencereleri atla" seçeneğinin adı, Ayarlar → Options'taki onay kutusunun
     adıyla **aynı**.
6. Ayarlar → kural listesinin üstündeki yardım linki.
   - [ ] Tarayıcıda `github.com/snipeTR/BrowserSelect/blob/master/help/filters.md` açılıyor
     (Türkçede `filters.tr.md`); `zumoshi` adresi **açılmıyor**.
7. Bir mesaj kutusu tetikle (ör. bir kuralı değiştir, `Apply`'a basmadan `Export...` → kaydedilmemiş değişiklik sorusu).
   - [ ] Soru ve butonlar anlamlı; Hayır'ın ne yaptığı açık ("son uygulanan kurallar dışa aktarılır").

### Test 18C – Japonca ve Çince yazı tipi

`日本語 (JA)` ve `简体中文 (ZH-HANS)` için 18B'ye ek olarak:

- [ ] Hiçbir yerde kare kutu (□) veya soru işareti yok.
- [ ] Japonca yazı tipi Yu Gothic UI (yoksa Meiryo UI), Çince Microsoft YaHei UI gibi görünüyor; MS UI Gothic /
  SimSun gibi eski, tırtıklı bir yazı tipi değil.
- [ ] Yazılar diğer dillerden küçük değil (en az 9 pt); dar butonlarda küçülmek yerine etiket boş alana doğru genişliyor.
- [ ] İtalik yazı yok (ipucu/not metinleri gerekirse kalın görünür).
- [ ] Kısayol satırındaki Latin harfli tuşlar (ör. `1`, `g`, `c`) normal Segoe UI ile görünüyor.
- [ ] Yardım pencerelerinde uzun Japonca/Çince satırlar pencere kenarında düzgün kırılıyor, taşmıyor.
- [ ] Japonca `About` → `Original project`: "開発者：Bor691" satırı tam görünüyor.

### Test 18D – Geri dönüş

1. Dili `Türkçe (TR)` (veya `English (EN)`) yap, yeniden başlat.

- [ ] Arayüz eski diline döndü; kurallar, tarayıcı listesi ve ayarlar dil değişikliklerinden **etkilenmedi**.

## Beklenen sonuç

12 dilin hepsinde arayüz ve yardım seçilen dilde, hiçbir yazı kesik değil, Japonca/Çince doğru yazı tipiyle
okunuyor ve kural/ayar davranışı dil değişikliğinden etkilenmiyor.

## Dikkat

- Taşma/kesilme bulursan dili, pencereyi, butonun adını ve ekran ölçeğini (100 % / 125 % / 150 % ...) not et;
  mümkünse ekran görüntüsü ekle.
- Dil değişikliği **yeniden başlatınca** uygulanır; Ayarlar penceresinde hemen değişmemesi normaldir.
