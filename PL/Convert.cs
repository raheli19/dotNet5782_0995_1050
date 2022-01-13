using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PL
{
    #region DisplaysButtonsInDroneWindow

    /// <summary>
    /// Multi binding for xaml window
    /// </summary>
    public class DisplaysButtonsInDroneWindow : IMultiValueConverter // Multi binding
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)values[1] == true) return Visibility.Hidden;

            else if ((bool)values[0] == true) return Visibility.Hidden;
            else return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region BatteryBackground

    /// <summary>
    /// Colors the purcent of the battery
    /// </summary>
    public class BatteryBackground : IValueConverter // packade to list
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (Double)value <= 20)
                return "#FFE30A0A";
            else return "#FF159C11";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion


}
