using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppPythone.AppData;
using WpfAppPythone.AppData.Models;

namespace WpfAppPythone.ViewModels
{
    public partial class MarketViewModel : ObservableObject
    {
        private readonly AppDBContext context;


        [ObservableProperty]
        private ObservableCollection<Item> _items=[];


        [ObservableProperty]
        private Item _selectItems;

        [ObservableProperty]
        private string _description =string.Empty;

        [ObservableProperty]
        private string _name =string.Empty;

        [ObservableProperty]
        private string _cost = string.Empty;


        [ObservableProperty]
        private string _countItems = string.Empty;

        public MarketViewModel() {
            //items=new ObservableCollection<Item>();
            Items.Add(new Item() { Cost=10,Description= "item test Description",Name="item name" });
            Items.Add(new Item() { Cost = 10, Description = "item test Description", Name = "item name" });
            Items.Add(new Item() { Cost = 10, Description = "item test Description", Name = "item name" });
        }


        public MarketViewModel(AppDBContext context)
        {
            this.context = context;
        }

        public async Task LoadData()
        {
            var itemsDB=await context.Items.ToArrayAsync();
            Items = [.. itemsDB];

        }


        [RelayCommand]
        public async Task SelectItem(string id)
        {
            var selitem = Items.FirstOrDefault(i=>i.Id==id);
            if (selitem != null)
            {
                SelectItems = selitem;
                Description = selitem.Description;
                Name = selitem.Name;
                Cost = $"Цена покупки {selitem.Cost}";
                var itemsDB = await context.UserItems.CountAsync(i=>i.UserId==App.Identity.Id&&i.ItemId==selitem.Id);
                CountItems = $"У вас есть {itemsDB}";
            }
        }

        [RelayCommand]
        public async Task BuyItem()
        {
            if (SelectItems==null)
            {
                return;
            }

            var user=context.Users.FirstOrDefault(u=>u.Id==App.Identity.Id);

            if (user == null) return;

            if (user.Points < SelectItems.Cost) return;
            user.Points-=SelectItems.Cost;
            App.Identity = user;
            // await context.Items.AddAsync(SelectItems);
            var userItem = new UserItems();
            userItem.UserId = App.Identity.Id;
            userItem.ItemId=SelectItems.Id;
            await context.UserItems.AddAsync(userItem);
            await context.SaveChangesAsync();
        }

    }
}
