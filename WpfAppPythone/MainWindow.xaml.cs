using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;
using System.Text;
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
using WpfAppPythone.Pages;

namespace WpfAppPythone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static Frame main {  get; private set; }


        //public static UserApp User
        //{
        //    get
        //    {
        //        return User;
        //    }
        //    set
        //    {
        //        window._user = value;
        //    }
        //}

        public UserApp? _user
        {
            get
            {
                return _user;
            }
            set
            {
                if (value!=null)
                {
                    UserPointsWindow.Text = value.Points.ToString();
                    Logout.Visibility= Visibility.Visible;
                    MainBTN.Visibility = Visibility.Visible;
                    MarketBTN.Visibility = Visibility.Visible;
                }
                else
                {
                    Logout.Visibility = Visibility.Collapsed;
                    MainBTN.Visibility = Visibility.Collapsed;
                    MarketBTN.Visibility = Visibility.Collapsed;

                }
            }
        }



        public MainWindow()
        {
            InitializeComponent();
            main=MainFrame;
            App.window = this;
            App.Identity = null;
            if (App.Identity==null)
                GoToPage<LoginPage>(false);
            else
                GoToPage<MainPage>();
            //   content.Children
        }





        public static void GoToPage<T>(bool addToHistory=true) where T : Page
        {
            try
            {

                var page = App.ServiceProvider.GetRequiredService<T>();

                if (!addToHistory)
                {
                    NavigatedEventHandler? handler = null;
                    handler = (s, e) =>
                    {
                       
                        main.NavigationService.RemoveBackEntry();
                        main.Navigated -= handler;
                    };
                    main.Navigated += handler;
                }
                main.Navigate(page);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        public static void GoToSectionQuest(string IdSection,string idQuest="")
        {
            try
            {
                var page = App.ServiceProvider.GetRequiredService<QuestPage>();

                page.SectionId = IdSection;
                page.StartQuestId = idQuest;
                main.Navigate(page);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void MainBTN_Click(object sender, RoutedEventArgs e)
        {
            GoToPage<MainPage>();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            GoToPage<LoginPage>();
            App.Identity = null;
        }

        private void MarketBTN_Click(object sender, RoutedEventArgs e)
        {
            GoToPage<MarketPage>();
        }
    }
}