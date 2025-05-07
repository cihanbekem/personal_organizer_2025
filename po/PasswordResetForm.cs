using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace po
{
    public partial class PasswordResetForm : Form
    {
        public PasswordResetForm()
        {
            InitializeComponent();
        }

        private void btnSendReset_Click(object sender, EventArgs e)
        {
            string userEmail = txtEmail.Text.Trim().ToLower();
            string csvPath = "users.csv";

            if (!File.Exists(csvPath))
            {
                MessageBox.Show("User file not found.");
                return;
            }

            var allLines = File.ReadAllLines(csvPath).ToList();
            var header = allLines[0]; // Email,Password,Role,Name,Surname,Phone,Address,Photo
            var users = allLines.Skip(1).ToList();
            int userIndex = users.FindIndex(l => l.Split(',')[0].Trim().ToLower() == userEmail);

            if (userIndex == -1)
            {
                MessageBox.Show("No account found with this email.");
                return;
            }

            string newPassword = GenerateTemporaryPassword();

            // Güncelleme
            string[] userParts = users[userIndex].Split(',');
            userParts[1] = newPassword; // sadece şifre (Password) kısmını değiştiriyoruz
            users[userIndex] = string.Join(",", userParts);

            // CSV'yi yaz
            var updatedCsv = new[] { header }.Concat(users);
            File.WriteAllLines(csvPath, updatedCsv);

            // Mail gönder (GMAIL SMTP)
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("oopdeneme@gmail.com");
                mail.To.Add(userEmail);
                mail.Subject = "Your New Password";
                mail.Body = $"Hello {userParts[3]},\n\nYour password has been reset.\nNew Password: {newPassword}\n\nPlease log in and change it.";

                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("oopdeneme@gmail.com", "veidownxevndefes"); // App password (boşluksuz!)
                smtp.EnableSsl = true;
                smtp.Send(mail);

                MessageBox.Show("New password sent to your email.");
                this.Close();
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



    }
}
