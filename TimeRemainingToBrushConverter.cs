using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace KanbanApp
{
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
}