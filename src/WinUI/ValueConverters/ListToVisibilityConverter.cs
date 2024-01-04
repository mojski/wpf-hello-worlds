using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WinUI.ValueConverters
{
    internal sealed class ListToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return Visibility.Collapsed;
            }

            if (value is IList &&
                value.GetType().IsGenericType &&
                value.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(IList)))
            {
                var list = (IList)value;

                return list.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
