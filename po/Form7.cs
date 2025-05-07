using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace po
{
    public partial class PersonalInformation : Form
    {
        private User currentUser;
        public PersonalInformation(User user)
        {
            InitializeComponent();
            currentUser = user;
        }

        

        private void PersonalInformation_Load(object sender, EventArgs e)
        {
            string basephoto = currentUser.Photo;
            txtAddress.Text = currentUser.Address;
            txtEmail.Text = currentUser.Email;
            txtName.Text = currentUser.Name;
            txtPassword.Text = currentUser.Password;
            txtPhone.Text = currentUser.Phone;
            txtSurname.Text = currentUser.Surname;
            UserPicturebox.Image = ImageHelper.ConvertBase64ToImage(basephoto);



        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            // Formdaki bilgilerle currentUser'ı güncelle
            currentUser.Name = txtName.Text;
            currentUser.Surname = txtSurname.Text;
            currentUser.Phone = txtPhone.Text;
            currentUser.Address = txtAddress.Text;
            currentUser.Password = txtPassword.Text;
            // E-mail değiştirilmemeli, güvenlik için
            // currentUser.Email = txtEmail.Text;

            // Fotoğraf değişmediyse, mevcut kalır. İleride güncelleme eklenirse buraya gelir.

            // CSV dosyasını da güncelle
            UserService.UpdateUser(currentUser);

            MessageBox.Show("Your information has been successfully updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        public static class ImageHelper
        {
            public static string ConvertImageToBase64(Image image)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png); // istersen ImageFormat.Jpeg de olabilir
                    return Convert.ToBase64String(ms.ToArray());
                }
            }

            public static Image ConvertBase64ToImage(string base64)
            {
                if (string.IsNullOrWhiteSpace(base64)) return null;
                byte[] bytes = Convert.FromBase64String(base64);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a photo";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;

                    // PictureBox'a resmi yükle
                    Image image = Image.FromFile(selectedFilePath);
                    UserPicturebox.Image = image;

                    // Base64 string'e çevir ve kullanıcıya ata
                    currentUser.Photo = ImageHelper.ConvertImageToBase64(image);
                }
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            MenuForm form = new MenuForm(currentUser);
            form.Show();
            this.Hide();
        }
    }
}

