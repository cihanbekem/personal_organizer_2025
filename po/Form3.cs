using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace po
{
    public partial class CreatePhoneRecord : Form
    {
        private string csvFilePath;
        private string currentUserEmail;
        public CreatePhoneRecord(string csvFilePath, string userEmail)
        {
            InitializeComponent();
            this.csvFilePath = csvFilePath;
            this.currentUserEmail = userEmail;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string surname = txtSurname.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string description = txtDescription.Text.Trim();
            string email = txtEmail.Text.Trim();

            // Ad ve Soyad kontrolü
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
            {
                MessageBox.Show("First name and last name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Telefon format kontrolü: sadece rakam içermeli ve 10 hane olmalı
            string digitsOnlyPhone = new string(phone.Where(char.IsDigit).ToArray());
            if (digitsOnlyPhone.Length != 10)
            {
                MessageBox.Show("The phone number is invalid. It should be in the format (555) 555 55 55.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Email kontrolü: "@gmail.com" ile bitmeli
            if (!email.EndsWith("@gmail.com"))
            {
                MessageBox.Show("The email must end with '@gmail.com'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // CSV formatına uygun satır
            string newLine = $"{name},{surname},{phone},{address},{description},{email},{currentUserEmail}"; // Burada 'currentUserEmail' e-posta adresini kullanıyoruz.

            try
            {
                File.AppendAllText(csvFilePath, newLine + Environment.NewLine);
                MessageBox.Show("The record has been successfully added..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
