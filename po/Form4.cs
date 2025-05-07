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
    public partial class UpdatePhoneRecord : Form
    {
        private string csvFilePath;
        private string oldName;
        private string oldPhone;
        private string oldEmail;
        private string oldSurname;
        private string oldUserEmail;


        public UpdatePhoneRecord(string name, string surname, string phone, string address, string description, string email, string UserEmail)
        {
            InitializeComponent();
            // CSV dosyasının yolunu al
            csvFilePath = Path.Combine(Application.StartupPath, "phonebook.csv");

            // Verileri formdaki TextBox'lara aktar
            txtName.Text = name;
            txtSurname.Text = surname;
            txtPhone.Text = phone;
            txtAddress.Text = address;
            txtDescription.Text = description;
            txtEmail.Text = email;

            oldName = name;       // Güncellenen kaydın eski adı
            oldSurname = surname; // Güncellenen kaydın eski soyadı
            oldPhone = phone;     // Güncellenen kaydın eski telefon numarası
            oldEmail = email;     // Güncellenen kaydın eski e-posta adresi
            oldUserEmail = UserEmail;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string surname = txtSurname.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string description = txtDescription.Text.Trim();
            string email = txtEmail.Text.Trim();

            // Ad, soyad ve telefon boş olamaz
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(surname))
            {
                MessageBox.Show("First name, last name and phone number cannot be empty.");
                return;
            }

            string digitsOnlyPhone = new string(phone.Where(char.IsDigit).ToArray());
            if (digitsOnlyPhone.Length != 10)
            {
                MessageBox.Show("The phone number is invalid. It should be in the format (555) 555 55 55.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // E-posta adresinin formatını kontrol et (sadece @gmail.com olmalı)
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                MessageBox.Show("The email must end with '@gmail.com'.");
                return;
            }

            string newLine = $"{name},{surname},{phone},{address},{description},{email},{oldUserEmail}";

            try
            {
                // CSV dosyasındaki mevcut satırları oku
                var lines = File.ReadAllLines(csvFilePath).ToList();
                var updatedLines = new List<string>();

                // Başlık satırını ekle
                updatedLines.Add(lines[0]);

                // Güncellenen satırı bul ve değiştir
                bool recordUpdated = false;  // Kayıt güncellenip güncellenmediğini takip et

                // Mevcut kayıtları kontrol et
                for (int i = 1; i < lines.Count; i++) // İlk satır başlık olduğu için i = 1
                {
                    var values = lines[i].Split(',');

                    // Eğer eski telefon numarası ve e-posta ile eşleşen bir kayıt varsa, onu güncelle
                    if (values[2] == oldPhone && values[5] == oldEmail)
                    {
                        updatedLines.Add(newLine);  // Yeni kaydı ekle
                        recordUpdated = true;
                    }
                    else
                    {
                        updatedLines.Add(lines[i]);  // Eski kaydı olduğu gibi ekle
                    }
                }

                // Eğer kayıt güncellenmediyse, kullanıcıya bilgi ver
                if (!recordUpdated)
                {
                    MessageBox.Show("The record to be updated could not be found.");
                    return;
                }

                // Güncellenmiş verileri CSV dosyasına yaz
                File.WriteAllLines(csvFilePath, updatedLines);

                MessageBox.Show("The record has been updated.");
                this.Close();  // Formu kapat
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


    }
    }

