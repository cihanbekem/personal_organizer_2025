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
    public partial class Notes : Form
    {
        public Notes()
        {
            InitializeComponent();
            notes();
        }

        public class Note
        {
            public string? Content { get; set; }
        }

        private readonly string notesFilePath = "notes.csv";
        private List<Note> notesList = new List<Note>();

        private void notes()
        {
            notesList.Clear();
            lstNotes.Items.Clear();

            if (File.Exists(notesFilePath))
            {
                var lines = File.ReadAllLines(notesFilePath);
                foreach (var line in lines)
                {
                    notesList.Add(new Note { Content = line });
                    lstNotes.Items.Add(line);
                }
            }
        }

        private void SaveNotes()
        {
            File.WriteAllLines(notesFilePath, notesList.Select(n => n.Content));
        }

        private void LoadNotes()
        {
            notesList.Clear();
            lstNotes.Items.Clear();

            if (File.Exists(notesFilePath))
            {
                var lines = File.ReadAllLines(notesFilePath);
                foreach (var line in lines)
                {
                    notesList.Add(new Note { Content = line });
                    lstNotes.Items.Add(line);
                }
            }
        }






        private void lstNotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = lstNotes.SelectedIndex;
            if (selectedIndex >= 0)
            {
                txtNoteContent.Text = notesList[selectedIndex].Content;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var content = txtNoteContent.Text.Trim();
            if (!string.IsNullOrEmpty(content))
            {
                notesList.Add(new Note { Content = content });
                SaveNotes();
                LoadNotes();
                txtNoteContent.Clear();
            }
            else
            {
                MessageBox.Show("Note content cannot be empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            var selectedIndex = lstNotes.SelectedIndex;
            if (selectedIndex >= 0)
            {
                var content = txtNoteContent.Text.Trim();
                if (!string.IsNullOrEmpty(content))
                {
                    notesList[selectedIndex].Content = content;
                    SaveNotes();
                    LoadNotes();
                    txtNoteContent.Clear();
                }
                else
                {
                    MessageBox.Show("Note content cannot be empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select the note to be updated.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            var selectedIndex = lstNotes.SelectedIndex;
            if (selectedIndex >= 0)
            {
                var result = MessageBox.Show("Are you sure you want to delete the selected note?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    notesList.RemoveAt(selectedIndex);
                    SaveNotes();
                    LoadNotes();
                    txtNoteContent.Clear();
                }
            }
            else
            {
                MessageBox.Show("Please select the note to be deleted.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
