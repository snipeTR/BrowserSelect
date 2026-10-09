# 19 – Kural ekleme `+` satırı ve `Delete` düzeltmesi

> Bu reçete v1.5.6.0 ve sonrası içindir.

## Amaç

- `Settings` → `Auto Select Filters` kural tablosunun en altında artık **otomatik boş “yeni satır” yok**.
- Son kuralın altında, `Match` sütununun solundaki dar sütunda (satır başlığı) **`+`** işareti olan bir satır var.
  Üzerine gelince imleç el olur, `+` vurgulanır ve ipucu çıkar: `Add a new rule` (Türkçe arayüzde `Yeni kural ekle`).
- `+`’ya tıklamak, `+` satırının **üstüne** yeni bir kural satırı ekler (`Match` = `Domain`, tarayıcı boş) ve
  `Pattern` hücresini yazmaya hazır hâle getirir.
- `+` satırı bir kural **değildir**: kaydedilmez, doğrulanmaz, dışa aktarılmaz, `Move Up` / `Move Down` ile
  taşınmaz, `Delete` ile silinmez ve hücreleri düzenlenemez.
- **Düzeltilen hata:** eskiden en alttaki boş satıra tıklayıp `Delete`’e (buton veya Del tuşu) basınca **bir üstteki
  kural** siliniyordu. Artık `Delete` yalnızca seçili kural(lar)ı siler; sadece `+` satırı seçiliyse hiçbir şey olmaz.

## Ön koşullar

- [00 – Hazırlık](00-hazirlik.md) tamam; `BrowserSelect-1.5.6.0-x64-Setup.exe` (veya daha yeni) kurulu.
- En az iki tarayıcı (aşağıda **Tarayıcı A** ve **Tarayıcı B**).
- Başlangıçta kural listesi boş (veya önce mevcut kuralları Export ile yedekle).

## Test linkleri

- https://example.com/?bs-test=plus-row
- https://example.org/?bs-test=plus-row
- https://example.net/?bs-test=plus-row

## Adımlar

### Test 19A – Otomatik boş satır yok, `+` satırı var

1. BrowserSelect’i Başlat menüsünden aç → `Settings`.

- [ ] Kural listesi boşken tabloda yalnızca **tek bir satır** var ve onun sol sütununda `+` görünüyor; hücreleri boş
      (`Domain` yazısı, onay kutusu veya açılır liste oku **görünmüyor**).
- [ ] Fareyle `+`’nın üzerine gelince imleç el oluyor, `+` hücresi vurgulanıyor ve ipucu `Add a new rule` çıkıyor.
- [ ] `+` satırındaki hücrelere tıklamak/yazı yazmaya çalışmak hücreyi düzenlemeye açmıyor (satırın herhangi bir
      yerine tıklamak da yeni kural ekler).

### Test 19B – `+` ile kural ekleme

1. `+`’ya tıkla.
2. Açılan `Pattern` hücresine `example.com` yaz, `Browser` hücresinden Tarayıcı A’yı seç.
3. Tekrar `+`’ya tıkla → `example.org`, Tarayıcı B.
4. Tekrar `+`’ya tıkla → `Match` = `Keyword`, `Pattern` = `plus-row-test`, Tarayıcı A.

- [ ] Her tıklamada yeni satır `+` satırının **üstüne** eklendi; `+` satırı hep en altta kaldı.
- [ ] Yeni satırda `Match` = `Domain`, `Browser` boş, `Private` işaretsiz; imleç doğrudan `Pattern` hücresinde
      (yazmaya başlayınca yazı hücreye giriyor).
- [ ] İlk `+` tıklamasıyla `Apply` aktif oldu ve `Close` butonu `Cancel`’a döndü.
- [ ] Hiçbir yerde kendiliğinden boş bir satır oluşmadı.

### Test 19C – `Apply` ve kalıcılık

1. `Apply` → `Close` → `Settings`’i tekrar aç.
2. `.\bs-open.ps1 "https://example.com/?bs-test=plus-row"` ve `.\bs-open.ps1 "https://example.org/?bs-test=plus-row"`.

- [ ] Üç kural aynı sırayla geri geldi, en altta yine yalnızca `+` satırı var (boş bir “kural” kaydedilmemiş).
- [ ] example.com doğrudan Tarayıcı A’da, example.org doğrudan Tarayıcı B’de açıldı.
- [ ] `+` ile ekleyip hiçbir şey yazmadan bırakılan boş satır `Apply` sırasında hata mesajı vermeden yok sayılıyor.
- [ ] Sadece `Pattern` yazılıp tarayıcı seçilmeyen satırda `Apply` eskisi gibi “tarayıcı yok” uyarısı veriyor; sadece
      tarayıcı seçilip desen boş bırakılan satırda “desen boş” uyarısı geliyor. `+` satırı için **asla** uyarı çıkmıyor.

### Test 19D – `Delete` yalnızca seçili kuralı siler (hatanın düzeltmesi)

1. **Son kuralın** (`plus-row-test`) herhangi bir hücresine tıkla → `Delete` butonu.
2. `Cancel` → `Settings`’i tekrar aç (silme geri alındı).
3. `+` satırını fareyle değil klavyeyle seç (fareyle tıklamak yeni kural ekler): son kurala tıkla, `↓` (aşağı ok)
   ile `+` satırına in → `Delete` butonuna bas; sonra tekrar `+` satırına in ve klavyede `Del` tuşuna bas.
4. Ortadaki kuralın (`example.org`) satır başlığına tıklayıp tüm satırı seç → klavyede `Del`.
5. `Ctrl` basılıyken iki kuralın satır başlığına tıkla → `Delete`.

- [ ] 1. adımda yalnızca `plus-row-test` silindi; üstündeki `example.org` yerinde duruyor, seçim bir üstteki kurala geçti.
- [ ] 3. adımda (`+` satırı seçiliyken) `Delete` butonu ve `Del` tuşu **hiçbir şey silmedi**, hata vermedi,
      `Apply` aktif olmadı (önceden kaydedilmemiş değişiklik yoksa).
- [ ] 4. adımda yalnızca `example.org` silindi.
- [ ] 5. adımda tam olarak seçili iki kural silindi, başka kural silinmedi; `+` satırı hep yerinde.
- [ ] Hücre düzenlenirken (`Pattern` hücresinde yazı imleci varken) `Del` sadece yazıdaki harfi siliyor, kuralı değil.
- [ ] Tüm kurallar silinince tabloda yalnızca `+` satırı kalıyor; `Delete` yine hiçbir şey yapmıyor.

### Test 19E – `Move Up` / `Move Down`

1. `+` ile üç kural ekle (A, B, C sırasıyla, her birine desen + tarayıcı ver) → `Apply`.
2. C’yi seç → `Move Down`.
3. C’yi seç → `Move Up` iki kez (sıra C, A, B olur). B’yi (artık en altta) seç → `Move Down`.
4. `↓` ile `+` satırına in → `Move Up`.

- [ ] 2. adımda C yerinde kaldı (`+` satırının altına inmedi).
- [ ] 3. adımda sıra C, A, B oldu; en alttaki B’de `Move Down` hiçbir şey yapmadı; `+` hep en altta.
- [ ] 4. adımda `+` satırı yukarı taşınmadı, hiçbir kural yer değiştirmedi.
- [ ] Taşımadan sonra `+` satırının hücreleri hâlâ boş ve düzenlenemiyor.

### Test 19F – Export / Import

1. Birkaç kural varken `Export` → `BrowserSelect-settings.json` olarak kaydet. Kaydedilmemiş değişiklik varken
   `Export` sorusuna `Yes` de.
2. Dosyayı Not Defteri’nde aç.
3. `Import` → aynı dosyayı veya [files/sample-settings.json](files/sample-settings.json)’u seç → `Yes`.

- [ ] JSON’daki `rules` listesinde yalnızca gerçek kurallar var; boş desenli/boş tarayıcılı bir kayıt **yok**.
- [ ] Import’tan sonra kurallar listede, en altta tek bir `+` satırı var (iki `+` satırı veya boş satır yok).
- [ ] Import’tan sonra `+` ile kural eklemek ve `Delete` ile silmek normal çalışıyor.

### Test 19G – Ana pencereden `Always` kuralı

1. Ayarları kapat. `.\bs-open.ps1 "https://example.net/?bs-test=plus-row"` → seçim penceresinde bir tarayıcının
   `Always` butonuna bas (gerekirse alan adı seçeneğini seç).
2. Aynı linki tekrar aç; sonra `Settings`’i aç.

- [ ] Link ikinci seferde doğrudan seçilen tarayıcıda açıldı.
- [ ] Yeni kural listenin sonunda, `+` satırının **üstünde** görünüyor.

### Test 19H – Koyu tema

1. `Settings` → `Theme` = `Dark`.

- [ ] `+` işareti koyu arka planda açık renkte, net görünüyor; `+` satırının hücreleri tablonun koyu zemin renginde
      (beyaz kutu veya açık renkli şerit yok).
- [ ] Üzerine gelince `+` hücresi koyu temanın seçim rengiyle vurgulanıyor, `+` hâlâ okunuyor.
- [ ] `Light`’a dönünce `+` koyu renkte, açık zeminde net görünüyor.

### Test 19I – %150 ekran ölçeği

1. Windows Ayarlar → Ekran → Ölçek `%150` → oturumu kapatıp aç (veya BrowserSelect’i yeniden başlat).
2. `Settings`’i aç; mümkünse `%125`, `%175`, `%200` ile de tekrarla.

- [ ] `+` sol sütunda ortalanmış, kenarlara taşmıyor, çizgileri bulanık veya kırık değil; ölçekle orantılı büyümüş.
- [ ] `+` ile ekleme, `Delete` ile silme ve ipucu %150’de de yukarıdaki gibi çalışıyor.

## Notlar

- `+` satırı tıklandığında satırın tamamı “yeni kural ekle” düğmesi gibi davranır; klavyeyle `+` satırındayken
  `Space` veya `Insert` tuşu da yeni kural ekler.
- Yanlışlıkla eklenen boş bir kural `Apply`’da sessizce yok sayılır; istersen seçip `Delete` ile silebilirsin.
- Hata bulursan reçete numarası (19), adım ve ekran ölçeği / tema bilgisiyle bildir.
