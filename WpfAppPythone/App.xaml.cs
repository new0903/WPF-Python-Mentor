using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Security.Principal;
using System.Windows;
using WpfAppPythone.AppData;
using WpfAppPythone.AppData.Models;
using WpfAppPythone.Extensions;
using WpfAppPythone.PythonCompiler;

namespace WpfAppPythone
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        


        public static ServiceProvider ServiceProvider { get; private set; }

        public static ServiceCollection serviceCollection { get; private set; }


        public static MainWindow window { get;  set; }

        private static UserApp? _identity;
        public static UserApp? Identity
        {
            get => _identity;
            set
            {
                _identity = value;

                window._user = value; 
            }
        }

        private bool _initDB=false;

        public App()
        {

            serviceCollection = new ServiceCollection();

            serviceCollection.Init();


            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    
        protected override async void OnStartup(StartupEventArgs e)
        {
            //var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            //mainWindow.Show();
            if (!_initDB)
            {
                var context = ServiceProvider.GetRequiredService<AppDBContext>();
                await context.SeedData();
                _initDB = true;
                var python = ServiceProvider.GetRequiredService<PythoneCompiler>();
                await python.InitPython();
            }

            base.OnStartup(e);
        }
    }

}
