using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppPythone.AppData;
using WpfAppPythone.Pages;
using WpfAppPythone.PythonCompiler;
using WpfAppPythone.ViewModels;

namespace WpfAppPythone.Extensions
{
    public static class ServiceExtensions
    {

        public static IServiceCollection Init(this IServiceCollection services )
        {
            services.ConfigureRepositories();
            services.ConfigureViewModels();
            services.ConfigurePages();
            return services;
        }
        private static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddDbContext<AppDBContext>();
           // services.AddScoped<AppDBContext>();
            return services;
        }
        private static IServiceCollection ConfigureViewModels(this IServiceCollection services)
        {
            services.AddSingleton<PythoneCompiler>();

            services.AddTransient<MainViewModel>();

            services.AddTransient<QuestViewModel>();

            services.AddTransient<AuthViewModel>();


            services.AddTransient<MarketViewModel>();

            return services;
        }
        private static IServiceCollection ConfigurePages(this IServiceCollection services)
        {
            // Регистрация зависимостей
            services.AddSingleton<MainWindow>();



            services.AddSingleton<MainPage>();
            services.AddSingleton<LoginPage>();
            services.AddSingleton<RegistrationPage>();

            services.AddTransient<QuestPage>();
            services.AddTransient<MarketPage>();
            services.AddTransient<PythonPage>();
            return services;
        }
    }
}
