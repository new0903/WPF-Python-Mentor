using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WpfAppPythone.AppData.Models;
using WpfAppPythone.AppData.Models.Enums;
using WpfAppPythone.Pages;
using WpfAppPythone.ViewModels;

namespace WpfAppPythone.Converters
{
    class QuestConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {

            if (value!=null&& parameter is QuestType questType&& value is QuestType model)
            {
                if (model == questType)
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Visibility.Collapsed;
        }
    }
}
