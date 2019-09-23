# puremvvm
Proje tanımı
Bir teknik destek departmanına müşterilerden gelen taleplerin takibi.
Senaryo: Müşteri kullandığı üründe bir problem olduğunda teknik destek deparmanına ticket açar.
Açılan ticketlar veritabanına “solved” alanı “false” olarak kaydedilir. Teknik destekdeki çalışanlar
müşterilerden gelen ticketları görüntüler çözdükleri problemleri çözüldü olarak veritabanında
güncellerler. (Ticket class’ı aşağıda belirtilmiştir.)
Kullanılacak teknolojiler :
Database : elasticsearch
Data Service : WCF (with authentication)
User interface : WPF (MVVM method and telerik or devexpress controls)
İzlenmesi gereken adımlar:
1. Elasticsearch veritabanının local development ortamına yüklenmesi. Gerekli index’in
(veritabanı) ve type’ın (ticket tablosu) oluşturulması.
2. Elasticsearch veritabanından verileri çekmesi ve UI’a iletmesi için gereken WCF
uygulamasının geliştirilmesi.
a. Uygulama geliştirilirken authentication özelliği eklenmelidir. WCF uygulamasını
kullanacak olan UI uygulaması ilk olarak bir login ekranında username ve password
sağlayarak WCF uygulamasını göndermeli ve gönderilen credentiallar doğru ise
uygulamaya giriş yapabilmelidir. (geçerli username ve password hard-coded olarak
WCF uygulamasının içerisinde tutulabilir)
b. WCF uygulamasına projeyi yapabilmek için gerekli olan fonksiyonların oluşturulması.
3. User-interface (veritabanı ile haberleşme tamamen WCF data-service aracılığı ile
gerçekleştirilmelidir.)
a. İlk olarak login ekranı getirilmesi ve WCF tarafında girilen username ve password’un
kontrol edilmesi.
Kullanıcı başarılı bir şekilde login olduktan sonra
b. Elasticsearch veritabanında bulunan ticketların WCF data-service’ı kullanılarak
çekilmesi ve bir DataGrid içerisinde gösterilmesi.
c. DataGrid üzerinde gösterilen kayıtlar filtrelenebilir, gruplanabilir ve sıralanabilir.
d. Kullanıcı bir ticket çözümlendiğinde ilgili satıra sağ click solved diyebilmesi ve bu
durumda ilgili ticket’ın elasticsearch veritabanında solved = true olarak
güncellenmesi.
e. Teknik destek elemanlarından en fazla ticket çözenlerin bir chart kullanılarak
gösterilmesi (TOP 5 SolvedBy)
f. En fazla sorun çıkan ürünlerin bir chart kullanılarak gösterilmesi (TOP 5
ProductName)
g. DataGrid üzerinde gösterilen kayıtlar ve Top chartları x saniyede bir async olarak
(responsive UI) güncellenmelidir.
Not:
Müşterilerden gelen talepleri elasticsearch veritabanına
• Manuel olarak
• Her hangi bir dosyadan okuyup bulk insert yaparak
• Ayrı bir UI projesi oluşturarak
kaydedebilirsiniz.

