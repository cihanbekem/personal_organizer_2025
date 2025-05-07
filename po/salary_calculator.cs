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
    public partial class salary_calculator : Form
    {

        private User currentUser;
        public salary_calculator(User user)
        {
            InitializeComponent();
            currentUser = user;
            InitializeDropdowns();
        }

        private void InitializeDropdowns()
        {
            cmbCity.Items.AddRange(new object[] {
        "Istanbul", "Ankara", "Izmir",
        "Bursa", "Eskisehir", "Bilecik", "Other"
            });
            cmbExperience.Items.AddRange(new object[] {
        "0-2 years","2-4 years", "5-9 years", "10-14 years", "15-20 years", "Over 20 years"
            });
            cmbEducation.Items.AddRange(new object[] {
        "Master's Degree (related)", "PhD (related)", "Assoc. Prof. (rel)",
        "Master's Degree (other)", "PhD/Assoc. Prof. (other)", "None"
            });
            cmbManager.Items.AddRange(new object[] {
        "CTO / GM", "Project Director", "Project Manager",
        "Team Lead./Software Arch.", "IT Dept. (<=5 staff)", "IT Dept. (>5 staff)", "None"
            });
            cmbUserType.Items.AddRange(new object[] { "admin", "user", "part-time-user" });
        }

        private bool IsRadioGroupValid(RadioButton yesButton, RadioButton noButton)
        {
            return yesButton.Checked || noButton.Checked;
        }

        private bool AreAllRadioGroupsValid()
        {
            return IsRadioGroupValid(rdoEnglishEduYes, rdoEnglishEduNo)
                && IsRadioGroupValid(rdoEnglishCertifiedYes, rdoEnglishCertifiedNo)
                && IsRadioGroupValid(rdoOtherLangYes, rdoOtherLangNo)
                && IsRadioGroupValid(rdoSpouseNotWorking, rdoSpouseWorking)
                && IsRadioGroupValid(rdoChild0_6Yes, rdoChild0_6No)
                && IsRadioGroupValid(rdoChild7_18Yes, rdoChild7_18No)
                && IsRadioGroupValid(rdoChildUniversityYes, rdoChildUniversityNo);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            if (!AreAllRadioGroupsValid())
            {
                MessageBox.Show("Please answer all yes/no questions..", "Incomplete selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double baseSalary = 26005.50;
            double katsayi = 0;

            string selectedCity = cmbCity.SelectedItem?.ToString();
            if (selectedCity == "İstanbul") katsayi += 0.30;
            else if (selectedCity == "Ankara" || selectedCity == "İzmir") katsayi += 0.20;
            else if (selectedCity == "Bursa" || selectedCity == "Eskişehir" || selectedCity == "Bilecik") katsayi += 0.05;

            string experience = cmbExperience.SelectedItem?.ToString();
            if (experience == "0-2 years") katsayi += 0.30;
            if (experience == "2-4 years") katsayi += 0.60;
            else if (experience == "5-9 years") katsayi += 1.00;
            else if (experience == "10-14 years") katsayi += 1.20;
            else if (experience == "15-20 years") katsayi += 1.35;
            else if (experience == "Over 20 years") katsayi += 1.50;

            string education = cmbEducation.SelectedItem?.ToString();
            if (education == "Master's (related)") katsayi += 0.10;
            else if (education == "PhD (related)") katsayi += 0.30;
            else if (education == "Associate Professorship (related)") katsayi += 0.35;
            else if (education == "Master's (other)") katsayi += 0.05;
            else if (education == "PhD/Assoc. Prof. (other)") katsayi += 0.15;

            if (rdoEnglishEduYes.Checked) katsayi += 0.20;
            if (rdoEnglishCertifiedYes.Checked) katsayi += 0.20;
            if (rdoOtherLangYes.Checked) katsayi += 0.05;

            string manager = cmbManager.SelectedItem?.ToString();
            if (manager == "CTO / GM") katsayi += 1.00;
            else if (manager == "Project's Manager") katsayi += 0.85;
            else if (manager == "Proje Yöneticisi") katsayi += 0.75;
            else if (manager == "Team Leader / Software Architect") katsayi += 0.50;
            else if (manager == "BİM (<=5 Personnel)") katsayi += 0.40;
            else if (manager == "BİM (>5 Personnel)") katsayi += 0.60;

            if (rdoSpouseNotWorking.Checked) katsayi += 0.20;
            if (rdoChild0_6Yes.Checked) katsayi += 0.20;
            if (rdoChild7_18Yes.Checked) katsayi += 0.30;
            if (rdoChildUniversityYes.Checked) katsayi += 0.40;

            double calculated = baseSalary * (1 + katsayi);

            if (cmbUserType.SelectedItem?.ToString() == "part-time-user")
                calculated *= 0.5;

            lblSalary_1.Text = "User Name: " + currentUser.Name + $"\nGross Minimum Wage: {calculated:N2} TL";
        }

    }
}