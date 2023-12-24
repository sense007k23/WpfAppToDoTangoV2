using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.ComponentModel;
using System.Diagnostics;
using System.Data.SQLite;


namespace KanbanApp
{
    public partial class MainWindow : Window
    {
        private TaskRepository taskRepository;
        private DispatcherTimer timer;
        private readonly DispatcherTimer _timer;
        private FullScreenModal fullScreenModal;

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


            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick_clock;
            _timer.Start();


            RefreshListViews();
        }

        private void Timer_Tick_clock(object sender, EventArgs e)
        {
            var timeSinceMidnight = DateTime.Now - DateTime.Today;
            CountdownTextBlock.Text = timeSinceMidnight.ToString(@"hh\:mm\:ss");
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
            Backlog.ItemsSource = Tasks
            .Where(t => t.Status == "Backlog" && t.DueDate.HasValue)
            .OrderBy(t => t.DueDate.Value)
            .ThenBy(t => t.Priority)
            .ToList();
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

            var doingTasks = Tasks.Where(t => t.Status == "Doing").ToList();
            Doing.ItemsSource = null;
            Doing.ItemsSource = doingTasks;

            if (doingTasks.Count == 0)
            {
                PlaySound_DoingBucketEmpty();
            }

                           

            if (doingTasks.Count == 0 || doingTasks.Any(t => t.Stopwatch.Elapsed > t.DurationTimeSpan))
            {
                PlaySound_DoingBucketEmpty();
                if (fullScreenModal == null || !fullScreenModal.IsVisible)
                {
                    fullScreenModal = new FullScreenModal();
                    fullScreenModal.Show();
                }
            }

            [DllImport("user32.dll", SetLastError = true)]
            static extern bool LockWorkStation();

            //If Stop watch is not running.
            if (doingTasks.Count == 0 || doingTasks.All(t => !t.Stopwatch.IsRunning))
            {
                PlaySound_DoingBucketEmpty();
                if (fullScreenModal == null || !fullScreenModal.IsVisible)
                {
                    fullScreenModal = new FullScreenModal();
                    fullScreenModal.Show();
                }
                //System.Windows.MessageBox.Show("No Active Task Running.");
                //LockWorkStation();
            }

            CreateTasksFromCsv();


        }

        private void PlaySound()
        {
            string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "notification1.wav");
            var player = new System.Media.SoundPlayer(path);
            player.Play();
        }

        private void PlaySound_DoingBucketEmpty()
        {
            string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "notification2.wav");
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

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.DataContext is Task task)
            {
                var taskWindow = new TaskWindow(task) { Owner = this };
                if (taskWindow.ShowDialog() == true)
                {
                    taskRepository.UpdateTask(task);
                    RefreshListViews();
                }
            }
        }

        private void Backlog_MouseEnter(object sender, MouseEventArgs e)
        {
            CreateTaskButton.Visibility = Visibility.Visible;
            ShuffleTaskButton.Visibility = Visibility.Visible;
        }

        private void Backlog_MouseLeave(object sender, MouseEventArgs e)
        {
            CreateTaskButton.Visibility = Visibility.Collapsed;
            ShuffleTaskButton.Visibility = Visibility.Collapsed;
        }

        private void CreateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var task = new Task();
            var taskWindow = new TaskWindow(task) { Owner = this };
            if (taskWindow.ShowDialog() == true)
            {
                task.Status = "Backlog";
                Tasks.Add(task);
                taskRepository.AddTask(task);
                RefreshListViews();
            }
        }

        private void MoveToBacklogMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MoveTaskToBucket(sender, "Backlog");
        }

        private void MoveToDoingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MoveTaskToBucket(sender, "Doing");
        }

        private void MoveToReviewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MoveTaskToBucket(sender, "Review");
        }

        private void MoveToDoneMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MoveTaskToBucket(sender, "Done");
        }

        private void MoveTaskToBucket(object sender, string bucket)
        {
            if (sender is MenuItem menuItem && menuItem.DataContext is Task task)
            {
                Tasks.Remove(task);
                taskRepository.DeleteTask(task);
                task.Status = bucket;
                if (bucket == "Doing" && !task.Stopwatch.IsRunning)
                {
                    task.Stopwatch.Start();
                }
                else if (bucket != "Doing" && task.Stopwatch.IsRunning)
                {
                    task.Stopwatch.Stop();
                    task.ElapsedTime += task.Stopwatch.Elapsed;
                    //task.Stopwatch.Reset();
                }
                Tasks.Add(task);
                taskRepository.AddTask(task);
                RefreshListViews();
            }
        }

        private void QuickAddTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string taskName = InputBox.Show();
            if (!string.IsNullOrEmpty(taskName))
            {
                var task = new Task
                {
                    Name = taskName,
                    DueDate = DateTime.Now.AddMinutes(15),
                    Duration = "2 minutes",
                    Priority = "P1",
                    Status = "Backlog"
                };
                Tasks.Add(task);
                taskRepository.AddTask(task);
                RefreshListViews();
            }
        }

        private void CreateTasksFromCsv()
        {
            var csvLines = File.ReadAllLines("recurringtask.csv");
            foreach (var line in csvLines.Skip(1))  // Skip the header line
            {
                var fields = line.Split(',');
                var dueDate = DateTime.Today.Add(DateTime.Parse(fields[1].Trim()).TimeOfDay);
                if (dueDate >= DateTime.Now && dueDate <= DateTime.Now.AddMinutes(120))
                {
                    var taskName = fields[0].Trim() + "_" + DateTime.Today.ToString("dd_MM_yyyy");
                    if (!Tasks.Any(t => t.Name == taskName))
                    {
                        var task = new Task
                        {
                            Name = taskName,
                            DueDate = dueDate,
                            Duration = fields[2].Trim(),
                            Priority = fields[3].Trim(),
                            Status = "Backlog"
                        };
                        Tasks.Add(task);
                        taskRepository.AddTask(task);
                    }
                }
            }
        }    


    }


    public class TimeRemainingToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeRemaining)
            {
                if (timeRemaining.TotalMinutes < 15)
                {
                    return Brushes.LightCoral;
                }
                else
                {
                    return Brushes.Transparent;
                }
            }
            else
            {
                return Brushes.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /******************* Task Class **************************************/

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
                        return $"{timeSpan.Hours} hours and {timeSpan.Minutes % 60} minutes due";
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

        public Stopwatch Stopwatch { get; } = new Stopwatch();

        public string ElapsedFormatted
        {
            get
            {
                var totalElapsedTime = ElapsedTime;
                if (Stopwatch.IsRunning)
                {
                    totalElapsedTime += Stopwatch.Elapsed;
                }
                return totalElapsedTime.ToString(@"hh\:mm");
            }
        }

        public TimeSpan ElapsedTime { get; set; }

        public TimeSpan DurationTimeSpan
        {
            get
            {
                var parts = Duration.Split(' ');
                if (parts.Length >= 2)
                {
                    if (int.TryParse(parts[0], out int value))
                    {
                        if (parts[1].StartsWith("hour"))
                        {
                            return TimeSpan.FromHours(value);
                        }
                        else if (parts[1].StartsWith("minute"))
                        {
                            return TimeSpan.FromMinutes(value);
                        }
                    }
                }
                return TimeSpan.Zero;
            }
        }

    }

    /*********************************************************************/


    /************** Task Repository Class ***********************************/

    public class TaskRepository
    {
        private const string DatabaseFileName = "Tasks.db";

        public TaskRepository()
        {
            if (!File.Exists(DatabaseFileName))
            {
                SQLiteConnection.CreateFile(DatabaseFileName);
            }

            using (var connection = new SQLiteConnection($"Data Source={DatabaseFileName};Version=3;"))
            {
                connection.Open();

                string sql = "CREATE TABLE IF NOT EXISTS Tasks (Name TEXT, DueDate TEXT, Duration TEXT, Priority TEXT, Status TEXT, ElapsedTime TEXT)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Task> GetAllTasks()
        {
            var tasks = new List<Task>();

            using (var connection = new SQLiteConnection($"Data Source={DatabaseFileName};Version=3;"))
            {
                connection.Open();

                string sql = "SELECT * FROM Tasks";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new Task
                            {
                                Name = (string)reader["Name"],
                                DueDate = DateTime.Parse((string)reader["DueDate"]),
                                Duration = (string)reader["Duration"],
                                Priority = (string)reader["Priority"],
                                Status = (string)reader["Status"],
                                ElapsedTime = TimeSpan.Parse((string)reader["ElapsedTime"])
                            });
                        }
                    }
                }
            }

            return tasks;
        }

        public void AddTask(Task task)
        {
            using (var connection = new SQLiteConnection($"Data Source={DatabaseFileName};Version=3;"))
            {
                connection.Open();

                string sql = $"INSERT INTO Tasks (Name, DueDate, Duration, Priority, Status, ElapsedTime) VALUES ('{task.Name}', '{task.DueDate}', '{task.Duration}', '{task.Priority}', '{task.Status}', '{task.ElapsedTime}')";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTask(Task task)
        {
            using (var connection = new SQLiteConnection($"Data Source={DatabaseFileName};Version=3;"))
            {
                connection.Open();

                string sql = $"UPDATE Tasks SET DueDate = '{task.DueDate}', Duration = '{task.Duration}', Priority = '{task.Priority}', Status = '{task.Status}', ElapsedTime = '{task.ElapsedTime}' WHERE Name = '{task.Name}'";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTask(Task task)
        {
            using (var connection = new SQLiteConnection($"Data Source={DatabaseFileName};Version=3;"))
            {
                connection.Open();

                string sql = $"DELETE FROM Tasks WHERE Name = '{task.Name}'";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    /************************************************************************/

    /********************** Input Box ****************************************/

    public static class InputBox
    {
        public static string Show()
        {
            var inputBox = new Window()
            {
                Width = 500,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            var textBox = new TextBox
            {
                Margin = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            textBox.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    inputBox.Close();
                }
            };

            inputBox.Content = textBox;
            inputBox.ShowDialog();

            return textBox.Text;
        }
    }



    /**************************************************************************/








}