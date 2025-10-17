using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppPythone.AppData.Models;

namespace WpfAppPythone.Templates
{
    /// <summary>
    /// Логика взаимодействия для SectionCard.xaml
    /// </summary>
    public partial class SectionCard : UserControl
    {


        public static readonly DependencyProperty SelectSectionCardProperty =DependencyProperty.Register(
         "SelectSectionCard",
         typeof(ICommand),
         typeof(SectionCard),
         new PropertyMetadata(null));

        public ICommand SelectSectionCard
        {
            get => (ICommand)GetValue(SelectSectionCardProperty);
            set => SetValue(SelectSectionCardProperty, value);
        }

        public SectionCard()
        {
            InitializeComponent();

            Loaded += (_, __) =>
            {

                CheckEnable();
            };
        }


        public void CheckEnable()
        {

            if (DataContext is Section section)
            {
                var user=section.UsersSection.FirstOrDefault(u=>u.UserId==App.Identity.Id&&(
                u.IsCompleted==AppData.Models.Enums.UserSectionCompleted.InProgress||
                u.IsCompleted == AppData.Models.Enums.UserSectionCompleted.Completed));

                if (user == null)
                    SectionBTN.IsEnabled = false;
                else
                    SectionBTN.IsEnabled = true;
            }

        }

        private void GoToTest()
        {

            var bindObject = DataContext;
            if (bindObject is not Section section)
                return;

            if (SelectSectionCard?.CanExecute(section) == true)
            {
                SelectSectionCard.Execute(section);  // Передаем объект Section как параметр
            }
        }

        private void SelectSectionCardClick(object sender, RoutedEventArgs e)
        {
            GoToTest();
            //SelectSectionCard.Execute(section);

        }

        private void Card_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            GoToTest();
        }
    }
}
