
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace po
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtEmail.Text = "s@gmail.com";
            txtPassword.Text = "1234";
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            lbl_Password.ForeColor = Color.Blue;
        }

        private void lblPassword_Click(object sender, EventArgs e)
        {
            PasswordResetForm resetForm = new PasswordResetForm();
            resetForm.ShowDialog();
        }


        private void label2_MouseLeave(object sender, EventArgs e)
        {
            lbl_Password.ForeColor = Color.White;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            SignUp signUpForm = new SignUp();

            // SignUp formunu aç
            signUpForm.ShowDialog();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            // E-posta ve şifre boş kontrolü
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Email and password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcıyı doğrulamak için UserService kullanarak giriş yapalım
            var user = UserService.GetUserByEmailAndPassword(email, password);

            if (user != null)
            {


                // Menü formunu açalım ve kullanıcı bilgilerini geçirelim
                MenuForm menuForm = new MenuForm(user);
                menuForm.Show();

                // Login formunu kapatalım
                this.Hide();
            }
            else
            {
                // Hatalı giriş
                MessageBox.Show("Invalid email or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked)
            {
                txtPassword.UseSystemPasswordChar = false; // Şifreyi göster
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true; // Şifreyi gizle
            }
        }
    }
}
