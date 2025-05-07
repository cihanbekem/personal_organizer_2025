using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace po
{

    public partial class ReminderFormManager : Form
    {
        private ReminderManager _manager;
        private BaseReminder _selectedReminder;
        

        // (UI elementleri: lstReminders, txtSummary, txtDescription, dtpDate, dtpTime, etc.)

        public ReminderFormManager()
        {
            InitializeComponent();
            _manager = new ReminderManager(this);
            LoadList();

            timer.Interval = 1000; // her 1 saniyede bir kontrol et
            timer.Tick += (s, e) => _manager.CheckReminders();
            timer.Start();

            cmbType.Items.Add("Meeting");
            cmbType.Items.Add("Task");
            cmbType.SelectedIndex = 0;

            dtpTime.Format = DateTimePickerFormat.Time;
            dtpTime.ShowUpDown = true;

        }

        private void LoadList()
        {
            lstReminders.Items.Clear();
            foreach (var r in _manager.GetAll())
                lstReminders.Items.Add(r);
        }



        private void lstReminders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstReminders.SelectedItem is BaseReminder r)
            {
                _selectedReminder = r;
                txtSummary.Text = r.Summary;
                txtDescription.Text = r.Description;
                dtpDate.Value = r.Date;
                dtpTime.Value = r.Time;

                if (r is MeetingReminder m)
                    txtLocation.Text = m.Location;
                else
                    txtLocation.Clear();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedReminder == null)
            {
                MessageBox.Show("Please select a reminder to update.");
                return;
            }

            _selectedReminder.Summary = txtSummary.Text;
            _selectedReminder.Description = txtDescription.Text;
            _selectedReminder.Date = dtpDate.Value.Date;
            _selectedReminder.Time = dtpTime.Value;

            if (_selectedReminder is MeetingReminder m)
                m.Location = txtLocation.Text;

            _manager.Update(_selectedReminder);
            LoadList();
            MessageBox.Show("Reminder updated.");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedReminder == null)
            {
                MessageBox.Show("Please select a reminder to delete.");
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this reminder?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                _manager.Delete(_selectedReminder.Id);
                _selectedReminder = null;
                LoadList();
                ClearFields();
                MessageBox.Show("Reminder deleted.");
            }
        }
        private void ClearFields()
        {
            txtSummary.Clear();
            txtDescription.Clear();
            txtLocation.Clear();
            dtpDate.Value = DateTime.Now;
            dtpTime.Value = DateTime.Now;
        }

        private void numPriority_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ReminderFactory factory = cmbType.SelectedItem?.ToString() == "Meeting"
    ? new MeetingReminderFactory()
    : new TaskReminderFactory();

            BaseReminder r = factory.CreateReminder();
            r.Summary = txtSummary.Text;
            r.Description = txtDescription.Text;
            r.Date = dtpDate.Value.Date;
            r.Time = dtpTime.Value;

            if (r is MeetingReminder m)
                m.Location = txtLocation.Text;

            _manager.Add(r);
            LoadList();
        }

        private void txtSummary_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
