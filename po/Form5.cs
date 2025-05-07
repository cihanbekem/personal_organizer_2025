using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace po
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string surname = txtSurname.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirm.Text;
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();

            // Boş alan kontrolü
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("You must fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Telefon numarasındaki boşlukları kaldır
            phone = phone.Replace(" ", "");

            // Telefon numarasının geçerli olup olmadığını kontrol et
            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                MessageBox.Show("he phone number must be 10 digits long and consist of numbers only (e.g., 5555555555).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!email.EndsWith("@gmail.com"))
            {
                MessageBox.Show("The email must end with '@gmail.com'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // E-posta adresinin boşluk içermemesi gerektiğini kontrol et
            if (email.Contains(" "))
            {
                MessageBox.Show("The email address cannot contain spaces.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Şifre kontrolü
            if (password != confirmPassword)
            {
                MessageBox.Show("The passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcı kayıtlı mı?
            UserService.Initialize();
            if (UserService.IsEmailExists(email))
            {
                MessageBox.Show("This email is already registered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string role = UserService.IsFirstUser() ? "Admin" : "User";
            string imageFileName = "DijitalKullanıcıIkonu.png";
            string path = Path.Combine(Application.StartupPath, imageFileName);


            string base64Photo = ImageHelper.ConvertImageToBase64(path);

            // Kayıt işlemi
            try
            {
                UserService.AddUser(email, password, role, name, surname, phone, address, base64Photo);
                MessageBox.Show("Registration successful! You can log in now.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during registration: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
