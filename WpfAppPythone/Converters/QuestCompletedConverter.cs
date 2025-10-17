using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using WpfAppPythone.AppData.Models;
using WpfAppPythone.AppData.Models.Enums;
using WpfAppPythone.Pages;
using WpfAppPythone.ViewModels;

namespace WpfAppPythone.Converters
{
    class QuestCompletedConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {

            if (value!=null&& value is Quest model)
            {
                var userQuest= model.UsersQuest.FirstOrDefault(u=>u.UserId==App.Identity.Id);
                if (userQuest==null)
                {

                    return Brushes.White;
                }
                if (userQuest.IsCompleted == UserQuestCompleted.Completed)
                {
                    return Brushes.Green;
                }
                else
                {
                    return Brushes.Red;
                }
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Brushes.White;
        }
    }
}
