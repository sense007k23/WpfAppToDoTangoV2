using System;
using System.ComponentModel;

namespace KanbanApp
{
    public class Task : INotifyPropertyChanged
    {
        private string name;
        private DateTime? dueDate;
        private string duration;
        private string priority;
        private string status;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public DateTime? DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value;
                OnPropertyChanged("DueDate");
                OnPropertyChanged("TimeDue");
            }
        }

        public string Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged("Duration");
            }
        }

        public string Priority
        {
            get { return priority; }
            set
            {
                priority = value;
                OnPropertyChanged("Priority");
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public string TimeDue
        {
            get
            {
                if (DueDate.HasValue)
                {
                    var timeSpan = DueDate.Value - DateTime.Now;
                    if (timeSpan.TotalMinutes < 0)
                    {
                        return $"Overdue by {-timeSpan.Hours} hours and {-timeSpan.Minutes % 60} minutes";
                    }
                    else if (timeSpan.TotalMinutes < 1)
                    {
                        return "Due now";
                    }
                    else if (timeSpan.TotalHours < 1)
                    {
                        return $"{timeSpan.Minutes} minutes due";
                    }
                    else if (timeSpan.TotalDays < 1)
                    {
                        return $"{timeSpan.Hours} hours due";
                    }
                    else
                    {
                        return $"{timeSpan.Days} days due";
                    }
                }
                else
                {
                    return "No due date";
                }
            }
        }

        public TimeSpan TimeRemaining
        {
            get
            {
                if (DueDate.HasValue)
                {
                    return DueDate.Value - DateTime.Now;
                }
                else
                {
                    return TimeSpan.MaxValue;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}