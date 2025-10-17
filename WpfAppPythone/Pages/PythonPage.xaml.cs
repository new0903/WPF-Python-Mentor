using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WpfAppPythone.PythonCompiler;

namespace WpfAppPythone.Pages
{
    /// <summary>
    /// Логика взаимодействия для PythonPage.xaml
    /// </summary>
    public partial class PythonPage : Page
    {
        private readonly PythoneCompiler python;

        public PythonPage()
        {
            InitializeComponent();
            this.python = App.ServiceProvider.GetRequiredService<PythoneCompiler>();
        }

        private void OnDownloadPythonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Открываем официальный сайт Python для скачивания
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://www.python.org/downloads/",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть браузер: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async void OnRetryCheckClick(object sender, RoutedEventArgs e)
        {
           var result= await python.InitPython();
            if (result)
            {

                MainWindow.GoToPage<LoginPage>(false);
            }
        }
    }
}
