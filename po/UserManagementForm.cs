using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

namespace po
{
    public partial class UserManagementForm : Form
    {
        private string filePath = "users.csv";  // Kullanıcıların kaydedileceği CSV dosyası
        private List<User> users = new List<User>();  // Kullanıcıları tutacak liste

        public UserManagementForm()
        {
            InitializeComponent();
            LoadUsers();  // Başlangıçta kullanıcıları yükle
        }

        // CSV dosyasından kullanıcıları alıp DataGridView'e yükleme
        private void LoadUsers()
        {
            users = GetUsersFromFile();
            dataGridViewUsers.DataSource = users;
        }

        // CSV dosyasından kullanıcıları almak (örnek)
        private List<User> GetUsersFromFile()
        {
            var usersList = new List<User>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath, Encoding.UTF8).Skip(1); // başlığı atla
                int idCounter = 1; // ID artık CSV'de yoksa otomatik verilecek

                foreach (var line in lines)
                {
                    var columns = line.Split(',');

                    if (columns.Length < 5) // ilk 5 alan yoksa atla
                        continue;

                    var user = new User
                    {
                        ID = idCounter++,
                        Email = columns[0],
                        Password = columns[1],
                        Role = columns[2],
                        Name = columns[3],
                        Surname = columns[4]
                    };
                    usersList.Add(user);
                }
            }

            return usersList;
        }


        // TextBox'lar ve ComboBox'ı temizleme fonksiyonu
        private void ClearFields()
        {
            txtName.Clear();
            txtSurname.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            cmbRole.SelectedIndex = -1;  // ComboBox'ı sıfırlama
        }

        // Yeni Kullanıcı Ekleme


        // Kullanıcıyı dosyaya eklemek
        private void AddUserToFile(User user)
        {
            using (var writer = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                writer.WriteLine($"{user.ID},{user.Name},{user.Surname},{user.Email},{user.Password},{user.Role}");
            }
        }




        // Kullanıcıyı dosyada güncellemek
        private void UpdateUserInFile(User user)
        {
            var allUsers = GetUsersFromFile();  // Tüm kullanıcıları al
            var updatedUser = allUsers.FirstOrDefault(u => u.ID == user.ID);
            if (updatedUser != null)
            {
                if (user.Name != "")
                {
                    updatedUser.Name = user.Name;
                }

                if (user.Surname != "")
                {
                    updatedUser.Surname = user.Surname;
                }

                if (user.Email != "")
                {
                    updatedUser.Email = user.Email;
                }

                if (user.Password != "")
                {
                    updatedUser.Password = user.Password;
                }

                if (user.Role != "")
                {
                    updatedUser.Role = user.Role;
                }

            }

            // Güncellenmiş kullanıcıları yeniden dosyaya yaz
            File.WriteAllLines(filePath, allUsers.Select(u => $"{u.ID},{u.Name},{u.Surname},{u.Email},{u.Password},{u.Role}"), Encoding.UTF8);
        }




        // Kullanıcıyı dosyadan silmek
        private void DeleteUserFromFile(User user)
        {
            if (!File.Exists(filePath)) return;

            var allLines = File.ReadAllLines(filePath).ToList();
            var header = allLines[0];
            var users = allLines.Skip(1).ToList();

            // Email'e göre eşleşen kullanıcıyı bul ve çıkar
            users = users.Where(line =>
            {
                var cols = line.Split(',');
                return cols.Length >= 4 && !cols[0].Equals(user.Email, StringComparison.OrdinalIgnoreCase);
            }).ToList();

            // Dosyayı tekrar yaz
            File.WriteAllLines(filePath, new[] { header }.Concat(users), Encoding.UTF8);
        }

        // Seçili Kullanıcıyı Al
        private User GetSelectedUser()
        {
            // DataGridView'den seçilen kullanıcıyı al
            var selectedRow = dataGridViewUsers.SelectedRows[0];
            return (User)selectedRow.DataBoundItem;
        }





        private void guna2Button4_Click(object sender, EventArgs e)
        {
            var selectedUser = GetSelectedUser();
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user.");
                return;
            }

            string csvPath = "users.csv";
            if (!File.Exists(csvPath))
            {
                MessageBox.Show("User file not found.");
                return;
            }

            var allLines = File.ReadAllLines(csvPath).ToList();
            var header = allLines[0]; // İlk satır başlık
            var users = allLines.Skip(1).ToList();

            // Email ilk sütun (index 0)
            int userIndex = users.FindIndex(line =>
            {
                var cols = line.Split(',');
                return cols.Length >= 5 && cols[0].Trim().ToLower() == selectedUser.Email.ToLower();
            });

            if (userIndex == -1)
            {
                MessageBox.Show("Selected user not found in CSV.");
                return;
            }

            string newPassword = GenerateTemporaryPassword();

            // Şifreyi 2. sütuna (index 1) yaz
            string[] userParts = users[userIndex].Split(',');
            userParts[1] = newPassword;
            users[userIndex] = string.Join(",", userParts);

            // CSV'yi yeniden oluştur
            var updatedCsv = new[] { header }.Concat(users);
            File.WriteAllLines(csvPath, updatedCsv, Encoding.UTF8);

            // E-posta gönderme
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("oopdeneme@gmail.com");
                mail.To.Add(selectedUser.Email);
                mail.Subject = "Your New Password";
                mail.Body = $"Hello {selectedUser.Name},\n\nYour password has been reset.\nNew Password: {newPassword}\n\nPlease log in and change it.";

                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("oopdeneme@gmail.com", "veidownxevndefes"); // Gmail App Password
                smtp.EnableSsl = true;
                smtp.Send(mail);

                MessageBox.Show("New password sent to the user's email.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email: " + ex.Message);
            }
        }

        private string GenerateTemporaryPassword()
        {
            string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            Random rnd = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                              .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        private void SendEmail(string email, string newPassword)
        {
            // Gmail SMTP ayarları
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("oopdeneme@gmail.com", "your-app-password"),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("oopdeneme@gmail.com"),
                Subject = "Your New Password",
                Body = $"Your new password is: {newPassword}",
                IsBodyHtml = false
            };
            mailMessage.To.Add(email);

            smtpClient.Send(mailMessage);
        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Boş alan kontrolü
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtSurname.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text) ||
                cmbRole.SelectedItem == null)
            {
                // Eğer herhangi bir alan boşsa kullanıcıya uyarı göster
                MessageBox.Show("Please fill in all the fields.");
                return;  // İşlemi durdur
            }

            // İlk kullanıcıyı admin yap, sonrakiler user veya admin olabilir
            string role = users.Count == 0 ? "admin" : cmbRole.SelectedItem.ToString();  // İlk kullanıcı admin olur

            var newUser = new User
            {
                ID = users.Count + 1,  // Yeni kullanıcı ID'si, mevcut kullanıcı sayısına göre artırılır
                Name = txtName.Text,
                Surname = txtSurname.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                Role = role
            };

            // Yeni kullanıcıyı dosyaya ekle
            AddUserToFile(newUser);
            LoadUsers();  // Kullanıcıyı listeye ekle
            ClearFields();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            var selectedUser = GetSelectedUser();  // Seçili kullanıcıyı al

            // Eğer kullanıcı admin ise rolü değiştirilemez
            if (selectedUser.Role == "admin" && cmbRole.SelectedItem != null && cmbRole.SelectedItem.ToString() != "admin")
            {
                MessageBox.Show("The admin user's role cannot be changed..");
                return;  // Admin rolünü değiştirmeye çalışıyorsa işlemi durdur
            }

            // Eğer rol seçilmemişse, mevcut rolü bırak
            selectedUser.Name = txtName.Text;
            selectedUser.Surname = txtSurname.Text;
            selectedUser.Email = txtEmail.Text;
            selectedUser.Password = txtPassword.Text;

            // Eğer combo box'tan bir rol seçilmemişse, mevcut rolü kullan
            selectedUser.Role = cmbRole.SelectedItem != null ? cmbRole.SelectedItem.ToString() : selectedUser.Role;

            // Kullanıcıyı dosyada güncelle
            UpdateUserInFile(selectedUser);
            LoadUsers();  // Listeyi güncelle
            ClearFields();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            var selectedUser = GetSelectedUser();  // Seçili kullanıcıyı al

            if (selectedUser.Role == "admin")
            {
                MessageBox.Show("You cannot delete the admin user.");
                return;  // Admin kullanıcıyı silmemek için işlemi durdur
            }

            DeleteUserFromFile(selectedUser);
            LoadUsers();  // Listeyi güncelle
            ClearFields();
        }



        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }



}