using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppPythone.AppData;
using WpfAppPythone.AppData.Hasher;
using WpfAppPythone.AppData.Models;
using WpfAppPythone.Pages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfAppPythone.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {


        [ObservableProperty]
        private string _userName = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;


        [ObservableProperty]
        private string _errors = string.Empty;//string.Empty

        private readonly AppDBContext context;

        public AuthViewModel()
        {
        }
        public AuthViewModel(AppDBContext context) 
        {


            this.context = context;
        }
        [RelayCommand]
        public async Task GoToRegistration()
        {

            MainWindow.GoToPage<RegistrationPage>(false);
        }

        [RelayCommand]
        public async Task GoToLogin()
        {

            MainWindow.GoToPage<LoginPage>(false);
        }

        [RelayCommand]
        public async Task Registration()
        {
            Errors = string.Empty;
            var newUser = new UserApp();
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                Errors = ("Поля не заполнены");
                return;
            }
            var user = await context.Users.FirstOrDefaultAsync(u => u.Name == UserName);
            if (user != null)
            {
                Errors = ("Пользователь с этим ником уже есть. Придумай другой.");
                return;
            }
            newUser.Name=UserName;
            newUser.Password = Hasher.HashPasswordV3(Password);
            newUser.Points = 0;
            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();
            App.Identity = newUser;

            MainWindow.GoToPage<MainPage>(false);
        }

        [RelayCommand]
        public async Task Login()
        {

            Errors = string.Empty;
            if (string.IsNullOrEmpty(UserName)|| string.IsNullOrEmpty(Password))
            {
                Errors = ("Поля не заполнены");
                return;
            }
            var user = await context.Users.FirstOrDefaultAsync(u=>u.Name==UserName);
            if (user==null)
            {
                Errors = ("Пользователь не найден");
                return;
            }
            if (Hasher.VerifyHashedPasswordV3(Password, user.Password, out int iter, out var prf))
            {
                Errors="Неверный пароль";
                return;
            }

            App.Identity = user;

            MainWindow.GoToPage<MainPage>(false);

        }


        [RelayCommand]
        public void Otaldka()
        {
            string test = UserName;

        }



    }
}
