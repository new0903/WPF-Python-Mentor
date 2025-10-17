using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfAppPythone.AppData.Models.Enums;

namespace WpfAppPythone.Templates
{
    /// <summary>
    /// Логика взаимодействия для SectionChart.xaml
    /// </summary>
    public partial class SectionChart : UserControl
    {


        // Переименуем свойства чтобы не было конфликтов
        public static readonly DependencyProperty ChartSectionsProperty =
            DependencyProperty.Register(
                "ChartSections",
                typeof(ObservableCollection<Section>),
                typeof(SectionChart),
                new PropertyMetadata(null, OnDataChanged));

        public static readonly DependencyProperty ChartQuestsProperty =
            DependencyProperty.Register(
                "ChartQuests",
                typeof(ObservableCollection<Quest>),
                typeof(SectionChart),
                new PropertyMetadata(null, OnDataChanged));

        public ObservableCollection<Section> ChartSections
        {
            get => (ObservableCollection<Section>)GetValue(ChartSectionsProperty);
            set => SetValue(ChartSectionsProperty, value);
        }

        public ObservableCollection<Quest> ChartQuests
        {
            get => (ObservableCollection<Quest>)GetValue(ChartQuestsProperty);
            set => SetValue(ChartQuestsProperty, value);
        }

        public SeriesCollection SeriesCollection { get; set; }

        public SectionChart()
        {
            SeriesCollection = new SeriesCollection();
            //DataContext = this;
            InitializeComponent();
            Chart.Series = SeriesCollection;
            // Тестовые данные на старте
            LoadTestData();
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SectionChart)d;
            control.UpdateChart();
        }

        private void UpdateChart()
        {
            SeriesCollection.Clear();

            if (ChartSections?.Count > 0 && ChartQuests?.Count > 0)
            {
                // Простая логика для проверки
                var completed = ChartSections.Count(s =>
                    s.UsersSection?.Any(u => u.IsCompleted == UserSectionCompleted.Completed&&u.UserId==App.Identity.Id) == true);
                var total = ChartSections.Count-completed;

               var seriesCompleted=new PieSeries
                {
                    Title = "Пройдено",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(completed) },
                    DataLabels = true
                };

                var seriesNoCompleted=new PieSeries
                {
                    Title = "Всего",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(total) },
                    DataLabels = true
                };
                SeriesCollection.Add(seriesCompleted);
                SeriesCollection.Add(seriesNoCompleted);
                var currentSection = ChartSections.FirstOrDefault(s => s.UsersSection.FirstOrDefault(u => u.UserId == App.Identity.Id && u.IsCompleted == UserSectionCompleted.InProgress) != null);

                if (currentSection != null) {
                    var questsUser = ChartQuests.Where(q =>q.SectionId == currentSection.Id).ToArray();
                    if (questsUser.Length>0)
                    {
                        var qCompleted= questsUser.Where(q=>q.UsersQuest.FirstOrDefault(u=>u.IsCompleted==UserQuestCompleted.Completed && u.UserId == App.Identity.Id) !=null).Count();
                        var qNoCompleted = questsUser.Length - qCompleted;
                        seriesNoCompleted.Values.Add(new ObservableValue(qNoCompleted));
                        seriesCompleted.Values.Add(new ObservableValue(qCompleted));
                    }
                }
            }
            else
            {
                LoadTestData();
            }
        }

        private void LoadTestData()
        {
            SeriesCollection.Add(new PieSeries
            {
                Title = "Тест данные",
                Values = new ChartValues<double> { 3 },
                DataLabels = true
            });
        }

        /*
        public static readonly DependencyProperty AllSectionProperty = DependencyProperty.Register(
         "AllSection",
         typeof(ObservableCollection<Section>),
         typeof(SectionChart),
         new PropertyMetadata(null, OnAllChanged));

        public ObservableCollection<Section> AllSection
        {
            get => (ObservableCollection<Section>)GetValue(AllSectionProperty);
            set => SetValue(AllSectionProperty, value);
        }


        public static readonly DependencyProperty AllQuestProperty = DependencyProperty.Register(
         "AllQuest",
         typeof(ObservableCollection<Quest>),
         typeof(SectionChart),
         new PropertyMetadata(null, OnAllChanged));

        public ObservableCollection<Quest> AllQuest
        {
            get => (ObservableCollection<Quest>)GetValue(AllQuestProperty);
            set => SetValue(AllQuestProperty, value);
        }

        public SeriesCollection SeriesCollection{ get; set; }

        public SectionChart()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection();

            DataContext = this;
            InitializeChart();
        }

        private static void OnAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SectionChart)d;
            MessageBox.Show("OnAllChanged");
            control.InitializeChart();
        }

        public void InitializeChart()
        {
            SeriesCollection.Clear();
            //Chart.Series = SeriesCollection;
            if (AllSection != null&& AllSection.Count>0)
            {

                //var uQ = q.UsersSection.FirstOrDefault(u=>u.UserId==App.Identity.Id);

                var completedS = AllSection.Where(s=>s.UsersSection.FirstOrDefault(u => u.UserId == App.Identity.Id&&u.IsCompleted==UserSectionCompleted.Completed)!=null).Count();
                var notCompletedS = AllSection.Count- completedS;


                var seriesCompleted = new PieSeries
                {
                    Title = "Пройденные темы",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(completedS) },
                    DataLabels = true
                };


                var seriesNoCompleted = new PieSeries
                {
                    Title = "Не пройденные темы",
                    
                    Values = new ChartValues<ObservableValue> { new ObservableValue(notCompletedS) },
                    DataLabels = true
                };
                SeriesCollection.Add(seriesCompleted);
                SeriesCollection.Add(seriesNoCompleted);
          //      var currentSection = AllSection.FirstOrDefault(s => s.UsersSection.FirstOrDefault(u => u.UserId == App.Identity.Id && u.IsCompleted == UserSectionCompleted.InProgress) != null);

                //if (currentSection != null) {
                //    var questsUser = AllQuest.Where(q =>q.SectionId == currentSection.Id).ToArray();
                //    if (questsUser.Length>0)
                //    {
                //        var qCompleted= questsUser.Where(q=>q.UsersQuest.FirstOrDefault(u=>u.IsCompleted==UserQuestCompleted.Completed)!=null).Count();
                //        var qNoCompleted = questsUser.Length-qCompleted;
                //        seriesNoCompleted.Values.Add(new ObservableValue(qCompleted));
                //        seriesNoCompleted.Values.Add(new ObservableValue(qNoCompleted));
                //    }
                //}

            }
            else
            {
                // ТЕСТОВЫЕ ДАННЫЕ если нет реальных
                SeriesCollection.Add(new PieSeries
                {
                    Title = "Тест пройдено",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(3) },
                    DataLabels = true
                });

                SeriesCollection.Add(new PieSeries
                {
                    Title = "Тест не пройдено",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(2) },
                    DataLabels = true
                });
            }

        
        }
        */
    }
}
