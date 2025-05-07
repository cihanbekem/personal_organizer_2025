using System;
using System.IO;
using System.Linq;

namespace po
{
    public static class UserService
    {
        public static string FilePath = "users.csv";

        // Initialize metodu dosya yoksa başlık yazmak için kullanılır
        public static void Initialize()
        {
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "Email,Password,Role,Name,Surname,Phone,Address,Photo\n");
            }
        }

        // E-posta var mı kontrol eder
        public static bool IsEmailExists(string email)
        {
            if (!File.Exists(FilePath)) return false;
            return File.ReadAllLines(FilePath).Any(line => line.Split(',')[0] == email);
        }

        public static void UpdateUser(User updatedUser)
        {
            if (!File.Exists(FilePath)) return;

            var lines = File.ReadAllLines(FilePath).ToList();

            for (int i = 1; i < lines.Count; i++) // i = 1 çünkü başlık satırını atlıyoruz
            {
                var columns = lines[i].Split(',');

                if (columns[0] == updatedUser.Email)
                {
                    // Güncellenmiş verileri satır olarak oluştur
                    lines[i] = $"{updatedUser.Email},{updatedUser.Password},{updatedUser.Role},{updatedUser.Name},{updatedUser.Surname},{updatedUser.Phone},{updatedUser.Address},{updatedUser.Photo}";
                    break;
                }
            }

            File.WriteAllLines(FilePath, lines);
        }

        // İlk kullanıcı mı kontrol eder (Admin olacak)
        public static bool IsFirstUser()
        {
            if (!File.Exists(FilePath)) return true;
            var lines = File.ReadAllLines(FilePath);
            return lines.Length <= 1; // sadece başlık varsa
        }

        // Yeni kullanıcı ekler
        public static void AddUser(string email, string password, string role,
                                    string name, string surname, string phone, string address, string base64Photo)
        {
            string line = $"{email},{password},{role},{name},{surname},{phone},{address}, {base64Photo}";
            File.AppendAllText(FilePath, line + Environment.NewLine);
        }
        // Kullanıcıyı e-posta ve şifre ile bulur
        public static User GetUserByEmailAndPassword(string email, string password)
        {
            // Dosyanın var olup olmadığını kontrol et
            if (!File.Exists(FilePath)) return null;

            // Dosyadaki tüm satırları oku
            var lines = File.ReadAllLines(FilePath);

            // Her satırı kontrol et (ilk satır başlık olduğu için onu atla)
            foreach (var line in lines.Skip(1)) // Başlık satırını atla
            {
                var columns = line.Split(',');
                string storedEmail = columns[0];
                string storedPassword = columns[1];

                // E-posta ve şifreyi karşılaştır
                if (storedEmail == email && storedPassword == password)
                {
                    // Eğer eşleşirse, yeni bir User nesnesi döndür
                    return new User
                    {
                        Email = storedEmail,
                        Password = storedPassword,
                        Name = columns[3],
                        Surname = columns[4],
                        Role = columns[2],
                        Phone = columns[5],
                        Address = columns[6],
                        Photo = columns[7]
                    };
                }
            }

            return null; // Kullanıcı bulunamadı
        }
    }
}
