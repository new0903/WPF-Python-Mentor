using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppPythone.ViewModels;

namespace WpfAppPythone.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage(AuthViewModel model)
        {
            InitializeComponent();
            DataContext=model;
        }

        private void PasswordField_PasswordChanged(object sender, RoutedEventArgs e)
        {

            if (DataContext is AuthViewModel vm)
            {
                // Чтобы не зациклить событие
                if (vm.Password != PasswordField.Password)
                {
                    vm.Password = PasswordField.Password;
                }
            }
        }
    }
}
