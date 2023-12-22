using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace KanbanApp
{
    public partial class MainWindow : Window
    {
        private TaskRepository taskRepository;
        private DispatcherTimer timer;

        public ObservableCollection<Task> Tasks { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            taskRepository = new TaskRepository();
            Tasks = new ObservableCollection<Task>(taskRepository.GetAllTasks());

            foreach (var task in Tasks)
            {
                task.PropertyChanged += Task_PropertyChanged;
            }

            Tasks.CollectionChanged += Tasks_CollectionChanged;

            // Calculate the time until the next minute
            DateTime now = DateTime.Now;
            TimeSpan timeUntilNextMinute = TimeSpan.FromMinutes(1) - TimeSpan.FromSeconds(now.Second);

            // Create and start the timer
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = timeUntilNextMinute;
            timer.Start();

            RefreshListViews();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the tasks and refresh the ListViews
            foreach (var task in Tasks)
            {
                taskRepository.UpdateTask(task);
            }
            RefreshListViews();

            // Set the interval to one minute
            timer.Interval = TimeSpan.FromMinutes(1);
        }

        private void RefreshListViews()
        {
            Backlog.ItemsSource = null;
            Backlog.ItemsSource = Tasks.Where(t => t.Status == "Backlog").ToList();
            Doing.ItemsSource = null;
            Doing.ItemsSource = Tasks.Where(t => t.Status == "Doing").ToList();
            Review.ItemsSource = null;
            Review.ItemsSource = Tasks.Where(t => t.Status == "Review").ToList();
            Done.ItemsSource = null;
            Done.ItemsSource = Tasks.Where(t => t.Status == "Done").ToList();

            if (Tasks.Any(t => t.Status == "Backlog" && t.TimeRemaining.TotalMinutes < 0))
            {
                PlaySound();
            }
        }

        private void PlaySound()
        {
            string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "notification1.wav");
            var player = new System.Media.SoundPlayer(path);
            player.Play();
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var task = new Task();
            var taskWindow = new TaskWindow(task);
            if (taskWindow.ShowDialog() == true)
            {
                task.Status = "Backlog";
                Tasks.Add(task);
                taskRepository.AddTask(task);
                RefreshListViews();
            }
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is Task task)
            {
                DragDrop.DoDragDrop(listView, task, DragDropEffects.Move);
            }
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (sender is ListView listView && e.Data.GetData(typeof(Task)) is Task task)
            {
                Tasks.Remove(task);
                taskRepository.DeleteTask(task);
                task.Status = listView.Name; // Set the task's status to the name of the ListView it was dropped on
                Tasks.Add(task);
                taskRepository.AddTask(task);
                RefreshListViews();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (sender is Border border && border.DataContext is Task task)
                {
                    var taskWindow = new TaskWindow(task);
                    if (taskWindow.ShowDialog() == true)
                    {
                        taskRepository.UpdateTask(task);
                        RefreshListViews();
                    }
                }
            }
        }

        private void Task_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is Task task)
            {
                taskRepository.UpdateTask(task);
                RefreshListViews();
            }
        }

        private void Tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Task task in e.OldItems)
                {
                    task.PropertyChanged -= Task_PropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (Task task in e.NewItems)
                {
                    task.PropertyChanged += Task_PropertyChanged;
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Task task)
            {
                Tasks.Remove(task);
                taskRepository.DeleteTask(task);
                RefreshListViews();
            }
        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            var p3Tasks = Tasks.Where(t => t.Status == "Backlog" && t.Priority == "P3").ToList();
            if (p3Tasks.Count > 0)
            {
                var random = new Random();
                var randomP3Task = p3Tasks[random.Next(p3Tasks.Count)];

                Tasks.Remove(randomP3Task);
                Tasks.Insert(0, randomP3Task);

                RefreshListViews();
            }
        }
    }
}