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
    /// Логика взаимодействия для MarketPage.xaml
    /// </summary>
    public partial class MarketPage : Page
    {
        public MarketPage( MarketViewModel model)
        {
            InitializeComponent();
            DataContext=model;
            Loaded += async (_, __) =>
            {
                await model.LoadData();
            };
        }
    }
}
