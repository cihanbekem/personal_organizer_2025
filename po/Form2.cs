using System.Data;

namespace po
{
    public partial class PhoneBook : Form
    {
        private string csvFilePath;
        private string currentUserEmail;
        public PhoneBook(string email)
        {
            InitializeComponent();
            this.currentUserEmail = email;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            CreatePhoneRecord createForm = new CreatePhoneRecord(csvFilePath, currentUserEmail);
            createForm.ShowDialog();


        }

        private void PhoneBook_Load(object sender, EventArgs e)
        {
            csvFilePath = Path.Combine(Application.StartupPath, "phonebook.csv");

            // Eğer dosya yoksa, başlık satırı ile dosyayı oluştur
            if (!File.Exists(csvFilePath))
            {
                File.WriteAllText(csvFilePath, "Name,Surname,Phone,Address,Description,Email,UserEmail\n");
            }

            // Kayıtları listele
            //ListRecords();
        }

        public void ListRecords()
        {
            if (!File.Exists(csvFilePath))
            {
                MessageBox.Show("The record file could not be found.");
                return;
            }

            DataTable dt = new DataTable();

            try
            {
                var lines = File.ReadAllLines(csvFilePath);

                if (lines.Length == 0)
                {
                    MessageBox.Show("The CSV file is empty.");
                    return;
                }

                // Başlık satırını al (son sütunu dahil etme)
                string[] headers = lines[0].Split(',');
                for (int j = 0; j < headers.Length - 1; j++) // son header hariç
                {
                    dt.Columns.Add(headers[j].Trim());
                }

                // Diğer verileri ekle – sadece giriş yapan kullanıcıya ait olanları
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split(',');

                    // Email adresi son sütunda yer alıyor varsayımıyla
                    if (fields.Length >= headers.Length && fields[headers.Length - 1].Trim().Equals(currentUserEmail, StringComparison.OrdinalIgnoreCase))
                    {
                        // Eksik sütun varsa boşlukla tamamla
                        while (fields.Length < headers.Length)
                            fields = fields.Append("").ToArray();

                        // Son sütunu (email) hariç al
                        object[] row = fields.Take(headers.Length - 1).ToArray();
                        dt.Rows.Add(row);
                    }
                }

                dgvPhoneBook.DataSource = dt;

                // Görünüm ayarları
                dgvPhoneBook.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvPhoneBook.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvPhoneBook.ReadOnly = true;
                dgvPhoneBook.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvPhoneBook.MultiSelect = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Listing error: " + ex.Message);
            }
        }




        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (!File.Exists(csvFilePath))
            {
                MessageBox.Show("The record file could not be found.");
                return;
            }

            ListRecords();
        }

        private void dgvPhoneBook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Satır seçildiğinde Update butonunu aktif et
                btnUpdate.Enabled = true;
            }
            // Eğer bir satır seçiliyse, Delete butonunu aktif hale getir
            if (dgvPhoneBook.SelectedRows.Count > 0)
            {
                btnDelete.Enabled = true; // Delete butonunu etkinleştir
            }
            else
            {
                btnDelete.Enabled = false; // Delete butonunu devre dışı bırak
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvPhoneBook.SelectedRows.Count > 0)
            {
                // Seçilen satırı al
                var selectedRow = dgvPhoneBook.SelectedRows[0];

                // Seçilen satırdaki verileri al
                string name = selectedRow.Cells[0].Value.ToString();
                string surname = selectedRow.Cells[1].Value.ToString();
                string phone = selectedRow.Cells[2].Value.ToString();
                string address = selectedRow.Cells[3].Value.ToString();
                string description = selectedRow.Cells[4].Value.ToString();
                string email = selectedRow.Cells[5].Value.ToString();

                // UpdatePhoneRecord formunu oluştur ve bilgileri ilet
                UpdatePhoneRecord updateForm = new UpdatePhoneRecord(name, surname, phone, address, description, email, currentUserEmail);

                // Güncelleme işlemi için formu aç
                updateForm.ShowDialog();

                // Verileri tekrar listele
                ListRecords();
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }



        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (dgvPhoneBook.SelectedRows.Count > 0)
            {
                // Seçilen satırı al
                var selectedRow = dgvPhoneBook.SelectedRows[0];

                string name = selectedRow.Cells[0].Value.ToString();
                string surname = selectedRow.Cells[1].Value.ToString();
                string phone = selectedRow.Cells[2].Value.ToString();
                string address = selectedRow.Cells[3].Value.ToString();
                string description = selectedRow.Cells[4].Value.ToString();
                string email = selectedRow.Cells[5].Value.ToString();

                // Silme onayı al
                DialogResult result = MessageBox.Show(
                    $"'{name} {surname}' are you sure you want to delete this record",
                    "Delete Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var lines = File.ReadAllLines(csvFilePath).ToList();
                        var newLines = new List<string> { lines[0] }; // başlık satırı

                        for (int i = 1; i < lines.Count; i++)
                        {
                            var fields = lines[i].Split(',');
                            if (fields[0] != name || fields[1] != surname || fields[2] != phone ||
                                fields[3] != address || fields[4] != description || fields[5] != email)
                            {
                                newLines.Add(lines[i]);
                            }
                        }

                        File.WriteAllLines(csvFilePath, newLines);
                        MessageBox.Show("The record has been successfully deleted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListRecords();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred during the deletion process: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Kullanıcı hayır dedi, hiçbir şey yapma
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


    }
}
