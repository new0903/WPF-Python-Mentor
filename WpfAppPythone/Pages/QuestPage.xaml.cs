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
    /// Логика взаимодействия для QuestPage.xaml
    /// </summary>
    public partial class QuestPage : Page
    {
        public string SectionId { get; set; }
        public string StartQuestId { get; set; }
        private QuestViewModel model;

        public QuestPage(QuestViewModel questViewModel)
        {
            InitializeComponent();
            DataContext=questViewModel;
            model = questViewModel;
            Loaded += async (_, __) =>
            {

                await model.loadQuest(SectionId, StartQuestId);
                //GenerateDesciption();
            };

        }
        //protected override  void OnInitialized(EventArgs e)
        //{
        //    base.OnInitialized(e);
        //    //DataContext = questViewModel;
        //    // GenerateGuests();
        //}

        public void GenerateDesciption()
        {

            if (model == null) return;

            var elems=model.QuestCurrent.DescriptionJSON;

            foreach (var item in elems)
            {
                var label = new Label();
                label.Content= item.Value;
                label.Margin = new Thickness(10);
                if (item.Key=="Simple")
                {

                    label.FontSize = 16;
                }
                else
                {
                    label.FontWeight = FontWeights.Bold;
                    label.FontSize = 16;
                }
                ContentDescription.Children.Add(label);
            }

        }


        //<Label Content = "{Binding DescriptionQuest}" Margin="10" FontSize="14"/>

    }
}
