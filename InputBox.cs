using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

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