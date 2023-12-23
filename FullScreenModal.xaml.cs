using System.Windows;


namespace KanbanApp
{
    public partial class FullScreenModal : Window
    {
        public FullScreenModal()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowState = WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}