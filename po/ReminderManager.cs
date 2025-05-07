using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po
{
    public class ReminderManager
    {
        private List<BaseReminder> _reminders = new();
        private readonly string _filePath = "reminders.csv";
        private int _nextId = 1;
        private FormReminderObserver _observer;

        public ReminderManager(Form form)
        {
            _observer = new FormReminderObserver(form);
            Load();

        }

        public List<BaseReminder> GetAll() => _reminders;

        public void Add(BaseReminder r)
        {
            r.Id = _nextId++;
            r.Attach(_observer);
            _reminders.Add(r);
            Save();
        }

        public void Update(BaseReminder updated)
        {
            var index = _reminders.FindIndex(r => r.Id == updated.Id);
            if (index >= 0)
            {
                updated.Attach(_observer);
                _reminders[index] = updated;
                Save();
            }
        }

        public void Delete(int id)
        {
            _reminders.RemoveAll(r => r.Id == id);
            Save();
        }
        private void Load()
        {
            if (!File.Exists(_filePath)) return;

            foreach (var line in File.ReadAllLines(_filePath))
            {
                var parts = line.Split(',');
                if (parts.Length < 6) continue;

                BaseReminder r = parts[1] == "Meeting"
                    ? new MeetingReminder { Location = parts[5] }
                    : new TaskReminder { IsCompleted = bool.Parse(parts[5]), Priority = int.Parse(parts[6]) };

                r.Id = int.Parse(parts[0]);
                r.Summary = parts[2];
                r.Description = parts[3];
                r.Date = DateTime.Parse(parts[4]);
                r.Time = DateTime.Parse(parts[4]);
                r.Attach(_observer);
                _reminders.Add(r);
            }

            if (_reminders.Any())
                _nextId = _reminders.Max(r => r.Id) + 1;
        }

        private void Save()
        {
            var lines = new List<string>();
            foreach (var r in _reminders)
            {
                if (r is MeetingReminder m)
                    lines.Add($"{r.Id},Meeting,{r.Summary},{r.Description},{r.Date},{m.Location}");
                else if (r is TaskReminder t)
                    lines.Add($"{r.Id},Task,{r.Summary},{r.Description},{r.Date},{t.IsCompleted},{t.Priority}");
            }
            File.WriteAllLines(_filePath, lines);
        }

        public void CheckReminders()
        {
            DateTime now = DateTime.Now;
            foreach (var r in _reminders)
            {
                if (!r.IsNotified && r.Date.Date == now.Date && Math.Abs((r.Time - now).TotalSeconds) < 1)
                {
                    r.IsNotified = true;
                    r.Notify();
                }
            }
        }
    }

}
