using System;
using System.Windows;
using System.Windows.Controls;

namespace KanbanApp
{
    public partial class TaskWindow : Window
    {
        public Task Task { get; set; }

        public TaskWindow(Task task)
        {
            InitializeComponent();

            Task = task;
            DataContext = this;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var dueDate = DueDateDatePicker.SelectedDate;
            var dueTime = DueTimeTimePicker.Value?.TimeOfDay ?? TimeSpan.Zero;
            var dueDateTime = dueDate.Value.Date + dueTime;

            Task.Name = TaskNameTextBox.Text;
            Task.DueDate = dueDateTime;
            Task.Duration = DurationComboBox.SelectedValue as string;
            Task.Priority = PriorityComboBox.SelectedValue as string;

            DialogResult = true;
        }
    }
}