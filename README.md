# 🗂️ Personal Organizer – C# WinForms Application

Bu proje, C# dilinde ve WinForms mimarisiyle geliştirilmiş çok modüllü bir **kişisel organizatör uygulamasıdır**. Kullanıcı dostu arayüzüyle birden fazla kullanıcıya hizmet verebilir.

## 🎯 Proje Amacı

Bu uygulama; kullanıcıların:
- Kişisel bilgilerini yönetmesini,
- Not almasını,
- Telefon rehberi oluşturmasını,
- Hatırlatıcılar eklemesini,
- Ve maaş hesaplamalarını gerçekleştirmesini sağlar.

## 🧩 Modüller ve Özellikler

### 👤 Kullanıcı Yönetimi
- İlk kayıtlı kullanıcı `admin` olur.
- Kullanıcı tipleri: `admin`, `user`, `part-time-user`
- Sadece admin kullanıcılar:
  - Kullanıcı türünü değiştirebilir.
  - Kullanıcılara e-posta ile yeni şifre gönderebilir.
- Kapatma butonuna (X) basıldığında "Çıkmak istediğinizden emin misiniz?" uyarısı gösterilir.

### 📞 Telefon Rehberi
- Ad, soyad, telefon, adres, açıklama ve e-posta içeren kayıtlar tutulur.
- Kayıtları listeleme, oluşturma, güncelleme ve silme işlemleri yapılabilir.
- Veriler `phonebook.csv` dosyasında tutulur.
- Telefon numarası "(555) 555 55 55" formatında, e-posta ise stmp client ile doğrulanır.

### 📝 Notlar
- Basit not listesi içerir.
- Not ekle, güncelle, sil ve listele işlemleri.
- `notes.csv` dosyasında tutulur.

### 🧑‍💼 Kişisel Bilgiler
- Kullanıcı, ad-soyad, telefon, adres, e-posta, şifre ve fotoğraf gibi bilgilerini görebilir ve düzenleyebilir.
- Fotoğraf `base64` formatında saklanır.
- Şifre değiştirme ekranı içerir.

### 💰 Maaş Hesaplama
- Kullanıcının deneyim süresi, şehir, çocuk sayısı gibi veriler girilerek BMO’ya göre maaş hesaplanır.

### ⏰ Hatırlatıcı (Reminder)
- Toplantı veya görev türünde hatırlatıcılar eklenebilir (Abstract Factory kullanımı).
- Hatırlatıcı başlıkları pencere başlığına yansıtılır ve pencere 2 saniye titrer (Observer Pattern).
- Tarih, saat, özet ve açıklama alanlarını içerir.

## 🗃️ Veri Formatları

| Modül            | Dosya Adı         | Format     |
|------------------|-------------------|------------|
| Kullanıcılar     | `users.csv`       | CSV        |
| Telefon Rehberi  | `phonebook.csv`   | CSV        |
| Notlar           | `notes.csv`       | CSV        |
| Fotoğraflar      | Dahil (base64)    | CSV içinde |

Uygulama ekran görüntüleri ektedir:

(https://github.com/user-attachments/assets/c557084b-5a38-49c0-a8da-231d61ccabae)
(https://github.com/user-attachments/assets/ec2a6cb0-76d8-49e3-a2b0-09b97c6f2436)
(https://github.com/user-attachments/assets/56ff5422-cd00-4af9-9aea-99e6376ee3c2)
(https://github.com/user-attachments/assets/8b38fa61-dddd-4bb5-9ee0-b8c9cfab2e56)
(https://github.com/user-attachments/assets/534fd35e-940e-46c8-9d44-527139a7e7e9)
(https://github.com/user-attachments/assets/fb91e8ca-3a87-4a55-a6c6-e07b739d18dc)
(https://github.com/user-attachments/assets/39932f71-99a7-4c11-b665-4623dabee78c)
(https://github.com/user-attachments/assets/f4d801db-ba80-4598-a555-84ac5ef89d3d)



Cihan Bekem
Selin Uygunuçarlar
