using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppPythone.AppData;
using WpfAppPythone.AppData.Models;
using WpfAppPythone.AppData.Models.Enums;
using WpfAppPythone.Pages;

namespace WpfAppPythone.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        [ObservableProperty]
        private int _points = 0;


        [ObservableProperty]
        private ObservableCollection<Section> _sections = [];

        [ObservableProperty]
        private ObservableCollection<Quest> _quests = [];
        private readonly AppDBContext context;

        public MainViewModel() 
        {

        }

        public MainViewModel(AppDBContext context)
        {
            this.context = context;
        }

        public async Task GetSections()
        {
            try
            {
                Points = App.Identity.Points;
                await CheckSectionInProgress();
                var sections = await context.Sections.Include(s=>s.UsersSection).OrderBy(s=>s.Index).ToArrayAsync();
                var quests = await context.Quests.Include(q=>q.UsersQuest).ToArrayAsync();
                if (sections.Length > 0)
                {
                    Sections = [.. sections];
                    Quests = [.. quests];
                }
                else
                {
                    Sections.Add(new Section("Test1"));
                    Sections.Add(new Section("Test2"));
                    Sections.Add(new Section("Test3"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникал ошибка в методе GetSections:\n{ex.Message}");
            }

        }

        private async Task CheckSectionInProgress()
        {
            //var userSections =await context.UserSections.Where(us=>us.UserId==App.Identity.Id).ToArrayAsync();
            //var selectSection =await context.Sections.OrderBy(s => s.Index).ToArrayAsync();
            var sections = await context.Sections.Include(s => s.UsersSection).OrderBy(s => s.Index).ToArrayAsync();
            var userCurrentSection= sections.FirstOrDefault(s=> s.UsersSection.Where(us => 
                us.IsCompleted == UserSectionCompleted.InProgress&&
                us.UserId == App.Identity.Id
            ).Count()>0);
          //  var userCompletedSection = sections.Where(s => s.UsersSection.Where(us => us.IsCompleted == UserSectionCompleted.Completed&&us.UserId == App.Identity.Id).Count() > 0).OrderBy(s=>s.Index).ToArray();
            if (userCurrentSection==null)
            {
                var next = sections.FirstOrDefault(s => s.UsersSection.Where(us => us.IsCompleted == UserSectionCompleted.Completed &&
                us.UserId == App.Identity.Id).Count() < 1);
                if (next!=null)
                {

                    var us = new UserSections();
                    us.UserId = App.Identity.Id;
                    us.SectionId = next.Id;
                    us.IsCompleted = UserSectionCompleted.InProgress;
                    await context.AddAsync(us);
                    await context.SaveChangesAsync();
                }
            }
        }

        [RelayCommand]
        public void SelectSection(Section section)
        {
            if (section == null) return;
            var sect = Sections.FirstOrDefault(s => s.Id == section.Id);
            if (sect!=null)
            {
                MainWindow.GoToSectionQuest(sect.Id);
            }
        }


        [RelayCommand]
        public void GoToMarket()
        {
            MainWindow.GoToPage<MarketPage>();
        }
    }
}
