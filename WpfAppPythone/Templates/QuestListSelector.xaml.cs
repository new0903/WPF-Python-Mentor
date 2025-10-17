using MaterialDesignThemes.Wpf;
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
    /// Логика взаимодействия для QuestListSelector.xaml
    /// </summary>
    public partial class QuestListSelector : UserControl
    {
        public static readonly DependencyProperty OnSelectProperty = DependencyProperty.Register(
         "OnSelect",
         typeof(ICommand),
         typeof(QuestListSelector),
         new PropertyMetadata(null));

        public ICommand OnSelect
        {
            get => (ICommand)GetValue(OnSelectProperty);
            set => SetValue(OnSelectProperty, value);
        }

        public static readonly DependencyProperty BackgroundQuestProperty = DependencyProperty.Register(
         "BackgroundQuest",
         typeof(Style),
         typeof(QuestListSelector),
         new PropertyMetadata(null));

        public Style BackgroundQuest
        {
            get => (Style)GetValue(BackgroundQuestProperty);
            set => SetValue(BackgroundQuestProperty, value);
        }

        private Style defaultStyle;

        public QuestListSelector()
        {
            InitializeComponent();
            defaultStyle = QuestBTN.Style;
            Loaded += (_,__) =>
            {

                SetStyle();
            };
        }


        private void SetStyle()
        {
            if (DataContext is Quest quest)
            {
                var teset = quest.UsersQuest.FirstOrDefault(u => u.UserId == App.Identity.Id &&
                u.IsCompleted == AppData.Models.Enums.UserQuestCompleted.Completed);
                if (teset!=null)
                {
                    QuestBTN.Style = BackgroundQuest;
                }
                else
                {
                    QuestBTN.Style= defaultStyle;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var bindObject = DataContext;
            if (bindObject is not Quest quest)
                return;
            
            if (OnSelect?.CanExecute(quest) == true)
            {
                OnSelect.Execute(quest);  
            }
        }
    }
}
