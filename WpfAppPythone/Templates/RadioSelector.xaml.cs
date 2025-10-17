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
using WpfAppPythone.AppData.Models;

namespace WpfAppPythone.Templates
{
    /// <summary>
    /// Логика взаимодействия для RadioSelector.xaml
    /// </summary>
    public partial class RadioSelector : UserControl
    {
        public static readonly DependencyProperty OnSelectProperty = DependencyProperty.Register(
         "OnSelect",
         typeof(ICommand),
         typeof(RadioSelector),
         new PropertyMetadata(null));

        public ICommand OnSelect
        {
            get => (ICommand)GetValue(OnSelectProperty);
            set => SetValue(OnSelectProperty, value);
        }
        public RadioSelector()
        {
            InitializeComponent();
        }

        private void RadioSelectorQuest_Checked(object sender, RoutedEventArgs e)
        {
            if (OnSelect?.CanExecute(RadioSelectorQuest.Content) == true)
            {
                OnSelect.Execute(RadioSelectorQuest.Content);
            }
        }
    }
}
