using po;
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
    public partial class MenuForm : Form
    {
        private User currentUser; // Kullanıcı bilgisini tutan değişken
        public MenuForm(User user)
        {
            InitializeComponent();
            currentUser = user; // Gelen User nesnesini al
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            lblUserName.Text = $"{currentUser.Name} {currentUser.Surname}";

            // Kullanıcı rolünü gösteriyoruz
            lblRole.Text = $" {currentUser.Role}";

            // E-posta adresini gösteriyoruz
            lblEmail.Text = $" {currentUser.Email}";

            pctBox_Photo.Image = ImageHelper.ConvertBase64ToImage(currentUser.Photo);

            // Rol kontrolü yapılıyor
            if (currentUser.Role.ToLower() == "user")
            {
                guna2PictureBox6.Visible = false;
            }
            else if (currentUser.Role.ToLower() == "admin")
            {
                guna2PictureBox6.Visible = true;
            }

        }

        private void guna2PictureBox5_Click(object sender, EventArgs e)
        {
            string userEmail = currentUser.Email;  // currentUser nesnesindeki Email

            // PhoneBook formunu e-posta adresiyle başlat
            PhoneBook phoneBookForm = new PhoneBook(userEmail);
            phoneBookForm.Show();
        }

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {

            PersonalInformation personalInformation = new PersonalInformation(currentUser);
            personalInformation.Show();
            this.Hide();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            salary_calculator form = new salary_calculator(currentUser);
            form.Show();
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {
            Notes NotesForm = new Notes();
            NotesForm.Show();
        }

        private void guna2PictureBox6_Click(object sender, EventArgs e)
        {
            UserManagementForm user_management = new UserManagementForm();
            user_management.Show();
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            ReminderFormManager reminderForm = new ReminderFormManager();
            reminderForm.Show();
        }

        private void MenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true; // Kapatmayı iptal eder
            }
        }
    }
}
