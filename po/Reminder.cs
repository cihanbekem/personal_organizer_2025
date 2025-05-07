using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po
{
    // Observer Arayüzü
    public interface IReminderObserver
    {
        void Update(BaseReminder reminder);
    }

    // Reminder Sınıfı (Observer destekli soyut sınıf)
    public abstract class BaseReminder
    {
        private List<IReminderObserver> _observers = new();

        public int Id { get; set; }
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

        public bool IsNotified { get; set; } = false; // EKLENDİ

        public abstract string ReminderType { get; }

        public void Attach(IReminderObserver observer) => _observers.Add(observer);

        public void Notify()
        {
            foreach (var observer in _observers)
                observer.Update(this);
        }

        public override string ToString()
        {
            return $"{Date.ToShortDateString()} {Time.ToShortTimeString()} - {ReminderType}: {Summary}";
        }
    }

    // Meeting Reminder (Concrete Class)
    public class MeetingReminder : BaseReminder
    {
        public string? Location { get; set; }
        public override string ReminderType => "Meeting";
    }

    // Task Reminder (Concrete Class)
    public class TaskReminder : BaseReminder
    {
        public bool IsCompleted { get; set; }
        public int Priority { get; set; }
        public override string ReminderType => "Task";
    }

    // Abstract Factory
    public abstract class ReminderFactory
    {
        public abstract BaseReminder CreateReminder();
    }

    public class MeetingReminderFactory : ReminderFactory
    {
        public override BaseReminder CreateReminder() => new MeetingReminder();
    }

    public class TaskReminderFactory : ReminderFactory
    {
        public override BaseReminder CreateReminder() => new TaskReminder();
    }

    // Observer Gerçekleştiricisi
    public class FormReminderObserver : IReminderObserver
    {
        private Form _form;
        private string _originalTitle;

        public FormReminderObserver(Form form)
        {
            _form = form;
            _originalTitle = form.Text;
        }

        public void Update(BaseReminder reminder)
        {
            // Formu 3 saniye shake et
            Task.Run(() =>
            {
                ShakeForm();
            });

            // Hatırlatma mesajı
            MessageBox.Show($"Hatırlatma zamanı geldi!\n\n{reminder.Summary}", "Hatırlatma", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShakeForm()
        {
            var original = _form.Location;
            var rnd = new Random();
            for (int i = 0; i < 20; i++)
            {
                int offsetX = rnd.Next(-10, 10);
                int offsetY = rnd.Next(-10, 10);
                _form.Invoke(new Action(() =>
                {
                    _form.Location = new Point(original.X + offsetX, original.Y + offsetY);
                }));
                System.Threading.Thread.Sleep(50);
            }

            _form.Invoke(new Action(() =>
            {
                _form.Location = original;
            }));
        }


    }
}
