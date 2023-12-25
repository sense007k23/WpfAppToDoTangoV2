using System.Windows;
using System.Windows.Input;

namespace KanbanApp
{
    public partial class PinWindow : Window
    {
        public PinWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (PinPasswordBox.Password == "1234")
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Incorrect PIN.");
            }
        }

        private void PinPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OkButton_Click(sender, e);
            }
        }
    }
}